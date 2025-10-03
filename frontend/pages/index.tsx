import Link from 'next/link';

export default function Dashboard() {
  return (
    <>
      <header>
        <h1>Sistema Bancário Exemplo</h1>
      </header>
      <div className="table">
        <div className="row header">
          <div className="cell">Ações</div>
        </div>
        <div className="row">
          <div className="cell" data-title="resource">
            <Link href="/contas" className="secondary-btn">
              Acessar Contas
            </Link>
          </div>
        </div>
      </div>
    </>
  );
}
