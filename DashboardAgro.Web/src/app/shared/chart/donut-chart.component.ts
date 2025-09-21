import { AfterViewInit, Component, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

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
    },
  };

  // Tipo literal fixo 'doughnut'
  public donutChartType: 'doughnut' = 'doughnut';

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.lavouras || this.lavouras.length === 0) return;

    this.donutChartData.labels = this.lavouras.map(l => l.descricao);

    if (this.tipo === 'area') {
      this.donutChartData.datasets = [
        {
          data: this.lavouras.map(l => l.areaColhida),
          backgroundColor: ['#4f46e5', '#f59e0b', '#10b981', '#ef4444', '#3b82f6'],
        },
      ];
    } else {
      this.donutChartData.datasets = [
        {
          data: this.lavouras.map(l => l.valorProducao),
          backgroundColor: ['#4f46e5', '#f59e0b', '#10b981', '#ef4444', '#3b82f6'],
        },
      ];
    }

    setTimeout(() => this.chart?.update(), 0);
  }
}
