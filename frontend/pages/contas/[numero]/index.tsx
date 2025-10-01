import Link from 'next/link';
import { useRouter } from 'next/router';
import useSWR from 'swr';
import { fetcher } from '@/lib/fetcher';
import { normalizeContaDetalhe } from '@/lib/adapters';
import type { ContaDetalhe, ContaDetalheApi } from '@/types/conta';

const currencyFormatter = new Intl.NumberFormat('pt-BR', {
  style: 'currency',
  currency: 'BRL',
});

export default function ContaDetalhar() {
  const router = useRouter();
  const { numero } = router.query;

  const shouldFetch = typeof numero === 'string' && numero.length > 0;
  const { data, error, isLoading } = useSWR<ContaDetalheApi>(
    shouldFetch ? `/api/contas/${numero}` : null,
    fetcher,
  );

  if (error) {
    return <div className="alert error">{error.message}</div>;
  }

  if (isLoading || !data) {
    return <div>Carregando ...</div>;
  }

  const conta = normalizeContaDetalhe(data);

  return (
    <>
      <header>
        <h1>Conta</h1>
      </header>
      <div className="card">
        <p>
          <strong>Número:</strong>
          {conta.numero}
        </p>
        <p>
          <strong>Cliente:</strong>
          {conta.nomeCliente}
        </p>
        <p>
          <strong>Saldo:</strong>
          {currencyFormatter.format(conta.saldo)}
        </p>
        <p>
          <strong>Criada em:</strong>
          {new Date(conta.dataCriacao).toLocaleString('pt-BR')}
        </p>
        <p>
          <strong>Total de transações:</strong>
          {conta.quantidadeTransacoes}
        </p>
      </div>
      <footer>
        <Link href="/contas" className="secondary-btn">
          Retornar
        </Link>
      </footer>
    </>
  );
}
