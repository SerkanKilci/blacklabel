import { authorizedGet } from './authorizedClient';
import type { Additive } from '../types/additive';

export function getAdditives(): Promise<Additive[]> {
  return authorizedGet<Additive[]>('/additives');
}
