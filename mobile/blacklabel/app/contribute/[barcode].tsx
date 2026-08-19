import { Redirect } from 'expo-router';

// User-submitted contributions are pulled from the v1 front end: new products go live with no
// moderation step, and uploaded photos are publicly hosted with no reporting/filtering — a real
// App Store Guideline 1.2 (User Generated Content) gap, not just a nice-to-have. The backend
// endpoint (POST /products/{barcode}/contribute) is left in place for a v2 that adds real
// moderation; the previous camera-capture implementation is in git history, not deleted, just no
// longer wired to any route a user can reach.
export default function ContributeScreen() {
  return <Redirect href="/" />;
}
