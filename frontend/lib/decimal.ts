export function parseDecimalInput(value: string): number {
  const sanitized = value.replace(/[^0-9,.-]/g, '');

  if (!sanitized) {
    return 0;
  }

  const hasComma = sanitized.includes(',');
  const hasDot = sanitized.includes('.');
  let normalized = sanitized;

  if (hasComma) {
    // Treat comma as decimal separator and dots as thousand separators
    normalized = sanitized.replace(/\./g, '').replace(',', '.');
  } else if (hasDot) {
    const [integer, ...decimalParts] = sanitized.split('.');
    const decimals = decimalParts.join('');
    normalized = decimals.length > 0 ? `${integer}.${decimals}` : integer;
  }

  const parsed = Number(normalized);
  return Number.isFinite(parsed) ? parsed : 0;
}
