import { authorizedGet, authorizedPost } from './authorizedClient';
import type { Subscription } from '../types/subscription';

export function getSubscription(): Promise<Subscription> {
  return authorizedGet<Subscription>('/me/subscription');
}

/** Dev-only: the backend returns 404 outside Development regardless of build. See Settings. */
export function debugGrantPremium(): Promise<Subscription> {
  return authorizedPost<Subscription>('/me/debug-grant-premium', {});
}
