import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './modules/dashboard/dashboard.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, DashboardComponent], // importa o DashboardComponent
  templateUrl: './app.html',  // template com <app-dashboard></app-dashboard>
})
export class AppComponent { }
