import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loader',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './loader.component.html'
})
export class LoaderComponent {
  @Input() isLoading: boolean = false;
}
