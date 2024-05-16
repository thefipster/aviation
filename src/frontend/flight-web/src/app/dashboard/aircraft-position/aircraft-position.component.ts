import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Point } from '../../model/point';
import * as L from 'leaflet';

@Component({
  selector: 'app-aircraft-position',
  standalone: true,
  imports: [],
  templateUrl: './aircraft-position.component.html',
  styleUrl: './aircraft-position.component.scss'
})
export class AircraftPositionComponent {
  private map:any;
  public position: Point = {} as Point;

  constructor(private http:HttpClient) { }

  ngAfterViewInit(): void {
    this.http.get<Point>("https://localhost:7142/api/aircraft/position").subscribe((data:Point) => {
        this.position = data;
        this.map = L.map('map', {
          center: [ data.latitude, data.longitude ],
          zoom: 12
        });

        new L.Marker(new L.LatLng(data.latitude, data.longitude)).addTo(this.map).bindPopup(data.name);
    
        const tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
          maxZoom: 18,
          minZoom: 3,
          attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        });
    
        tiles.addTo(this.map);
    });
  }

}
