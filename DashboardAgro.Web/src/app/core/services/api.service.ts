import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { TipoLavouraDTO } from '../models/tipo-lavoura.dto';
import { RegiaoBrasilDTO } from '../models/regiao-brasil.dto';
import { UnidadeFederativaDTO } from '../models/unidade-federativa.dto';
import { ResumoAnualDTO } from '../models/resumo-anual.dto';
import { ResumoEstadoDTO } from '../models/resumo-estado.dto';
import { ProducaoDTO } from '../models/producao-dto';
import { ResumoPorRegiaoDTO } from '../models/resumo-por-regiao.dto';
import { RankingItemDTO } from '../models/ranking-dto';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private apiBase = 'http://localhost:5000';

  constructor(private http: HttpClient) { }

  getAnos(): Observable<number[]> {
    return this.http.get<number[]>(`${this.apiBase}/api/controle-interno/anos-disponiveis`);
  }

  getTiposLavoura(): Observable<TipoLavouraDTO[]> {
    return this.http.get<TipoLavouraDTO[]>(`${this.apiBase}/api/parametros/tipo-lavoura`);
  }

  getRegioes(): Observable<RegiaoBrasilDTO[]> {
    return this.http.get<RegiaoBrasilDTO[]>(`${this.apiBase}/api/parametros/regioes-brasil`);
  }

  getUfs(): Observable<UnidadeFederativaDTO[]> {
    return this.http.get<UnidadeFederativaDTO[]>(`${this.apiBase}/api/parametros/unidades-federativas`);
  }

  getProducoes(): Observable<ProducaoDTO[]> {
    return this.http.get<ProducaoDTO[]>(`${this.apiBase}/api/parametros/listar-producoes`);
  }

  getResumoAnual(
    ano: number,
    idRegiao: number,
    idUf: number,
    tipoLavoura: number,
    idProducao: number
  ): Observable<ResumoAnualDTO> {
    let params = new HttpParams()
      .set('ano', ano.toString());

    if (tipoLavoura != null && tipoLavoura >= 0) {
      params = params.set('tipoLavoura', tipoLavoura.toString());
    }
    if (idUf != null && idUf > 0) {
      params = params.set('idUf', idUf.toString());
    }
    if (idRegiao != null && idRegiao > 0) {
      params = params.set('idRegiao', idRegiao.toString());
    }
    if (idProducao != null && idProducao > 0) {
      params = params.set('idProducao', idProducao.toString());
    }

    return this.http.get<ResumoAnualDTO>(`${this.apiBase}/api/dashboard/resumo-anual`, { params });
  }

  getResumoPorEstado(
    ano: number,
    idRegiao: number,
    tipoLavoura: number,
    idProducao: number
  ): Observable<ResumoEstadoDTO[]> {
    let params = new HttpParams()
      .set('ano', ano.toString());

    if (tipoLavoura != null && tipoLavoura >= 0) {
      params = params.set('tipoLavoura', tipoLavoura.toString());
    }
    if (idRegiao != null && idRegiao > 0) {
      params = params.set('idRegiao', idRegiao.toString());
    }
    if (idProducao != null && idProducao > 0) {
      params = params.set('idProducao', idProducao.toString());
    }

    return this.http.get<ResumoEstadoDTO[]>(`${this.apiBase}/api/dashboard/resumo-anual-por-estado`, { params });
  }

  getResumoPorRegiao(
    ano: number,
    idRegiao: number,
    idUf: number,
    tipoLavoura: number,
    idProducao: number
  ): Observable<ResumoPorRegiaoDTO[]> {
    if (ano == null) {
      return of([]);
    }

    let params = new HttpParams()
      .set('ano', ano.toString());

    if (tipoLavoura != null && tipoLavoura >= 0) {
      params = params.set('tipoLavoura', tipoLavoura.toString());
    }
    if (idUf != null && idUf > 0) {
      params = params.set('idUf', idUf.toString());
    }
    if (idRegiao != null && idRegiao > 0) {
      params = params.set('idRegiao', idRegiao.toString());
    }
    if (idProducao != null && idProducao > 0) {
      params = params.set('idProducao', idProducao.toString());
    }

    return this.http.get<ResumoPorRegiaoDTO[]>(`${this.apiBase}/api/dashboard/analise-por-regiao`, { params });
  }

    getRanking(
    ano: number,
    tipoRanking: number,
    idRegiao: number,
    idUf: number,
    tipoLavoura: number,
    idProducao: number
  ): Observable<RankingItemDTO[]> {
    if (ano == null || tipoRanking == null) {
      return of([]);
    }

    let params = new HttpParams()
      .set('ano', ano.toString())
      .set('tipoRanking', tipoRanking.toString());

    if (tipoLavoura != null && tipoLavoura >= 0) {
      params = params.set('tipoLavoura', tipoLavoura.toString());
    }
    if (idUf != null && idUf > 0) {
      params = params.set('idUf', idUf.toString());
    }
    if (idRegiao != null && idRegiao > 0) {
      params = params.set('idRegiao', idRegiao.toString());
    }
    if (idProducao != null && idProducao > 0) {
      params = params.set('idProducao', idProducao.toString());
    }

    return this.http.get<RankingItemDTO[]>(`${this.apiBase}/api/dashboard/ranking-quantidade-produzida`, { params });
  }
}
