import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Stats } from '../../model/stats';
import { DatePipe, DecimalPipe } from '@angular/common';
import { Landing } from '../../model/landing';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [DatePipe, DecimalPipe],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss',
})
export class LandingComponent {
  constructor(private http: HttpClient) {}
  private soft: number = -9999;
  private hard: number = 0;
  public softest: Stats = { landing: {} as Landing } as Stats;
  public hardest: Stats = { landing: {} as Landing } as Stats;
  public average: number = 0;
  public landings: number = 0;

  ngAfterViewInit(): void {
    this.http
      .get<Stats[]>('https://localhost:7142/api/flights/stats')
      .subscribe((data: Stats[]) => {
        for (let element of data) {
          if (element.landing) {
            if (this.soft < element.landing.verticalSpeed) {
              this.soft = element.landing.verticalSpeed;
              this.softest = element;
            }
            if (this.hard > element.landing.verticalSpeed) {
              this.hard = element.landing.verticalSpeed;
              this.hardest = element;
            }
            this.average += element.landing.verticalSpeed;
            this.landings++;
          }
        }

        this.average /= this.landings;
      });
  }
}
