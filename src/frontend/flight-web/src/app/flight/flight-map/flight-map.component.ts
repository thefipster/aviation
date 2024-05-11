import { Component, AfterViewInit, Input } from '@angular/core';
import * as L from 'leaflet';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-flight-map',
  standalone: true,
  imports: [],
  templateUrl: './flight-map.component.html',
  styleUrl: './flight-map.component.scss'
})
export class FlightMapComponent implements AfterViewInit {
  @Input() departure: string = "";
  @Input() arrival: string = "";

  
  private map:any;

  constructor(private http:HttpClient) { }
  
  ngAfterViewInit(): void {
    this.http.get<any>(`https://localhost:7142/api/flights/${this.departure}/${this.arrival}/waypoints`).subscribe((data: any) => {
      console.log("got waypoints");
      let waypoints: Array<L.LatLng> = new Array();
      let lat = 0;
      let lon = 0;
      for (const element of data) {
        lat += element.latitude;
        lon += element.longitude;
        const waypoint = new L.LatLng(element.latitude, element.longitude);
        waypoints.push(waypoint);
      }
      lat = lat / data.length;
      lon = lon / data.length;
      this.initMap(lon,lat, 7);
      new L.Polyline(waypoints).addTo(this.map);
    })

    this.http.get<any>(`https://localhost:7142/api/flights/${this.departure}/${this.arrival}/track`).subscribe((data: any) => {
      console.log("got track");
      let waypoints: Array<L.LatLng> = new Array();
      for (const element of data) {
        const waypoint = new L.LatLng(element.latitude, element.longitude);
        waypoints.push(waypoint);
      }
      new L.Polyline(waypoints, { color: "green"}).addTo(this.map);
    })
  }

  private initMap(lat:number, lon:number, zoom:number): void {
    this.map = L.map('map', {
      center: [ lon, lat ],
      zoom: zoom
    });

    const tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 18,
      minZoom: 3,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    });

    tiles.addTo(this.map);
  }
}
