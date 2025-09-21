import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule, LoaderComponent],
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent {
  @Input() title!: string;
  @Input() content!: string;
  @Input() isLoading: boolean = false;
}
