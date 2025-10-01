import { FormEvent, useState } from 'react';
import Link from 'next/link';
import { useRouter } from 'next/router';
import useSWR from 'swr';
import { fetcher } from '@/lib/fetcher';
import { buildBackendUrl } from '@/lib/backendClient';
import { normalizeContaDetalhe, normalizeResultadoTransferencia } from '@/lib/adapters';
import { parseDecimalInput } from '@/lib/decimal';
import type { ContaDetalheApi, ResultadoTransferenciaApi } from '@/types/conta';

export default function ContaTransferir() {
  const router = useRouter();
  const { numero } = router.query;

  const shouldFetch = typeof numero === 'string' && numero.length > 0;
  const { data, error, isLoading } = useSWR<ContaDetalheApi>(
    shouldFetch ? `/api/contas/${numero}` : null,
    fetcher,
  );

  const [formState, setFormState] = useState({
    numeroContaDestino: '',
    valor: 0,
  });
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  if (error) {
    return <div className="alert error">{error.message}</div>;
  }

  if (isLoading || !data) {
    return <div>Carregando ...</div>;
  }

  const conta = normalizeContaDetalhe(data);

  const handleChange = (field: 'numeroContaDestino' | 'valor') => (event: React.ChangeEvent<HTMLInputElement>) => {
    if (field === 'valor') {
      setFormState((prev) => ({ ...prev, valor: parseDecimalInput(event.target.value) }));
      return;
    }

    setFormState((prev) => ({ ...prev, [field]: event.target.value }));
  };

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setErrorMessage(null);

    if (!Number.isFinite(formState.valor) || formState.valor <= 0) {
      setErrorMessage('Informe um valor válido para transferir.');
      return;
    }

    if (!formState.numeroContaDestino.trim()) {
      setErrorMessage('Informe a conta de destino.');
      return;
    }

    const response = await fetch(buildBackendUrl('/transferir'), {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        NumeroContaOrigem: conta.numero,
        NumeroContaDestino: formState.numeroContaDestino,
        Valor: formState.valor,
      }),
    });

    const body = (await response.json().catch(() => null)) as
      | ResultadoTransferenciaApi
      | { Mensagem?: string }
      | null;

    if (response.ok) {
      const recibo = normalizeResultadoTransferencia(body as ResultadoTransferenciaApi | null);
      const codigo = recibo?.CodigoTransacao ?? '';
      const dataHora = recibo?.DataHoraTransacao
        ? new Date(recibo.DataHoraTransacao).toLocaleString('pt-BR')
        : '';
      alert(`Transferência realizada! Código: ${codigo}\nData/Hora: ${dataHora}`);
      router.push('/contas');
      return;
    }

    const message = (body as { Mensagem?: string } | null)?.Mensagem ?? 'Erro ao realizar transferência.';
    setErrorMessage(message);
  }

  return (
    <>
      <header>
        <h1>Transferir</h1>
      </header>
      {errorMessage && <div className="alert error">{errorMessage}</div>}
      <form onSubmit={handleSubmit}>
        <div className="card">
          <p>
            <strong>De:</strong>
            {conta.numero}
          </p>
          <div>
            <label htmlFor="numeroContaDestino">Para:</label>
            <input
              type="text"
              id="numeroContaDestino"
              value={formState.numeroContaDestino}
              onChange={handleChange('numeroContaDestino')}
              required
            />
          </div>
          <div>
            <label htmlFor="valor">Valor (R$):</label>
            <input
              type="text"
              id="valor"
              value={formState.valor.toFixed(2).replace('.', ',')}
              onChange={handleChange('valor')}
              required
            />
          </div>
        </div>
        <footer>
          <button type="submit" className="primary-btn">
            Salvar
          </button>
          <Link href="/contas" className="secondary-btn">
            Retornar
          </Link>
        </footer>
      </form>
    </>
  );
}
