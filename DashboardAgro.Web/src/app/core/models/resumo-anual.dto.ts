import { LavouraDTO } from "./lavoura-dto";

export interface ResumoAnualDTO {
  ano: number,
  areaColhidaTotal: number,
  quantidadeProduzidaTotal: number,
  valorProducaoTotal: number,
  lavouras: LavouraDTO[]
}