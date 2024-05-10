import { Component } from '@angular/core';
import { MapComponent } from '../map/map.component';
import { LegsComponent } from '../legs/legs.component';

@Component({
  selector: 'app-overview',
  standalone: true,
  imports: [MapComponent, LegsComponent],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.scss'
})
export class OverviewComponent {

}
