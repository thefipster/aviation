import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { PlannedFlight } from '../../model/planned-flight';
import { Aircraft } from '../../model/aircraft';
import { Airport } from '../../model/airport';

@Component({
  selector: 'app-next-flight',
  standalone: true,
  imports: [],
  templateUrl: './next-flight.component.html',
  styleUrl: './next-flight.component.scss'
})
export class NextFlightComponent {
  public planned: PlannedFlight = { departure: {} as Airport, arrival: {} as Airport } as PlannedFlight;

  constructor(private http: HttpClient) { }

  ngAfterViewInit(): void {
    this.http.get<PlannedFlight>("https://localhost:7142/api/flights/next").subscribe((data: PlannedFlight) => {
      this.planned = data;
    });
  }
}
