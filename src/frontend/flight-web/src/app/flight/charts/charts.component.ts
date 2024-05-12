import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-charts',
  standalone: true,
  imports: [NgFor],
  templateUrl: './charts.component.html',
  styleUrl: './charts.component.scss'
})
export class ChartsComponent {
  @Input() departure: string = '';
  @Input() arrival: string = '';

  public charts: string[] = [];

  constructor(private http: HttpClient) {}

  ngAfterViewInit(): void {
    this.http
      .get<string[]>(
        'https://localhost:7142/api/flights/' +
          this.departure +
          '/' +
          this.arrival +
          '/charts'
      )
      .subscribe((data: string[]) => {
        this.charts = data;
      });
  }
}
