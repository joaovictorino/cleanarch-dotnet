import type {
  ContaDetalhe,
  ContaDetalheApi,
  ContaResumo,
  ContaResumoApi,
  ResultadoTransferencia,
  ResultadoTransferenciaApi,
} from '@/types/conta';

function pick<T>(...values: Array<T | undefined | null>): T | undefined {
  for (const value of values) {
    if (value !== undefined && value !== null) {
      return value;
    }
  }
  return undefined;
}

function coerceString(value: unknown): string | undefined {
  if (typeof value === 'string') {
    const trimmed = value.trim();
    if (!trimmed || trimmed.toLowerCase() === 'undefined' || trimmed.toLowerCase() === 'null') {
      return undefined;
    }
    return trimmed;
  }

  if (typeof value === 'number' && Number.isFinite(value)) {
    return value.toString();
  }

  return undefined;
}

function coerceNumber(value: unknown): number | undefined {
  if (typeof value === 'number' && Number.isFinite(value)) {
    return value;
  }

  if (typeof value === 'string') {
    const normalized = value.replace(/[^0-9,.-]/g, '').replace(',', '.');
    const parsed = Number(normalized);
    if (Number.isFinite(parsed)) {
      return parsed;
    }
  }

  return undefined;
}

function coerceDate(value: unknown): string | undefined {
  const stringValue = coerceString(value);
  if (!stringValue) {
    return undefined;
  }

  const date = new Date(stringValue);
  return Number.isNaN(date.getTime()) ? undefined : date.toISOString();
}

export function normalizeContaResumo(conta: ContaResumoApi): ContaResumo {
  const numero = coerceString(pick(conta.numero, conta.Numero)) ?? '';
  const nomeCliente = coerceString(pick(conta.nomeCliente, conta.NomeCliente)) ?? '';
  const saldo = coerceNumber(pick(conta.saldo, conta.Saldo)) ?? 0;
  const dataCriacao = coerceDate(pick(conta.dataCriacao, conta.DataCriacao)) ?? new Date().toISOString();

  return {
    numero,
    nomeCliente,
    saldo,
    dataCriacao,
  };
}

export function normalizeContaDetalhe(conta: ContaDetalheApi): ContaDetalhe {
  const quantidadeTransacoes =
    coerceNumber(pick(conta.quantidadeTransacoes, conta.QuantidadeTransacoes)) ?? 0;

  return {
    ...normalizeContaResumo(conta),
    quantidadeTransacoes,
  };
}

export function normalizeResultadoTransferencia(
  resultado: ResultadoTransferenciaApi | null,
): ResultadoTransferencia | null {
  if (!resultado) {
    return null;
  }

  const codigoTransacao = coerceString(
    pick(resultado.CodigoTransacao, resultado.codigoTransacao),
  );
  const dataHoraTransacao = coerceDate(
    pick(resultado.DataHoraTransacao, resultado.dataHoraTransacao),
  );

  return {
    CodigoTransacao: codigoTransacao ?? '',
    DataHoraTransacao: dataHoraTransacao ?? '',
  };
}
