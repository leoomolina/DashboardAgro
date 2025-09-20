import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardPageComponent } from './modules/dashboard/dashboard-page/pages/dashboard-page.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, DashboardPageComponent], 
  templateUrl: './app.html',  
})
export class AppComponent { }
