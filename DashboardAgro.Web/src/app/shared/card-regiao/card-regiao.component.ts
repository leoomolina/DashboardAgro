import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { formatarArea, formatarPeso, formatarDinheiro, formatarPercentual } from '../utils/format-utils';
import { TituloCard } from '../title-card/titulo-card.component';

@Component({
  selector: 'app-regiao-card',
  imports: [CommonModule, TituloCard],
  templateUrl: './card-regiao.component.html',
  standalone: true,
})
export class CardRegiaoComponent {
  @Input() descricao!: string;
  @Input() area!: number;
  @Input() quantidadeProduzida!: number;
  @Input() valorProducao!: number;
  @Input() produtividade!: number;
  @Input() crops!: string[];

  formatarArea = formatarArea;
  formatarPeso = formatarPeso;
  formatarDinheiro = formatarDinheiro;
  formatarPercentual = formatarPercentual;
}
