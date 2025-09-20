import { RegiaoBrasilDTO } from "./regiao-brasil.dto"

export interface UnidadeFederativaDTO {
    id: number,
    siglaUF: string,
    nomeUF: string,
    regiao?: RegiaoBrasilDTO
}