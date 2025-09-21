import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RankingItemDTO } from '../../../../core/models/ranking-dto';
import { formatarPeso } from '../../../../shared/utils/format-utils';

@Component({
  selector: 'app-ranking',
  templateUrl: './ranking.component.html',
  imports: [CommonModule]
})
export class RankingComponent implements OnChanges {
  ngOnChanges(changes: SimpleChanges): void {
    this.itens.forEach((item, key) => {
      item.posicao = key + 1;
    });
  }
  @Input() titulo: string = 'Ranking';
  @Input() itens: RankingItemDTO[] = [];

  formatarPeso = formatarPeso;

  getBadgeColor(posicao: number): string {
    switch (posicao) {
      case 1:
        return 'bg-orange-400 text-white';
      case 2:
        return 'bg-yellow-400 text-white';
      case 3:
        return 'bg-blue-300 text-white';
      default:
        return 'bg-gray-200 text-gray-800';
    }
  }
}