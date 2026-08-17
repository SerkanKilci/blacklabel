# Privacy Policy — Blacklabel

**Draft — not yet reviewed by counsel.** Replace the placeholder contact address below with a
real one, have this reviewed against applicable law in your target markets (KVKK in Turkey,
GDPR in the EU, App Store/Play Store privacy requirements), and host it at a public URL before
submitting the app to either store — both require a live Privacy Policy URL in the listing.

Last updated: 2026-08-14.

## What this app does

Blacklabel scans packaged-food barcodes and shows a 0–100 score based on ingredients, additives,
and nutrition. It does not provide medical advice or diagnose anything.

## Data we collect

- **Device identifier.** On first launch, the app generates a random identifier stored on your
  device and sends it to our server to create an anonymous account. It is not tied to your name,
  email, or any other personal identity unless you separately choose to link an Apple or Google
  account.
- **Scan history.** The barcodes you scan, when you scanned them, and the resulting score are
  stored so you can see your own history. This is tied to your anonymous account, not shared with
  other users.
- **Contributed photos.** If you photograph a product's packaging to help add it to the database
  (front, ingredients list, nutrition table), those images are uploaded to our server and sent to
  a third-party vision/OCR service to extract text. They are retained to allow review of
  contributed data.
- **Preferences.** Allergens you flag and dietary preferences you set, used only to show you
  personal warnings on products you scan.
- **Subscription status.** If you subscribe to premium, our payments provider (RevenueCat) shares
  your subscription status and entitlement with us so we can unlock premium features. We do not
  receive your payment card details — those are handled by Apple/Google/RevenueCat directly.

We do not collect precise location, contacts, or browsing history outside this app.

## Third parties we share data with

- **Open Food Facts** — barcode and product lookups are sent to Open Food Facts' public API
  (openfoodfacts.org) to retrieve product data. Product data returned by Open Food Facts is
  licensed under the Open Database License (ODbL); see the in-app "Data Sources" page.
- **Vision/OCR provider** — package photos you submit are sent to a third-party service to extract
  text. No photo is sent anywhere unless you actively choose to contribute a product.
- **RevenueCat** — manages in-app subscriptions and reports purchase/entitlement events to us.
- We do not sell your data, and we do not use it for advertising.

## Data retention and deletion

Your scan history, preferences, and contributed photos are retained as long as your account
exists. You can permanently delete your account and all associated data at any time from
Settings → Delete my account (`DELETE /api/v1/me`), or contact us (see below) with your device
identifier, visible in Settings. Deletion removes your scan history, preferences, and
contributions from our database immediately; uploaded contribution images may persist on disk
briefly afterward (a known gap — see `README.md`'s account-deletion notes).

## Children's privacy

This app is not directed at children and does not knowingly collect data from children.

## Your rights

Depending on where you live, you may have rights to access, correct, or delete your data. Contact
us to exercise these rights.

## Changes to this policy

We may update this policy as the app changes. Material changes will be reflected in the app's
"Data Sources & Privacy" page.

## Contact

privacy@blacklabel.app *(placeholder — replace before publishing)*
