import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartOptions, ChartConfiguration } from 'chart.js';
import { formatarPeso } from '../../shared/utils/format-utils';
import { LoaderComponent } from '../loader/loader.component';

@Component({
  selector: 'app-producao-estado-chart',
  standalone: true,
  imports: [CommonModule, BaseChartDirective, LoaderComponent],
  templateUrl: './producao-estado-chart.component.html'
})
export class ProducaoEstadoChartComponent implements OnChanges {
  @Input() estados: string[] = [];
  @Input() producaoTemporaria: number[] = [];
  @Input() producaoPermanente: number[] = [];
  @Input() isLoading: boolean = false;

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
        data: this.producaoPermanente,
        label: 'Produção Permanente',
        backgroundColor: 'rgba(54, 162, 235, 0.7)',
        borderColor: 'rgba(54, 162, 235, 1)',
        borderWidth: 1
      },
      {
        data: this.producaoTemporaria,
        label: 'Produção permanente',
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
            data: this.producaoPermanente,
            label: 'Produção Permanente',
            backgroundColor: 'rgba(79, 70, 229, 0.7)',
            borderColor: 'rgba(79, 70, 229, 1)',
            borderWidth: 1
          },
          {
            data: this.producaoTemporaria,
            label: 'Produção Temporária',
            backgroundColor: 'rgba(245, 158, 11, 0.7)',
            borderColor: 'rgba(245, 158, 11, 1)',
            borderWidth: 1
          }
        ]
      };
    }
  }
}
