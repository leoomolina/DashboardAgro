import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'titulo-card',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './titulo-card.component.html'
})
export class TituloCard {
  @Input() titulo!: string;
  @Input() subTitulo!: string;
}
