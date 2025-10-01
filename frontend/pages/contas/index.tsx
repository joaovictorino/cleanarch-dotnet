import Link from 'next/link';
import useSWR from 'swr';
import { fetcher } from '@/lib/fetcher';
import { normalizeContaResumo } from '@/lib/adapters';
import type { ContaResumo, ContaResumoApi } from '@/types/conta';

const currencyFormatter = new Intl.NumberFormat('pt-BR', {
  style: 'currency',
  currency: 'BRL',
});

export default function ContaIndex() {
  const { data, error, isLoading } = useSWR<ContaResumoApi[]>('/api/contas', fetcher);

  if (error) {
    return <div className="alert error">{error.message}</div>;
  }

  if (isLoading || !data) {
    return <div>Carregando ...</div>;
  }

  const contas = data.map(normalizeContaResumo);

  return (
    <>
      <header>
        <h1>Contas</h1>
        <Link href="/contas/criar" className="secondary-btn">
          + Criar nova conta
        </Link>
      </header>
      <div className="table">
        <div className="row header">
          <div className="cell">Número</div>
          <div className="cell">Cliente</div>
          <div className="cell">Saldo</div>
          <div className="cell">Criada em</div>
          <div className="cell">Ações</div>
        </div>
        {contas.map((conta: ContaResumo) => (
          <div className="row" key={conta.numero}>
            <div className="cell" data-title="numero">{conta.numero}</div>
            <div className="cell" data-title="cliente">{conta.nomeCliente}</div>
            <div className="cell" data-title="saldo">{currencyFormatter.format(conta.saldo)}</div>
            <div className="cell" data-title="criada">
              {new Date(conta.dataCriacao).toLocaleDateString('pt-BR')}
            </div>
            <div className="cell actions" data-title="actions">
              <div className="action-buttons">
                <Link
                  href={`/contas/${conta.numero}`}
                  data-testid={`${conta.numero}-ver`}
                  className="secondary-btn small"
                >
                  &#128065; Ver
                </Link>
                <Link
                  href={`/contas/${conta.numero}/transferir`}
                  data-testid={`${conta.numero}-transferir`}
                  className="secondary-btn small"
                >
                  &#9998; Transferir
                </Link>
              </div>
            </div>
          </div>
        ))}
      </div>
      <footer>
        <Link href="/" className="secondary-btn">
          Retornar
        </Link>
      </footer>
    </>
  );
}
