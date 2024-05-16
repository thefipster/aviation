import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Component } from '@angular/core';
import { Leg } from '../model/leg';
import { MapLeg } from '../model/map-leg';
import { Airport } from '../model/airport';

@Component({
  selector: 'app-flight-plan',
  standalone: true,
  imports: [NgFor, NgIf, RouterLink],
  templateUrl: './flight-plan.component.html',
  styleUrl: './flight-plan.component.scss'
})
export class FlightPlanComponent {
  public plan: Leg[] = new Array<Leg>();
  public legs: MapLeg[] = new Array<MapLeg>();
  public index: number = -1;
  public icao: string = '';

  constructor(private http: HttpClient) { }

  ngAfterViewInit(): void {
    this.http.get<Leg[]>("https://localhost:7142/api/flightplan").subscribe((data: Leg[]) => {
      this.plan = data;
    });
    this.http.get<MapLeg[]>("https://localhost:7142/api/legs").subscribe((data: MapLeg[]) => {
      this.legs = data;
    });
  }

  activate(leg: number): void {
    this.index = leg;
  }

  deactivate(): void {
    this.index = -1;
  }

  input(event: Event): void {
    this.icao = (event.target as HTMLInputElement).value;
  }

  insertFlight(): void {
    this.http.get<Airport>("https://localhost:7142/api/airport/" + this.icao).subscribe((airport: Airport) => {
      if (airport != null) {
        this.addFlight(airport)
      }
    })
  }

  addFlight(airport: Airport): void {

    let newLegs = new Array<Leg>();
    for (const leg of this.plan) {
      if (leg.no < this.index) {
        newLegs.push(leg);
      }
      if (leg.no === this.index) {
        let newLeg = { no: this.index + 1, from: airport.ident, to: leg.to } as Leg;
        leg.to = airport.ident;

        newLegs.push(leg);
        newLegs.push(newLeg);
      }
      if (leg.no > this.index) {
        leg.no = leg.no + 1;
        newLegs.push(leg);
      }
    }

    const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

    this.http.post("https://localhost:7142/api/flightplan", JSON.stringify(newLegs), {
      headers: headers
    }).subscribe((result: any) => {
      console.log(result);
    });
  }
}
