import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { DashboardPageComponent } from './app/modules/dashboard/dashboard-page/pages/dashboard-page.component';
import { HttpClientModule } from '@angular/common/http';
import { importProvidersFrom } from '@angular/core';
import {
  Chart,
  ArcElement,
  DoughnutController,
  Tooltip,
  Legend,
  CategoryScale,
  LinearScale,
  BarController,
  BarElement
} from 'chart.js';

Chart.register(
  DoughnutController,
  ArcElement,
  Tooltip,
  Legend,
  CategoryScale,
  LinearScale,
  BarController,
  BarElement
);

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(HttpClientModule),
    provideRouter([
      { path: '', component: DashboardPageComponent }
    ]),
  ]
}).catch(err => console.error(err));
