import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RankingItemDTO } from '../../../../core/models/ranking-dto';

@Component({
    selector: 'app-ranking',
    templateUrl: './ranking.component.html',
    imports: [CommonModule]
})
export class RankingComponent {
  @Input() titulo: string = 'Ranking';
  @Input() itens: RankingItemDTO[] = [];

  private colorMap: Record<string, string> = {};

  getBadgeColor(posicao: number): string {
    switch (posicao) {
      case 1:
        return 'bg-yellow-400 text-white';
      case 2:
        return 'bg-gray-300 text-gray-700';
      case 3:
        return 'bg-orange-400 text-white';
      default:
        return 'bg-gray-200 text-gray-800';
    }
  }

  getEtiquetaColor(etiqueta: string): string {
    if (!this.colorMap[etiqueta]) {
      this.colorMap[etiqueta] = this.generateRandomColor();
    }
    return this.colorMap[etiqueta];
  }

  private generateRandomColor(): string {
    // cores mais suaves (HSL → tons claros)
    const hue = Math.floor(Math.random() * 360); // 0–360
    return `hsl(${hue}, 70%, 70%)`;
  }
}