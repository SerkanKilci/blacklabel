# Store listing draft — Blacklabel

Draft copy for App Store Connect / Google Play Console, written to stay inside §9's language
rules (no diagnosis/treatment/health-claim wording anywhere a reviewer or user can see it —
that includes this metadata, not just in-app text). Replace placeholders, get this reviewed
alongside `PRIVACY.md` and `TERMS.md`, and keep future edits to this copy in the same register.

## Category

**Primary: Food & Drink.** Do not list under Health & Fitness or Medical — those categories
trigger stricter medical-claim review on both stores, and this app explicitly doesn't diagnose,
treat, or give medical advice (§9). Food & Drink accurately describes what the app does: look up
a packaged product and show ingredient/additive/nutrition information.

**Secondary (optional):** Lifestyle / Shopping.

## Age rating

**4+ / Everyone.** No objectionable content, no user-generated content of any kind in v1 (the
photo-contribution feature is pulled from the front end — see the README's milestones section;
the backend endpoint still exists but nothing in the app links to it), no gambling, no violence.
Apple's questionnaire and Play's Data Safety form should both reflect: camera access (for barcode
scanning only), no ads, no third-party analytics/ad SDKs, in-app purchases present
(RevenueCat-managed subscription).

**Note for whoever submits:** Apple replaced its old 4+/9+/12+/17+ scale with a new
4+/9+/13+/16+/18+ system (rolled out with iOS 26) — App Store Connect's questionnaire now asks a
different/expanded set of content questions than older guides describe. Re-answer it fresh rather
than copying a rating from an older reference; 4+ is still the expected outcome here (no
objectionable content of any kind), but don't skip re-filling the form on the assumption nothing
changed.

**Child data note (relevant to household profiles, not age rating itself):** the app is not
directed at children and doesn't verify anyone's identity or age — the household-profiles feature
just lets the *account holder* (an adult, by virtue of registering the account) label a profile
with a first name and preferences (e.g. a child's allergy) for their own use. No birthdate, no
separate login, no data collected *from* a child directly. This is a reasonable COPPA/child-privacy
posture but is exactly the kind of judgment call that needs the counsel review already flagged for
`PRIVACY.md` — call this feature out specifically when that review happens.

## App Store

**Subtitle (30 chars max):** `Barcode & ingredient scanner`

**Promotional text (170 chars, editable without review):**
> Scan any packaged food's barcode to see its ingredients, additives, and a clear 0–100 score —
> instantly, in Turkish or English.

**Description:**
> Blacklabel scans a product's barcode and shows you what's actually in it: full ingredient list,
> flagged additives with plain-language explanations, nutrition breakdown, and a 0–100 score
> that summarizes all of it at a glance.
>
> WHAT YOU GET
> • Instant barcode scanning — point your camera, get the full picture
> • Every additive explained — what it is, what it's used for, and any EFSA intake limits on file
> • A 0–100 score blending nutrition, additives, and processing level
> • Separate allergen and diet warnings for every person in your household — one scan, a result
>   for each family member (Premium)
> • Better-scoring alternatives in the same category (Premium)
> • Your scan history, searchable, on or offline
>
> Product data comes from Open Food Facts, an open, community-maintained database.
>
> Blacklabel is an information tool, not a medical device. It does not diagnose conditions,
> recommend treatment, or replace advice from a qualified healthcare professional. Always check a
> product's official packaging.

**Keywords (100 chars, comma-separated, no spaces after commas):**
`barcode,scanner,ingredients,additives,e-numbers,nutrition,food,label,allergen,grocery`

## Google Play — Health apps declaration

Separate from category selection (Play Console → App content → Health Content and Services): any
app offering "health-related features or information" must complete this form, regardless of
which store category it's listed under. Blacklabel's additive/allergen warnings, diet flags
(low-sugar, low-salt, gluten-free, etc.), and 0–100 score put it inside that definition even
though it's listed under Food & Drink, not Health & Fitness — don't skip this form assuming the
category choice exempts it.

Draft answers for the declaration questionnaire:
- **Does the app provide health information or features?** Yes — nutrition/ingredient scoring,
  allergen and dietary-preference warnings.
- **Does it diagnose, treat, cure, or prevent a disease/condition?** No.
- **Does it claim regulatory clearance (FDA/CE) or a "medical device" status?** No — the standard
  disclaimer already in `common.medicalDisclaimer` ("this app does not provide medical advice and
  is not a substitute for a nutrition professional") covers this; keep using it in-app and reuse
  the same wording in the Play Console form's free-text fields if asked to describe the app.
- **Does it read/write Android Health Connect data?** No — Blacklabel has no Health Connect
  integration; all preference data (allergens, diet flags) is entered manually and stored in our
  own backend, not Android's health data platform.
- **Nutrition/diet-tracking sub-category?** Closest fit is "apps focused on specific dietary needs"
  (allergen/ingredient avoidance) — not calorie counting or weight-management, so don't select
  those sub-options even though they're the most common examples in Play's own guidance.

Also note: as of January 2026, Play Console requires health-declared apps to submit under a
**verified Organization account**, not an individual developer account — factor that into which
Play Console account type gets set up before submission.

## Google Play

**Short description (80 chars max):**
> Scan barcodes to see ingredients, additives, and a 0–100 product score.

**Full description:** same body copy as the App Store description above (Play allows longer, but
there's no need to pad it — keep the register identical between stores).

## Screenshots / preview video

Every screenshot caption and any preview-video on-screen text must stay inside the same rules as
in-app copy: no "healthy/unhealthy", no disease or symptom names, no "safe to eat" framing. Lead
with what's literally on screen — "See every additive explained", "Your 0–100 score, broken
down", "Flag allergens you care about" — not health outcomes.

## What NOT to say anywhere in store metadata (§9)

Don't use, in any language: kanserojen / zehirli / zararlı / hastalık yapar, or their English
equivalents (carcinogenic, toxic, harmful, causes disease), or any phrasing that implies
diagnosis, treatment, prevention, or cure. When metadata needs to reference a risk, use the same
register as the in-app additive copy: "EFSA has set a daily intake limit", "associated with in
some studies", "reported in sensitive individuals" — never a direct causal health claim.
