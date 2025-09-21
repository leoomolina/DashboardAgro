import { LavouraDTO } from "./lavoura-dto";

export interface ResumoEstadoDTO {
  ano: number,
  siglaUf: string,
  quantidadeProduzidaTotal: number,
  lavouras: LavouraDTO[]
}