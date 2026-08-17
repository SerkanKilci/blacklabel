export interface DietFlags {
  vegan: boolean;
  vegetarian: boolean;
  glutenFree: boolean;
  lactoseFree: boolean;
  noPalmOil: boolean;
  lowSugar: boolean;
  lowSalt: boolean;
}

export interface UserPreferences {
  avoidedAdditiveCodes: string[];
  allergenCodes: string[];
  dietFlags: DietFlags;
}

export const EMPTY_DIET_FLAGS: DietFlags = {
  vegan: false,
  vegetarian: false,
  glutenFree: false,
  lactoseFree: false,
  noPalmOil: false,
  lowSugar: false,
  lowSalt: false,
};

export const EMPTY_PREFERENCES: UserPreferences = {
  avoidedAdditiveCodes: [],
  allergenCodes: [],
  dietFlags: EMPTY_DIET_FLAGS,
};
