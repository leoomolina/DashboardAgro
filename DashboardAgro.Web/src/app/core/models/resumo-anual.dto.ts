import { LavouraDTO } from "./lavoura-dto";

export interface ResumoAnualDTO {
  ano: number,
  areaColhidaTotal: number,
  areaPlantadaTotal: number,
  quantidadeProduzidaTotal: number,
  valorProducaoTotal: number,
  produtividade: number,
  lavouras: LavouraDTO[]
}