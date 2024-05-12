import { Routes } from '@angular/router';
import { OverviewComponent } from './overview/overview.component';
import { FlightComponent } from './flight/flight.component';
import { ErrorComponent } from './error/error.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FlightPlanComponent } from './flight-plan/flight-plan.component';

export const routes: Routes = [
    { path: 'dashboard', component: DashboardComponent },
    { path: 'overview', component: OverviewComponent },
    { path: 'flightplan', component: FlightPlanComponent },
    { path: 'flight/:departure/:arrival', component: FlightComponent },
    { path: '',   redirectTo: '/dashboard', pathMatch: 'full' },
    { path: '**', component: ErrorComponent }
];