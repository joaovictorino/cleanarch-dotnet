import { FormEvent, useState } from 'react';
import { useRouter } from 'next/router';
import Link from 'next/link';
import { buildBackendUrl } from '@/lib/backendClient';
import { parseDecimalInput } from '@/lib/decimal';

interface FormState {
  numeroConta: string;
  nomeCliente: string;
  saldoInicial: number;
}

export default function ContaCriar() {
  const [formState, setFormState] = useState<FormState>({
    numeroConta: '',
    nomeCliente: '',
    saldoInicial: 0,
  });
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const router = useRouter();

  const handleChange = (field: keyof FormState) => (event: React.ChangeEvent<HTMLInputElement>) => {
    if (field === 'saldoInicial') {
      setFormState((prev) => ({ ...prev, saldoInicial: parseDecimalInput(event.target.value) }));
      return;
    }

    setFormState((prev) => ({ ...prev, [field]: event.target.value }));
  };

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setErrorMessage(null);

    const response = await fetch(buildBackendUrl('/api/contas'), {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        NumeroConta: formState.numeroConta,
        NomeCliente: formState.nomeCliente,
        SaldoInicial: formState.saldoInicial,
      }),
    });

    if (response.ok) {
      alert('Conta criada!');
      router.push('/contas');
      return;
    }

    try {
      const data = await response.json();
      setErrorMessage(data?.Mensagem ?? 'Erro ao criar conta.');
    } catch (error) {
      setErrorMessage('Erro ao criar conta.');
    }
  }

  return (
    <>
      <header>
        <h1>Cadastrar Conta</h1>
      </header>
      {errorMessage && <div className="alert error">{errorMessage}</div>}
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="numeroConta">Número:</label>
          <input
            type="text"
            id="numeroConta"
            maxLength={20}
            value={formState.numeroConta}
            onChange={handleChange('numeroConta')}
            required
          />
        </div>
        <div>
          <label htmlFor="nomeCliente">Nome do cliente:</label>
          <input
            type="text"
            id="nomeCliente"
            value={formState.nomeCliente}
            onChange={handleChange('nomeCliente')}
            required
          />
        </div>
        <div>
          <label htmlFor="saldoInicial">Saldo inicial (R$):</label>
          <input
            type="text"
            id="saldoInicial"
            value={formState.saldoInicial.toFixed(2).replace('.', ',')}
            onChange={handleChange('saldoInicial')}
            required
          />
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
