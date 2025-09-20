import { Component, OnInit } from '@angular/core';
import { CardComponent } from '../../../../shared/card/card.component';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../../core/services/api.service';
import { DashboardFiltersComponent } from '../../components/filters/filter-bar.component';
import { TipoLavouraDTO } from '../../../../core/models/tipo-lavoura.dto';
import { ResumoAnualDTO } from '../../../../core/models/resumo-anual.dto';
import * as FormatUtils from '../../../../shared/utils/format-utils';
import { ProducaoEstadoChartComponent } from '../../../../shared/chart/producao-estado-chart.component';
import { ResumoEstadoDTO } from '../../../../core/models/resumo-estado.dto';
import { UnidadeFederativaDTO } from '../../../../core/models/unidade-federativa.dto';
import { RegiaoBrasilDTO } from '../../../../core/models/regiao-brasil.dto';
import { MapaBrasilComponent } from '../../../../shared/map/mapa-brasil.component';

@Component({
  selector: 'dashboard-page',
  standalone: true,
  imports: [
    CommonModule,
    CardComponent,
    DashboardFiltersComponent,
    ProducaoEstadoChartComponent,
    MapaBrasilComponent
  ],
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.css']
})
export class DashboardPageComponent implements OnInit {
  selectedAno!: number;
  selectedRegiao!: RegiaoBrasilDTO;
  selectedUnidadeFederativa!: UnidadeFederativaDTO;
  selectedTipoLavoura!: TipoLavouraDTO;

  valorTotalArea!: string;
  valorTotalDinheiro!: string;
  valorTotalPeso!: string;

  resumoAnual!: ResumoAnualDTO;
  estados: string[] = [];
  producao: number[] = [];

  loading: boolean = false;

  constructor(private apiService: ApiService) { }
  ngOnInit(): void {
  }

  loadDashboard(): void {
    this.loading = true;

    this.apiService.getResumoAnual(this.selectedAno, this.selectedRegiao.id, this.selectedUnidadeFederativa!.id, this.selectedTipoLavoura.id).subscribe(data => {
      if (data == null)
        return;

      this.resumoAnual = data;

      this.valorTotalArea = FormatUtils.formatarArea(this.resumoAnual.areaColhida);
      this.valorTotalDinheiro = FormatUtils.formatarDinheiro(this.resumoAnual.valorProducao);
      this.valorTotalPeso = FormatUtils.formatarPeso(this.resumoAnual.quantidadeProduzida);
    })

    this.apiService.getResumoPorEstado(this.selectedAno, this.selectedRegiao.id, this.selectedTipoLavoura.id).subscribe((dados: ResumoEstadoDTO[]) => {
      this.estados = dados.map(d => d.siglaUf);
      this.producao = dados.map(d => d.quantidadeProduzida);
    });

    this.loading = false;
  }

  onFiltersApplied(filtros: { ano: number; regiao: RegiaoBrasilDTO; unidadeFederativa?: UnidadeFederativaDTO; tipoLavoura: TipoLavouraDTO; }) {
    this.selectedAno = filtros.ano;
    this.selectedRegiao = filtros.regiao;
    this.selectedUnidadeFederativa = filtros.unidadeFederativa!;
    this.selectedTipoLavoura = filtros.tipoLavoura;

    this.loadDashboard();
  }

  onFilterBarInitialized(filters: any) {
    this.selectedAno = filters.ano;
    this.selectedTipoLavoura = filters.tipoLavoura;
    this.selectedRegiao = filters.regiao;
    this.selectedUnidadeFederativa = filters.uf;

    // chama o loadDashboard com os valores do filter-bar
    this.loadDashboard();
  }
}
