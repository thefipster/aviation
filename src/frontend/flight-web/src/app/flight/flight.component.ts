import { Component, Input } from '@angular/core';
import { FlightMapComponent } from '../flight-map/flight-map.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-flight',
  standalone: true,
  imports: [FlightMapComponent, RouterLink],
  templateUrl: './flight.component.html',
  styleUrl: './flight.component.scss'
})
export class FlightComponent {
  @Input() departure: string = "";
  @Input() arrival: string = "";
}
