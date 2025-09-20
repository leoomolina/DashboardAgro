export function formatarArea(valor: number): string {
  if (valor == null) return '-';
  return `${valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })} ha`;
}

export function formatarPeso(valor: number): string {
  if (valor == null) return '-';
  return `${valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })} kg`;
}

export function formatarDinheiro(valor: number): string {
  if (valor == null) return '-';
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', minimumFractionDigits: 2 });
}

export function formatarPercentual(valor: number): string {
  if (valor == null) return '-';
  return `${(valor * 100).toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}%`;
}
