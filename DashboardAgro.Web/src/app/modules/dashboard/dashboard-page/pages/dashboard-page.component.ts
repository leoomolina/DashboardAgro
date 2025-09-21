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
import { forkJoin } from 'rxjs';
import { AnalisePorRegiaoComponent } from '../../components/analise-por-regiao/analise-por-regiao.component';

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
    RankingComponent,
    AnalisePorRegiaoComponent,
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
          tipoLavoura: 0,
          areaColhida: 0,
          quantidadeProduzida: 0,
          valorProducao: 0,
          id: 0,
          areaPlantadaxDestinadaColheita: 0,
          produtividadePercentual: 0
        },
        {
          descricao: 'Lavoura Permanente',
          tipoLavoura: 1,
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
  producaoTemporaria: number[] = [];
  producaoPermanente: number[] = [];

  rankingRegioes: RankingItemDTO[] = [];
  rankingUfs: RankingItemDTO[] = [];
  rankingProducoesPermanentes: RankingItemDTO[] = [];
  rankingProducoesTemporarias: RankingItemDTO[] = [];

  loading: boolean = false;

  constructor(private apiService: ApiService) { }
  ngOnInit(): void {
  }

  loadDashboard(): void {
    this.loading = true;

    const resumoAnual$ = this.apiService.getResumoAnual(
      this.selectedAno,
      this.selectedRegiao.id,
      this.selectedUnidadeFederativa?.id ?? 0,
      this.selectedTipoLavoura.id,
      this.selectedProducao.id
    );

    const resumoPorEstado$ = this.apiService.getResumoPorEstado(
      this.selectedAno,
      this.selectedRegiao.id,
      this.selectedTipoLavoura.id,
      this.selectedProducao.id
    );

    this.apiService.getRanking(
      this.selectedAno,
      0, // Unidades Federativas = 0
      this.selectedRegiao.id,
      this.selectedUnidadeFederativa.id,
      this.selectedTipoLavoura.id,
      this.selectedProducao.id
    ).subscribe(data => {
      this.rankingUfs = data;
    });

    this.apiService.getRanking(
      this.selectedAno,
      1, // Região = 1
      this.selectedRegiao.id,
      this.selectedUnidadeFederativa.id,
      this.selectedTipoLavoura.id,
      this.selectedProducao.id
    ).subscribe(data => {
      this.rankingRegioes = data;
    });

    this.apiService.getRanking(
      this.selectedAno,
      2, // Produção Permanente = 2
      this.selectedRegiao.id,
      this.selectedUnidadeFederativa.id,
      this.selectedTipoLavoura.id,
      this.selectedProducao.id
    ).subscribe(data => {
      this.rankingProducoesPermanentes = data;
    });

    this.apiService.getRanking(
      this.selectedAno,
      3, // Produção Temporária = 3
      this.selectedRegiao.id,
      this.selectedUnidadeFederativa.id,
      this.selectedTipoLavoura.id,
      this.selectedProducao.id
    ).subscribe(data => {
      this.rankingProducoesTemporarias = data;
    });

    forkJoin([resumoAnual$, resumoPorEstado$]).subscribe(
      ([resumoAnualData, resumoPorEstadoData]) => {
        // Atualiza cards
        if (resumoAnualData) {
          this.resumoAnual = resumoAnualData;
          this.valorTotalArea = FormatUtils.formatarArea(this.resumoAnual.areaColhidaTotal);
          this.valorTotalDinheiro = FormatUtils.formatarDinheiro(this.resumoAnual.valorProducaoTotal);
          this.valorTotalPeso = FormatUtils.formatarPeso(this.resumoAnual.quantidadeProduzidaTotal);
        }

        // Atualiza gráfico/mapa
        this.resumoPorEstado = resumoPorEstadoData;

        this.loading = false;
      },
      (error) => {
        console.error(error);
        this.loading = false;
      }
    );

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

    this.loadDashboard();
  }
}
