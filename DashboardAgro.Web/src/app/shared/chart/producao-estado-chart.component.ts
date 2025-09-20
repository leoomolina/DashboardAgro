import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartOptions, ChartConfiguration } from 'chart.js';
import { formatarPeso } from '../../shared/utils/format-utils';

@Component({
  selector: 'app-producao-estado-chart',
  standalone: true,
  imports: [CommonModule, BaseChartDirective],
  templateUrl: './producao-estado-chart.component.html'
})
export class ProducaoEstadoChartComponent implements OnChanges {
  @Input() estados: string[] = [];
  @Input() producao: number[] = [];

  public barChartType: 'bar' = 'bar';

  public barChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    plugins: {
      legend: { position: 'top' },
      tooltip: {
        callbacks: {
          label: (context) => {
            const value = context.raw as number;
            return formatarPeso(value);
          }
        }
      }
    },
    scales: {
      x: {},
      y: {
        beginAtZero: true,
        ticks: {
          callback: (value) => formatarPeso(Number(value))
        }
      }
    }
  };

  public barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: this.estados,
    datasets: [
      {
        data: this.producao,
        label: 'Produção',
        backgroundColor: 'rgba(54, 162, 235, 0.7)',
        borderColor: 'rgba(54, 162, 235, 1)',
        borderWidth: 1
      }
    ]
  };

  ngOnChanges(changes: SimpleChanges) {
    if (changes['estados'] || changes['producao']) {
      this.barChartData = {
        labels: this.estados,
        datasets: [
          {
            data: this.producao,
            label: 'Produção',
            backgroundColor: 'rgba(54, 162, 235, 0.7)',
            borderColor: 'rgba(54, 162, 235, 1)',
            borderWidth: 1
          }
        ]
      };
    }
  }
}
