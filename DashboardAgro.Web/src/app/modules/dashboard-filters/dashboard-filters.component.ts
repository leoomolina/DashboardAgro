import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-dashboard-filters',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
  ],
  templateUrl: './dashboard-filters.component.html',
  styleUrls: ['./dashboard-filters.component.css']
})
export class DashboardFiltersComponent {
  form: FormGroup;

  lavouras = [
    { value: 'permanente', label: 'Lavoura Permanente' },
    { value: 'temporaria', label: 'Lavoura Temporária' }
  ];

  producoes: Record<string, { value: string; label: string }[]> = {
    permanente: [
      { value: 'cafe', label: 'Café' },
      { value: 'soja', label: 'Soja' },
    ],
    temporaria: [
      { value: 'milho', label: 'Milho' },
      { value: 'laranja', label: 'Laranja' },
    ],
  };

  regioes = ['Sul', 'Sudeste', 'Centro-Oeste', 'Norte', 'Nordeste'];
  estados = ['SP', 'MG', 'AM', 'RS', 'PR'];

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      ano: [''],
      lavoura: [''],
      producao: [''],
      regiao: [''],
      estado: [''],
    });
  }

  get producoesFiltradas() {
    const lavoura = this.form.get('lavoura')?.value;
    return lavoura ? this.producoes[lavoura] : [];
  }

  atualizar() {
    console.log(this.form.value);
  }
}
