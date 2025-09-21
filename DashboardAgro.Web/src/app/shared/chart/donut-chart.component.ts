import { Component, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { formatarArea, formatarDinheiro } from '../utils/format-utils';

interface Lavoura {
  descricao: string;
  areaColhida: number;
  valorProducao: number;
}

@Component({
  selector: 'app-donut-chart',
  templateUrl: './donut-chart.component.html',
  standalone: true,
  imports: [BaseChartDirective],
})
export class DonutChartComponent implements OnChanges {
  @Input() lavouras: Lavoura[] = [];
  @Input() tipo: 'area' | 'valorProducao' = 'area';
  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  public donutChartData: ChartData<'doughnut'> = {
    labels: [],
    datasets: [{ data: [], backgroundColor: [] }],
  };

  public donutChartOptions: ChartOptions<'doughnut'> = {
    responsive: true,
    plugins: {
      legend: { position: 'bottom' },
      tooltip: {
        callbacks: {
          label: (tooltipItem) => {
            const value = tooltipItem.raw as number;
            return this.tipo === 'area'
              ? formatarArea(value)
              : formatarDinheiro(value);
          },
        },
      },
    },
  };

  public donutChartType: 'doughnut' = 'doughnut';

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.lavouras || this.lavouras.length === 0) return;

    this.donutChartData.labels = this.lavouras.map(l => l.descricao);

    const dataValues = this.lavouras.map(l =>
      this.tipo === 'area' ? l.areaColhida : l.valorProducao
    );

    this.donutChartData.datasets = [
      {
        data: dataValues,
        backgroundColor: ['#4f46e5', '#f59e0b', '#10b981', '#ef4444', '#3b82f6'],
      },
    ];

    setTimeout(() => this.chart?.update(), 0);
  }
}
