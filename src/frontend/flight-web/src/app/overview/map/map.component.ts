import { Component, AfterViewInit } from '@angular/core';
import * as L from 'leaflet';
import { HttpClient } from "@angular/common/http";
import { MapLeg } from '../../model/map-leg';

@Component({
  selector: 'app-map',
  standalone: true,
  imports: [],
  templateUrl: './map.component.html',
  styleUrl: './map.component.scss'
})
export class MapComponent implements AfterViewInit {
  private map: any;

  constructor(private http: HttpClient) { }

  ngAfterViewInit(): void {
    this.initMap(7, 40);
    this.http.get<any>("https://localhost:7142/api/legs").subscribe((data: MapLeg[]) => {
      for (const leg of data) {
        new L.Marker(new L.LatLng(leg.departure.latitude, leg.departure.longitude)).addTo(this.map).bindTooltip(leg.departure.name);

        if (leg.no === 42)
          continue;

        if (leg.isFlown) {
          let waypoints: Array<L.LatLng> = new Array();
          waypoints.push(new L.LatLng(leg.departure.latitude, leg.departure.longitude));
          waypoints.push(new L.LatLng(leg.arrival.latitude, leg.arrival.longitude));
          new L.Polyline(waypoints, { color: "green" }).addTo(this.map).bindPopup(`${leg.departure.icao} - ${leg.arrival.icao}`);
        }
      }
    });
  }

  private initMap(lat: number, lon: number): void {
    this.map = L.map('map', {
      center: [lon, lat],
      zoom: 3
    });

    const tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 18,
      minZoom: 3,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    });

    tiles.addTo(this.map);
  }
}
