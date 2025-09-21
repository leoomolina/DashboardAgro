import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartOptions, ChartConfiguration } from 'chart.js';
import { formatarPeso } from '../../shared/utils/format-utils';
import { LoaderComponent } from '../loader/loader.component';
import { ResumoEstadoDTO } from '../../core/models/resumo-estado.dto';
import { TituloCard } from '../title-card/titulo-card.component';

@Component({
  selector: 'app-producao-estado-chart',
  standalone: true,
  imports: [CommonModule, BaseChartDirective, LoaderComponent, TituloCard],
  templateUrl: './producao-estado-chart.component.html'
})
export class ProducaoEstadoChartComponent implements OnChanges {
  @Input() estados: string[] = [];
  @Input() producaoTemporaria: number[] = [];
  @Input() producaoPermanente: number[] = [];
  @Input() isLoading: boolean = false;
  @Input() resumoPorEstado: ResumoEstadoDTO[] = [];

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
    if (this.resumoPorEstado?.length) {
      this.montarGrafico();
    }
  }

  private montarGrafico(): void {
    const labels = this.resumoPorEstado.map(e => e.siglaUf);

    const dataPermanente = this.resumoPorEstado.map(e => {
      const lavoura = e.lavouras.find(l => l.tipoLavoura === 0);
      return lavoura ? lavoura.quantidadeProduzida : 0;
    });

    const dataTemporaria = this.resumoPorEstado.map(e => {
      const lavoura = e.lavouras.find(l => l.tipoLavoura === 1);
      return lavoura ? lavoura.quantidadeProduzida : 0;
    });

    this.barChartData = {
      labels,
      datasets: [
        {
          data: dataPermanente,
          label: 'Lavoura Permanente',
          backgroundColor: 'rgba(79, 70, 229, 0.7)',
          borderColor: 'rgba(79, 70, 229, 1)',
          borderWidth: 1
        },
        {
          data: dataTemporaria,
          label: 'Lavoura Temporária',
          backgroundColor: 'rgba(245, 158, 11, 0.7)',
          borderColor: 'rgba(245, 158, 11, 1)',
          borderWidth: 1
        }
      ]
    };
  }
}
