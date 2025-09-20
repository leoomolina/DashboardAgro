import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LavouraDTO } from '../../../../core/models/lavoura-dto';
import { formatarArea, formatarPeso, formatarDinheiro } from '../../../../shared/utils/format-utils';
import { DonutChartComponent } from '../../../../shared/chart/donut-chart.component';

@Component({
    selector: 'app-comparacao-lavouras-card',
    standalone: true,
    imports: [CommonModule, DonutChartComponent],
    templateUrl: './comparacao-lavouras.component.html'
})
export class ComparacaoLavourasCard {
    @Input() lavouras: LavouraDTO[] = [];

    formatarArea = formatarArea;
    formatarPeso = formatarPeso;
    formatarDinheiro = formatarDinheiro;
}
