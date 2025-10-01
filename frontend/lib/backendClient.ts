const DEFAULT_BASE_URL = 'http://localhost:5071';

declare global {
  interface Window {
    __ENV?: Record<string, string | undefined>;
  }
}

function resolveRuntimeVariable(name: string): string | undefined {
  if (typeof window !== 'undefined') {
    return window.__ENV?.[name];
  }
  return undefined;
}

function resolveBaseUrl(): string {
  const variables = [
    process.env.API_BASE_URL,
    process.env.NEXT_PUBLIC_API_BASE_URL,
    resolveRuntimeVariable('API_BASE_URL'),
    resolveRuntimeVariable('NEXT_PUBLIC_API_BASE_URL'),
  ];

  const base = variables.find((value) => typeof value === 'string' && value.trim().length > 0);
  return base ?? DEFAULT_BASE_URL;
}

export function buildBackendUrl(path: string): string {
  const base = resolveBaseUrl();
  const normalizedPath = path.startsWith('/') ? path : `/${path}`;
  return `${base.replace(/\/$/, '')}${normalizedPath}`;
}
