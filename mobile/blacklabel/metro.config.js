// eslint-disable-next-line @typescript-eslint/no-var-requires
const { getDefaultConfig } = require('expo/metro-config');

const config = getDefaultConfig(__dirname);

// expo-sqlite's web worker imports a .wasm module directly; Metro's default asset
// extensions don't include "wasm", which breaks `expo start --web` / `expo export --platform web`.
config.resolver.assetExts.push('wasm');

module.exports = config;
