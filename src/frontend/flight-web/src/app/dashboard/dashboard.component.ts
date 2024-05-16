import { Component } from '@angular/core';
import { AircraftComponent } from './aircraft/aircraft.component';
import { AircraftPositionComponent } from './aircraft-position/aircraft-position.component';
import { LastFlightComponent } from './last-flight/last-flight.component';
import { NextFlightComponent } from './next-flight/next-flight.component';
import { LandingComponent } from './landing/landing.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [AircraftComponent, AircraftPositionComponent, LastFlightComponent, NextFlightComponent, LandingComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {

}
