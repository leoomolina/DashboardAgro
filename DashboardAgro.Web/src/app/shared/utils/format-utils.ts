export function formatarArea(valor: number): string {
  if (valor == null) return '-';
  return `${formatNumber(valor)} ha`;
}

export function formatarPeso(valor: number): string {
  if (valor == null) return '-';
  return `${formatNumber(valor)} t`;
}

export function formatarDinheiro(valor: number): string {
  if (valor == null) return '-';

  if (valor >= 1_000_000_000) {
    return `R$ ${(valor / 1_000_000_000).toFixed(2)} B`;
  }
  if (valor >= 1_000_000) {
    return `R$ ${(valor / 1_000_000).toFixed(2)} M`;
  }

  return valor.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  });
}

export function formatarPercentual(valor: number): string {
  if (valor == null) return '-';
  return `${(valor * 100).toLocaleString('pt-BR', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  })}%`;
}

export function formatNumber(value: number): string {
  if (value >= 1_000_000_000) {
    return (value / 1_000_000_000).toFixed(1) + 'B';
  }
  if (value >= 1_000_000) {
    return (value / 1_000_000).toFixed(1) + 'M';
  }
  return value.toLocaleString('pt-BR', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  });
}
