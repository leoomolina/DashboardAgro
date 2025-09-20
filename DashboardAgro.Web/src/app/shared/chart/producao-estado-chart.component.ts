import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartOptions, ChartConfiguration } from 'chart.js';

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
      title: { display: true, text: 'Produção por Estado' }
    },
    scales: {
      x: {},
      y: { beginAtZero: true }
    }
  };

  public barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: ['SP', 'MG', 'PR', 'RS'],
    datasets: [
      {
        data: [1200, 950, 700, 600],
        label: 'Produção (toneladas)',
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
            label: 'Produção (toneladas)',
            backgroundColor: 'rgba(54, 162, 235, 0.7)',
            borderColor: 'rgba(54, 162, 235, 1)',
            borderWidth: 1
          }
        ]
      };
    }
  }
}
