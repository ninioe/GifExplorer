import { Component, OnInit } from '@angular/core';

import { Gif } from 'src/app/_models/gif';
import { GifService } from 'src/app/_services/gif.service';

@Component({
  selector: 'app-trending',
  templateUrl: './trending.component.html',
  styleUrls: ['./trending.component.scss']
})
export class TrendingComponent implements OnInit {
  
  trendingGifs: Gif[] = [];

  constructor(private gifService: GifService) { }

  ngOnInit() {
    this.gifService.getTrending().subscribe(res => {
      this.trendingGifs = res;
    }, err => {
      console.log(err);
    });
  }

}
