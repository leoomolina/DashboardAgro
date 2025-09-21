import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ApiService } from '../../../../core/services/api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TipoLavouraDTO } from '../../../../core/models/tipo-lavoura.dto';
import { RegiaoBrasilDTO } from '../../../../core/models/regiao-brasil.dto';
import { UnidadeFederativaDTO } from '../../../../core/models/unidade-federativa.dto';
import { ProducaoDTO } from '../../../../core/models/producao-dto';

@Component({
  selector: 'filter-bar',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './filter-bar.component.html',
  styleUrls: ['./filter-bar.component.css']
})
export class DashboardFiltersComponent implements OnInit {
  @Output() filtersApplied = new EventEmitter<
    {
      ano: number;
      regiao: RegiaoBrasilDTO;
      unidadeFederativa?: UnidadeFederativaDTO;
      tipoLavoura: TipoLavouraDTO;
      producao: ProducaoDTO
    }>();
  @Output() initialized = new EventEmitter<any>();

  anos: number[] = [];
  unidadesFederativas: UnidadeFederativaDTO[] = [];
  tiposLavouras: TipoLavouraDTO[] = [];
  regioes: RegiaoBrasilDTO[] = [];
  producoes: ProducaoDTO[] = [];

  selectedAno!: number;
  selectedUnidadeFederativa!: UnidadeFederativaDTO;
  selectedTipoLavoura!: TipoLavouraDTO;
  selectedRegiao!: RegiaoBrasilDTO;
  selectedProducao!: ProducaoDTO;

  loading: boolean = false;

  constructor(private apiService: ApiService) { }
  ngOnInit(): void {
    this.loadFilters();
  }

  loadFilters(): void {
    this.loading = true;

    this.apiService.getAnos().subscribe(data => {
      this.anos = data;
      this.selectedAno = this.anos[this.anos.length - 1];
    });

    this.apiService.getRegioes().subscribe(data => {
      if (data == null)
        return;

      this.regioes = [{ id: -1, descricao: 'Todas' }, ...data];
      this.selectedRegiao = this.regioes[0];

      this.checkInitialized();
    });

    this.apiService.getTiposLavoura().subscribe(data => {
      if (data == null)
        return;

      this.tiposLavouras = [{ id: -1, descricao: 'Todas' }, ...data];
      this.selectedTipoLavoura = this.tiposLavouras[0];

      this.checkInitialized();
    });

    this.apiService.getUfs().subscribe(data => {
      if (data == null)
        return;

      this.unidadesFederativas = [{ id: -1, nomeUF: 'Todos', siglaUF: '' }, ...data];
      this.selectedUnidadeFederativa = this.unidadesFederativas[0];

      this.checkInitialized();
    });

    this.apiService.getProducoes().subscribe(data => {
      if (data == null)
        return;

      this.producoes = [{ id: -1, descricao: 'Todos' }, ...data];
      this.selectedProducao = this.producoes[0];

      this.checkInitialized();
    });

    this.loading = false;
  }

  private checkInitialized() {
    // só emite quando todos os filtros estiverem definidos
    if (this.selectedAno && this.selectedTipoLavoura && this.selectedRegiao && this.selectedUnidadeFederativa) {
      this.initialized.emit({
        ano: this.selectedAno,
        tipoLavoura: this.selectedTipoLavoura,
        regiao: this.selectedRegiao,
        uf: this.selectedUnidadeFederativa,
        producao: this.selectedProducao
      });
    }
  }

  applyFilters(): void {
    this.filtersApplied.emit({
      ano: this.selectedAno,
      regiao: this.selectedRegiao,
      unidadeFederativa: this.selectedUnidadeFederativa,
      tipoLavoura: this.selectedTipoLavoura,
      producao: this.selectedProducao
    });
  }
}
