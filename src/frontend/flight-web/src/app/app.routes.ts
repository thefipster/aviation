import { Routes } from '@angular/router';
import { OverviewComponent } from './overview/overview.component';
import { FlightComponent } from './flight/flight.component';
import { ErrorComponent } from './error/error.component';

export const routes: Routes = [
    { path: 'overview', component: OverviewComponent },
    { path: 'flight/:departure/:arrival', component: FlightComponent },
    { path: '',   redirectTo: '/overview', pathMatch: 'full' },
    { path: '**', component: ErrorComponent }
];