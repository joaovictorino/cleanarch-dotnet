import { buildBackendUrl } from '@/lib/backendClient';

interface ApiErrorResponse {
  Mensagem?: string;
}

export async function fetcher<T>(path: string): Promise<T> {
  const response = await fetch(buildBackendUrl(path));

  if (!response.ok) {
    let message = 'Erro ao carregar dados.';
    try {
      const data: ApiErrorResponse = await response.json();
      if (data?.Mensagem) {
        message = data.Mensagem;
      }
    } catch (error) {
      // ignore JSON parsing errors and use default message
    }
    throw new Error(message);
  }

  return response.json() as Promise<T>;
}
