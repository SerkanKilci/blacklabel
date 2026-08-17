import { ensureAuthToken } from '../auth/deviceAuthClient';
import type { ProductFound } from '../types/product';
import { API_BASE_URL, ApiError } from './client';

export class ContributionFailedError extends Error {
  constructor() {
    super('Vision extraction failed for this contribution.');
    this.name = 'ContributionFailedError';
  }
}

export interface CapturedPhoto {
  uri: string;
  fileName: string;
  mimeType: string;
}

export interface ContributionPhotos {
  front: CapturedPhoto;
  ingredients: CapturedPhoto;
  nutrition: CapturedPhoto;
}

export async function contributeProduct(barcode: string, photos: ContributionPhotos): Promise<ProductFound> {
  const token = await ensureAuthToken();

  const formData = new FormData();
  (Object.keys(photos) as Array<keyof ContributionPhotos>).forEach((slot) => {
    const photo = photos[slot];
    formData.append(
      slot,
      {
        uri: photo.uri,
        name: photo.fileName,
        type: photo.mimeType,
      } as unknown as Blob,
    );
  });

  let response: Response;
  try {
    response = await fetch(`${API_BASE_URL}/products/${encodeURIComponent(barcode)}/contribute`, {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${token}`,
      },
      body: formData,
    });
  } catch {
    throw new ApiError('Network request failed');
  }

  if (response.status === 422) {
    throw new ContributionFailedError();
  }

  if (!response.ok) {
    throw new ApiError(`Contribution request failed with status ${response.status}`, response.status);
  }

  return (await response.json()) as ProductFound;
}
