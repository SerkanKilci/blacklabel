export interface DietFlags {
  vegan: boolean;
  vegetarian: boolean;
  glutenFree: boolean;
  lactoseFree: boolean;
  noPalmOil: boolean;
  lowSugar: boolean;
  lowSalt: boolean;
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

export interface HouseholdProfile {
  id: string;
  name: string;
  avoidedAdditiveCodes: string[];
  allergenCodes: string[];
  dietFlags: DietFlags;
}

export type ProfileFormValues = Omit<HouseholdProfile, 'id'>;

export const EMPTY_PROFILE_FORM: ProfileFormValues = {
  name: '',
  avoidedAdditiveCodes: [],
  allergenCodes: [],
  dietFlags: EMPTY_DIET_FLAGS,
};
