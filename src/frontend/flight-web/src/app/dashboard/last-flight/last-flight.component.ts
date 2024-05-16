import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Stats } from '../../model/stats';
import { Airport } from '../../model/airport';
import { DatePipe, DecimalPipe } from '@angular/common';
import { Landing } from '../../model/landing';

@Component({
  selector: 'app-last-flight',
  standalone: true,
  imports: [DatePipe, DecimalPipe],
  templateUrl: './last-flight.component.html',
  styleUrl: './last-flight.component.scss'
})
export class LastFlightComponent {
  public stats: Stats = { landing: {} as Landing } as Stats;
  public departure: Airport = {} as Airport;
  public arrival: Airport = {} as Airport;

  constructor(private http: HttpClient) { }

  ngAfterViewInit(): void {
    this.http.get<Stats>("https://localhost:7142/api/flights/last").subscribe((data: Stats) => {
      this.stats = data;

      this.http.get<Airport>("https://localhost:7142/api/airport/" + data.departure).subscribe((data: Airport) => {
        this.departure = data
      });
      this.http.get<Airport>("https://localhost:7142/api/airport/" + data.arrival).subscribe((data: Airport) => {
        this.arrival = data
      });
    });
  }
}
