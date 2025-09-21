import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LavouraDTO } from '../../../../core/models/lavoura-dto';
import { formatarArea, formatarPeso, formatarDinheiro } from '../../../../shared/utils/format-utils';
import { DonutChartComponent } from '../../../../shared/chart/donut-chart.component';
import { LoaderComponent } from '../../../../shared/loader/loader.component';

@Component({
    selector: 'app-comparacao-lavouras-card',
    standalone: true,
    imports: [CommonModule, DonutChartComponent, LoaderComponent],
    templateUrl: './comparacao-lavouras.component.html'
})
export class ComparacaoLavourasCard {
    @Input() lavouras: LavouraDTO[] = [];
    @Input() isLoading: boolean = false;

    formatarArea = formatarArea;
    formatarPeso = formatarPeso;
    formatarDinheiro = formatarDinheiro;
}
