#!/bin/sh

set -eu

escape_json() {
  printf '%s' "$1" | sed 's/\\/\\\\/g; s/"/\\"/g'
}

API_BASE_URL_ESCAPED=$(escape_json "${API_BASE_URL:-}")
NEXT_PUBLIC_API_BASE_URL_ESCAPED=$(escape_json "${NEXT_PUBLIC_API_BASE_URL:-}")

cat <<RUNTIME > /app/public/runtime-config.js
window.__ENV = {
  API_BASE_URL: "${API_BASE_URL_ESCAPED}",
  NEXT_PUBLIC_API_BASE_URL: "${NEXT_PUBLIC_API_BASE_URL_ESCAPED}"
};
RUNTIME

exec "$@"
