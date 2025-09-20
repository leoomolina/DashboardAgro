import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TipoLavouraDTO } from '../models/tipo-lavoura.dto';
import { RegiaoBrasilDTO } from '../models/regiao-brasil.dto';
import { UnidadeFederativaDTO } from '../models/unidade-federativa.dto';
import { ResumoAnualDTO } from '../models/resumo-anual.dto';
import { ResumoEstadoDTO } from '../models/resumo-estado.dto';

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

  getResumoAnual(
    ano: number,
    idRegiao: number,
    idUf: number,
    tipoLavoura: number
  ): Observable<ResumoAnualDTO> {
    let params = new HttpParams()
      .set('ano', ano.toString());

    if (tipoLavoura != null && tipoLavoura > 0) {
      params = params.set('tipoLavoura', tipoLavoura.toString());
    }
    if (idUf != null && idUf > 0) {
      params = params.set('idUf', idUf.toString());
    }
    if (idRegiao != null && idRegiao > 0) {
      params = params.set('idRegiao', idRegiao.toString());
    }

    return this.http.get<ResumoAnualDTO>(`${this.apiBase}/api/dashboard/resumo-anual`, { params });
  }

  getResumoPorEstado(
    ano: number,
    idRegiao: number,
    tipoLavoura: number
  ): Observable<ResumoEstadoDTO[]> {
    let params = new HttpParams()
      .set('ano', ano.toString());

    if (tipoLavoura != null && tipoLavoura > 0) {
      params = params.set('tipoLavoura', tipoLavoura.toString());
    }
    if (idRegiao != null && idRegiao > 0) {
      params = params.set('idRegiao', idRegiao.toString());
    }

    return this.http.get<ResumoEstadoDTO[]>(`${this.apiBase}/api/dashboard/resumo-anual-por-estado`, { params });
  }
}
