import { Component, Input } from '@angular/core';
import { FlightMapComponent } from './flight-map/flight-map.component';
import { RouterLink } from '@angular/router';
import { ImagesComponent } from './images/images.component';
import { ChartsComponent } from './charts/charts.component';

@Component({
  selector: 'app-flight',
  standalone: true,
  imports: [FlightMapComponent, RouterLink, ImagesComponent, ChartsComponent],
  templateUrl: './flight.component.html',
  styleUrl: './flight.component.scss'
})
export class FlightComponent {
  @Input() departure: string = "";
  @Input() arrival: string = "";
}
