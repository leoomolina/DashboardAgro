

import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { CardRegiaoComponent } from '../../../../shared/card-regiao/card-regiao.component';
import { TituloCard } from '../../../../shared/title-card/titulo-card.component';
import { ResumoPorRegiaoDTO } from '../../../../core/models/resumo-por-regiao.dto';
import { ApiService } from '../../../../core/services/api.service';
import { RegiaoBrasilDTO } from '../../../../core/models/regiao-brasil.dto';
import { UnidadeFederativaDTO } from '../../../../core/models/unidade-federativa.dto';
import { TipoLavouraDTO } from '../../../../core/models/tipo-lavoura.dto';
import { ProducaoDTO } from '../../../../core/models/producao-dto';

@Component({
    selector: 'app-analise-por-regiao',
    imports: [CommonModule, CardRegiaoComponent, TituloCard],
    standalone: true,
    templateUrl: './analise-por-regiao.component.html',
})
export class AnalisePorRegiaoComponent implements OnChanges {
    titulo: string = "Análise por Região Geográfica";
    subTitulo: string = "Análise por Região Geográfica";

    resumosPorRegiao: ResumoPorRegiaoDTO[] = [];

    @Input({ required: true }) selectedAno!: number;
    @Input() selectedRegiao!: RegiaoBrasilDTO;
    @Input() selectedUnidadeFederativa!: UnidadeFederativaDTO;
    @Input() selectedTipoLavoura!: TipoLavouraDTO;
    @Input() selectedProducao!: ProducaoDTO;

    carregarRegioes(): void {
        this.apiService.getResumoPorRegiao(
            this.selectedAno,
            this.selectedRegiao?.id,
            this.selectedUnidadeFederativa?.id,
            this.selectedTipoLavoura?.id,
            this.selectedProducao?.id
        ).subscribe(data => {
            this.resumosPorRegiao = data;
            console.log(this.resumosPorRegiao);
        });
    }

    constructor(private apiService: ApiService) { }
    ngOnChanges(): void {
        this.carregarRegioes();
    }
}
