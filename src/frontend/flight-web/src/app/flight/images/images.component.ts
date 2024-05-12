import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-images',
  standalone: true,
  imports: [NgFor, RouterLink],
  templateUrl: './images.component.html',
  styleUrl: './images.component.scss',
})
export class ImagesComponent {
  @Input() departure: string = '';
  @Input() arrival: string = '';

  public images: string[] = [];

  constructor(private http: HttpClient) {}

  ngAfterViewInit(): void {
    this.http
      .get<string[]>(
        'https://localhost:7142/api/flights/' +
          this.departure +
          '/' +
          this.arrival +
          '/images'
      )
      .subscribe((data: string[]) => {
        this.images = data;
      });
  }
}
