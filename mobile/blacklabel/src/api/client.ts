export const API_BASE_URL = process.env.EXPO_PUBLIC_API_URL ?? 'http://localhost:5236/api/v1';

export class ApiError extends Error {
  status?: number;

  constructor(message: string, status?: number) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
  }
}

interface RequestOptions {
  token?: string;
}

function buildHeaders(options?: RequestOptions): Record<string, string> {
  const headers: Record<string, string> = { 'Content-Type': 'application/json' };
  if (options?.token) {
    headers.Authorization = `Bearer ${options.token}`;
  }
  return headers;
}

export async function apiGet<T>(path: string, options?: RequestOptions): Promise<T> {
  let response: Response;

  try {
    response = await fetch(`${API_BASE_URL}${path}`, { headers: buildHeaders(options) });
  } catch {
    throw new ApiError('Network request failed');
  }

  if (!response.ok && response.status !== 404) {
    throw new ApiError(`Request failed with status ${response.status}`, response.status);
  }

  return (await response.json()) as T;
}

export async function apiPost<T>(path: string, body: unknown, options?: RequestOptions): Promise<T> {
  let response: Response;

  try {
    response = await fetch(`${API_BASE_URL}${path}`, {
      method: 'POST',
      headers: buildHeaders(options),
      body: JSON.stringify(body),
    });
  } catch {
    throw new ApiError('Network request failed');
  }

  if (!response.ok) {
    throw new ApiError(`Request failed with status ${response.status}`, response.status);
  }

  return (await response.json()) as T;
}

export async function apiPut<T>(path: string, body: unknown, options?: RequestOptions): Promise<T> {
  let response: Response;

  try {
    response = await fetch(`${API_BASE_URL}${path}`, {
      method: 'PUT',
      headers: buildHeaders(options),
      body: JSON.stringify(body),
    });
  } catch {
    throw new ApiError('Network request failed');
  }

  if (!response.ok) {
    throw new ApiError(`Request failed with status ${response.status}`, response.status);
  }

  return (await response.json()) as T;
}

export async function apiDelete(path: string, options?: RequestOptions): Promise<void> {
  let response: Response;

  try {
    response = await fetch(`${API_BASE_URL}${path}`, {
      method: 'DELETE',
      headers: buildHeaders(options),
    });
  } catch {
    throw new ApiError('Network request failed');
  }

  if (!response.ok) {
    throw new ApiError(`Request failed with status ${response.status}`, response.status);
  }
}
