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
import { ComparacaoLavourasCard } from '../../components/comparacao-lavouras/comparacao-lavouras.component';
import { RankingComponent } from '../../components/ranking/ranking.component';
import { RankingItemDTO } from '../../../../core/models/ranking-dto';
import { ProducaoDTO } from '../../../../core/models/producao-dto';

@Component({
  selector: 'dashboard-page',
  standalone: true,
  imports: [
    CommonModule,
    CardComponent,
    DashboardFiltersComponent,
    ProducaoEstadoChartComponent,
    MapaBrasilComponent,
    ComparacaoLavourasCard,
    RankingComponent
  ],
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.css']
})
export class DashboardPageComponent implements OnInit {
  selectedAno!: number;
  selectedRegiao!: RegiaoBrasilDTO;
  selectedUnidadeFederativa!: UnidadeFederativaDTO;
  selectedTipoLavoura!: TipoLavouraDTO;
  selectedProducao!: ProducaoDTO;

  valorTotalArea!: string;
  valorTotalDinheiro!: string;
  valorTotalPeso!: string;

  resumoAnual: ResumoAnualDTO =
    {
      ano: new Date().getFullYear(),
      areaColhidaTotal: 0,
      valorProducaoTotal: 0,
      quantidadeProduzidaTotal: 0,
      lavouras: [
        {
          descricao: "Lavoura Permanente",
          areaColhida: 0,
          quantidadeProduzida: 0,
          valorProducao: 0,
          id: 0,
          areaPlantadaxDestinadaColheita: 0,
          produtividadePercentual: 0
        },
        {
          descricao: 'Lavoura Permanente',
          areaColhida: 0,
          quantidadeProduzida: 0,
          valorProducao: 0,
          id: 0,
          areaPlantadaxDestinadaColheita: 0,
          produtividadePercentual: 0
        }
      ]
    };

  resumoPorEstado: ResumoEstadoDTO[] = [];

  estados: string[] = [];
  producao: number[] = [];

  rankingEstados: RankingItemDTO[] = [
    { posicao: 1, nome: 'Mato Grosso', etiqueta: 'Centro-Oeste', valor: '25.000 ha' },
    { posicao: 2, nome: 'Paraná', etiqueta: 'Sul', valor: '22.000 ha' },
    { posicao: 3, nome: 'Bahia', etiqueta: 'Nordeste', valor: '20.000 ha' },
    { posicao: 4, nome: 'São Paulo', etiqueta: 'Sudeste', valor: '18.500 ha' },
  ];

  rankingCulturas: RankingItemDTO[] = [
    { posicao: 1, nome: 'Soja', etiqueta: '', valor: '40.000 ha' },
    { posicao: 2, nome: 'Milho', etiqueta: '', valor: '30.000 ha' },
    { posicao: 3, nome: 'Café', etiqueta: '', valor: '12.000 ha' },
  ];

  loading: boolean = false;

  constructor(private apiService: ApiService) { }
  ngOnInit(): void {
  }

  loadDashboard(): void {
    this.loading = true;

    this.apiService.getResumoAnual(
      this.selectedAno,
      this.selectedRegiao.id,
      this.selectedUnidadeFederativa!.id,
      this.selectedTipoLavoura.id,
      this.selectedProducao!.id
    ).subscribe(data => {
      if (data == null)
        return;

      this.resumoAnual = data;

      this.valorTotalArea = FormatUtils.formatarArea(this.resumoAnual.areaColhidaTotal);
      this.valorTotalDinheiro = FormatUtils.formatarDinheiro(this.resumoAnual.valorProducaoTotal);
      this.valorTotalPeso = FormatUtils.formatarPeso(this.resumoAnual.quantidadeProduzidaTotal);
    })

    this.apiService.getResumoPorEstado(
      this.selectedAno,
      this.selectedRegiao.id,
      this.selectedTipoLavoura.id,
      this.selectedProducao.id
    ).subscribe((dados: ResumoEstadoDTO[]) => {
      this.resumoPorEstado = dados;

      this.estados = dados.map(d => d.siglaUf);
      this.producao = dados.map(d => d.quantidadeProduzidaTotal);
    });

    this.loading = false;
  }

  onFiltersApplied(
    filtros:
      {
        ano: number;
        regiao: RegiaoBrasilDTO;
        unidadeFederativa?: UnidadeFederativaDTO;
        tipoLavoura: TipoLavouraDTO;
        producao: ProducaoDTO;
      }) {
    this.selectedAno = filtros.ano;
    this.selectedRegiao = filtros.regiao;
    this.selectedUnidadeFederativa = filtros.unidadeFederativa!;
    this.selectedTipoLavoura = filtros.tipoLavoura;
    this.selectedProducao = filtros.producao;

    this.loadDashboard();
  }

  onFilterBarInitialized(filters: any) {
    this.selectedAno = filters.ano;
    this.selectedTipoLavoura = filters.tipoLavoura;
    this.selectedRegiao = filters.regiao;
    this.selectedUnidadeFederativa = filters.uf;
    this.selectedProducao = filters.producao;

    // chama o loadDashboard com os valores do filter-bar
    this.loadDashboard();
  }
}
