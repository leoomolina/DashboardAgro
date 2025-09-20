import * as L from 'leaflet';
import { Component, Input, OnChanges, AfterViewInit } from '@angular/core';
import * as BrazilTopoJSON from '../../../assets/br-states.json';
import { ResumoEstadoDTO } from '../../core/models/resumo-estado.dto';
import * as turf from '@turf/turf';

@Component({
  selector: 'app-mapa-brasil',
  template: `<div id="mapaBrasil" style="height: 500px; position: relative;"></div>`,
})
export class MapaBrasilComponent implements OnChanges, AfterViewInit {
  @Input() dados: ResumoEstadoDTO[] = [];
  private map!: L.Map;
  private geoJsonLayer!: L.GeoJSON<any>;
  private legendControl!: L.Control; // importante: tipo L.Control

  ngAfterViewInit() {
    this.map = L.map('mapaBrasil', { center: [-14.2350, -51.9253], zoom: 4 });
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.map);

    this.renderMapa();
  }

  ngOnChanges() {
    if (this.map) this.renderMapa();
  }

  private getColor(value: number): string {
    if (value < 1_000_000) return '#f7f3e9';
    if (value < 10_000_000) return '#edd9b0';
    if (value < 50_000_000) return '#e1b86d';
    if (value < 100_000_000) return '#d78f3d';
    if (value < 200_000_000) return '#b9651f';
    return '#7b3f1c';
  }

  private createLegend() {
    // Remove a legenda antiga se existir
    if (this.legendControl) this.legendControl.remove();

    // Criar o controle de legenda usando L.Control
    this.legendControl = new L.Control({ position: 'bottomright' });

    this.legendControl.onAdd = (map: L.Map) => {
      const div = L.DomUtil.create('div', 'info legend');
      const grades = [0, 1_000_000, 10_000_000, 50_000_000, 100_000_000, 200_000_000];
      const labels = ['0 – 1M', '1M – 10M', '10M – 50M', '50M – 100M', '100M – 200M', '> 200M'];

      let html = '<div style="background:white;padding:8px;border-radius:4px;box-shadow:0 0 6px rgba(0,0,0,0.3);">';
      for (let i = 0; i < grades.length; i++) {
        html += `<i style="background:${this.getColor(grades[i] + 1)};width:18px;height:18px;display:inline-block;margin-right:8px;"></i>` + labels[i] + '<br>';
      }
      html += '</div>';
      div.innerHTML = html;
      return div;
    };

    this.legendControl.addTo(this.map);
  }

  private renderMapa() {
    if (this.geoJsonLayer) this.geoJsonLayer.remove();

    const dataMap = this.dados.reduce((acc, item) => {
      acc[item.siglaUf] = item.quantidadeProduzidaTotal;
      return acc;
    }, {} as Record<string, number>);

    const states = (BrazilTopoJSON as any).features;

    this.geoJsonLayer = L.geoJSON(states, {
      style: (feature: any) => ({
        fillColor: this.getColor(dataMap[feature.properties.sigla] ?? 0),
        weight: 1,
        color: '#fff',
        fillOpacity: 0.8,
      }),
      onEachFeature: (feature: any, layer: L.Layer) => {
        const sigla = feature.properties.sigla;
        const value = dataMap[sigla] ?? 0;

        layer.bindTooltip(`${sigla}: ${value.toLocaleString()}`);

        const center = turf.centroid(feature).geometry.coordinates.reverse() as [number, number];

        const marker = L.marker(center, {
          icon: L.divIcon({
            className: 'state-label',
            html: `<b>${sigla}</b>`,
            iconSize: [30, 20],
            iconAnchor: [15, 10]
          }),
          interactive: false
        });

        marker.addTo(this.map);
      }
    });

    this.geoJsonLayer.addTo(this.map);

    this.createLegend();
  }
}
