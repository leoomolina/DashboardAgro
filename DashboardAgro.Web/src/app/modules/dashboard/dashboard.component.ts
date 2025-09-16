import { Component } from '@angular/core';
import { CardComponent } from '../../shared/card/card.component';
import { CommonModule } from '@angular/common';
import { DashboardFiltersComponent } from '../dashboard-filters/dashboard-filters.component'

export interface Producao {
  cultura: string;
  areaPlantada: number;
  areaColhida: number;
  valorProducao: number;
}

const ELEMENT_DATA: Producao[] = [
  {cultura: 'Soja', areaPlantada: 1000, areaColhida: 950, valorProducao: 500000},
  {cultura: 'Milho', areaPlantada: 800, areaColhida: 780, valorProducao: 300000},
  {cultura: 'Trigo', areaPlantada: 500, areaColhida: 480, valorProducao: 200000},
];


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    CardComponent,
    DashboardFiltersComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  displayedColumns: string[] = ['cultura', 'areaPlantada', 'areaColhida', 'valorProducao'];
  dataSource = ELEMENT_DATA;

  getTotalValorProducao(): number {
  return this.dataSource.reduce((a, b) => a + b.valorProducao, 0);
}

getTotalAreaPlantada(): number {
  return this.dataSource.reduce((a, b) => a + b.areaPlantada, 0);
}

getTotalAreaColhida(): number {
  return this.dataSource.reduce((a, b) => a + b.areaColhida, 0);
}

  filtroCultura: string = '';
}
