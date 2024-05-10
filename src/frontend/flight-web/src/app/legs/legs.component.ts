import { AfterViewInit, Component } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MapLeg } from '../map-leg';

@Component({
  selector: 'app-legs',
  standalone: true,
  imports: [NgFor, NgIf, RouterLink],
  templateUrl: './legs.component.html',
  styleUrl: './legs.component.scss'
})
export class LegsComponent implements AfterViewInit {
  public legs: MapLeg[] = [];

  constructor(private http:HttpClient) { }

  ngAfterViewInit(): void {
    this.http.get<MapLeg[]>("https://localhost:7142/api/legs").subscribe((data:MapLeg[]) => {
      for (const leg of data) {
        this.legs.push(leg);
      }
      this.legs = data;
    });
  }
}
