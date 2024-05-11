import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Stats } from '../../model/stats';
import { Aircraft } from '../../model/aircraft';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-aircraft',
  standalone: true,
  imports: [DecimalPipe],
  templateUrl: './aircraft.component.html',
  styleUrl: './aircraft.component.scss'
})
export class AircraftComponent {
  public aircraft: Aircraft = {} as Aircraft;
  public totalFuel: number = 0;
  public totalDistance: number = 0;
  public totalAirtime: number = 0;
  public totalFlights: number = 0;
  public airHours: number = 0;
  public airMinutes: number = 0;

  constructor(private http:HttpClient) { }

  ngAfterViewInit(): void {
    this.http.get<Aircraft>("https://localhost:7142/api/aircraft").subscribe((data:Aircraft) => {
      this.aircraft = data;
    });

    this.http.get<Stats[]>("https://localhost:7142/api/flights/stats").subscribe((data:Stats[]) => {
      data.forEach((element: Stats) => {
        this.totalFuel += element.fuelUsed;

        this.totalDistance += element.routeDistance * 1.852;


        this.totalAirtime += element.flightTime;
        this.airHours = Math.floor(this.totalAirtime / 3600);
        this.airMinutes = Math.floor((this.totalAirtime % 3600) / 60);

        this.totalFlights += 1;
      });
    });
  }

}
