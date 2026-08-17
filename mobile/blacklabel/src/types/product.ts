export type ProductSource = 'OpenFoodFacts' | 'Ocr' | 'Manual';
export type DataQuality = 'Complete' | 'Partial' | 'Unverified';

export interface ScoreBreakdown {
  nutrition: number | null;
  additives: number | null;
  processing: number | null;
}

export interface AdditiveInfo {
  code: string;
  nameTr: string;
  nameEn: string;
  category: string;
  riskLevel: number;
  descriptionTr: string;
  descriptionEn: string;
  sourceNote: string | null;
}

export interface Nutriments {
  energyKcal100g: number | null;
  fat100g: number | null;
  saturatedFat100g: number | null;
  carbohydrates100g: number | null;
  sugars100g: number | null;
  fiber100g: number | null;
  proteins100g: number | null;
  salt100g: number | null;
}

export interface PersonalWarning {
  type: string;
  code: string;
  messageKey: string;
}

export type ComparisonLevel = 'Good' | 'Medium' | 'Bad';

/** "Good"/"Medium"/"Bad"/null per category — derived server-side from the exact same
 * thresholds used for `score`, so these labels can never disagree with the score itself. */
export interface ComparisonBands {
  sugar: ComparisonLevel | null;
  saturatedFat: ComparisonLevel | null;
  salt: ComparisonLevel | null;
  additives: ComparisonLevel | null;
}

export interface ProductFound {
  found: true;
  barcode: string;
  name: string;
  brand: string | null;
  imageUrl: string | null;
  score: number | null;
  scoreBreakdown: ScoreBreakdown | null;
  novaGroup: number | null;
  nutriScore: string | null;
  ingredientsText: string | null;
  additives: AdditiveInfo[];
  allergens: string[];
  nutriments: Nutriments | null;
  personalWarnings: PersonalWarning[];
  dataQuality: DataQuality;
  source: ProductSource;
  comparisonBands: ComparisonBands;
}

export interface ProductNotFound {
  found: false;
  canContribute: boolean;
}

export type ProductLookupResponse = ProductFound | ProductNotFound;
