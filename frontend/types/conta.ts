export interface ContaResumo {
  numero: string;
  nomeCliente: string;
  saldo: number;
  dataCriacao: string;
}

export interface ContaDetalhe extends ContaResumo {
  quantidadeTransacoes: number;
}

export type ContaResumoApi = ContaResumo & {
  Numero?: string;
  NomeCliente?: string;
  Saldo?: number;
  DataCriacao?: string;
};

export type ContaDetalheApi = ContaResumoApi & {
  quantidadeTransacoes?: number;
  QuantidadeTransacoes?: number;
};

export interface CriarContaPayload {
  NumeroConta: string;
  NomeCliente: string;
  SaldoInicial: number;
}

export interface TransferenciaPayload {
  NumeroContaOrigem: string;
  NumeroContaDestino: string;
  Valor: number;
}

export interface ResultadoTransferencia {
  CodigoTransacao: string;
  DataHoraTransacao: string;
}

export type ResultadoTransferenciaApi = Partial<ResultadoTransferencia> & {
  CodigoTransacao?: string;
  DataHoraTransacao?: string;
  codigoTransacao?: string;
  dataHoraTransacao?: string;
};

export interface ApiError {
  Mensagem: string;
}
