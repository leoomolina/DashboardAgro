import { Component, OnInit } from '@angular/core';
import { Chart, LinearScale, Tooltip, Legend } from 'chart.js';
import { ChoroplethController, GeoFeature, ColorScale, ProjectionScale } from 'chartjs-chart-geo';
import * as BrazilTopoJSON from '../../../assets/br-states.json';

Chart.register(
  ChoroplethController,
  GeoFeature,
  ColorScale,
  ProjectionScale,
  LinearScale,
  Tooltip,
  Legend
);

@Component({
  selector: 'app-mapa-brasil',
  template: `<canvas id="mapaBrasil"></canvas>`,
})
export class MapaBrasilComponent implements OnInit {
  ngOnInit() {
    const states = (BrazilTopoJSON as any).features;

    const data = [
      { id: 'AC', value: Math.floor(Math.random() * 1500) },
      { id: 'AL', value: Math.floor(Math.random() * 1500) },
      { id: 'AP', value: Math.floor(Math.random() * 1500) },
      { id: 'AM', value: Math.floor(Math.random() * 1500) },
      { id: 'BA', value: Math.floor(Math.random() * 1500) },
      { id: 'CE', value: Math.floor(Math.random() * 1500) },
      { id: 'DF', value: Math.floor(Math.random() * 1500) },
      { id: 'ES', value: Math.floor(Math.random() * 1500) },
      { id: 'GO', value: Math.floor(Math.random() * 1500) },
      { id: 'MA', value: Math.floor(Math.random() * 1500) },
      { id: 'MT', value: Math.floor(Math.random() * 1500) },
      { id: 'MS', value: Math.floor(Math.random() * 1500) },
      { id: 'MG', value: Math.floor(Math.random() * 1500) },
      { id: 'PA', value: Math.floor(Math.random() * 1500) },
      { id: 'PB', value: Math.floor(Math.random() * 1500) },
      { id: 'PR', value: Math.floor(Math.random() * 1500) },
      { id: 'PE', value: Math.floor(Math.random() * 1500) },
      { id: 'PI', value: Math.floor(Math.random() * 1500) },
      { id: 'RJ', value: Math.floor(Math.random() * 1500) },
      { id: 'RN', value: Math.floor(Math.random() * 1500) },
      { id: 'RS', value: Math.floor(Math.random() * 1500) },
      { id: 'RO', value: Math.floor(Math.random() * 1500) },
      { id: 'RR', value: Math.floor(Math.random() * 1500) },
      { id: 'SC', value: Math.floor(Math.random() * 1500) },
      { id: 'SP', value: Math.floor(Math.random() * 1500) },
      { id: 'SE', value: Math.floor(Math.random() * 1500) },
      { id: 'TO', value: Math.floor(Math.random() * 1500) }
    ];

    // Mapeia todos os estados para garantir que feature não seja undefined
    const chartData = states.map((s: any) => {
      const d = data.find(item => item.id === s.properties.sigla);
      return {
        feature: s,
        value: d ? d.value : 0
      };
    });

    new Chart('mapaBrasil', {
      type: 'choropleth',
      data: {
        labels: states.map((s: any) => s.properties.sigla),
        datasets: [
          {
            label: 'Produção por Estado',
            outline: states,
            data: chartData,
            backgroundColor: (ctx: any) => {
              const raw = ctx.raw as { feature: any; value: number };
              const val = raw?.value ?? 0;
              if (val < 500) return '#cfe2f3';
              if (val < 1000) return '#6fa8dc';
              return '#08306b';
            }
          }
        ]
      },
      options: {
        showOutline: true,
        scales: {
          projection: {
            axis: 'x',
            projection: 'mercator'
          }
        },
        plugins: {
          tooltip: {
            callbacks: {
              label: ctx => {
                const raw = ctx.raw as { feature: any; value: number };
                return `${raw.feature.properties.sigla}: ${raw.value ?? 0}`;
              }
            }
          },
          legend: { display: false }
        }
      }
    });
  }
}
