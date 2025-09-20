import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { DashboardPageComponent } from './app/modules/dashboard/dashboard-page/pages/dashboard-page.component';
import { HttpClientModule } from '@angular/common/http';
import { importProvidersFrom } from '@angular/core';
import { provideCharts, withDefaultRegisterables } from 'ng2-charts';

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(HttpClientModule),
    provideCharts(withDefaultRegisterables()),
    provideRouter([
      { path: '', component: DashboardPageComponent } 
    ]), provideCharts(withDefaultRegisterables())
  ]
}).catch(err => console.error(err));
