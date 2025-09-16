// import { Component, Input } from '@angular/core';
// import * as L from 'leaflet';

// @Component({
//   selector: 'app-map',
//   template: `<div id="map" style="height: 400px;"></div>`
// })
// export class MapComponent {
//   @Input() geoData: any; // GeoJSON
  
//   ngOnInit() {
//     const map = L.map('map').setView([-15, -55], 4); // Centro do Brasil
//     L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(map);
    
//     if(this.geoData) {
//       L.geoJSON(this.geoData).addTo(map);
//     }
//   }
// }
