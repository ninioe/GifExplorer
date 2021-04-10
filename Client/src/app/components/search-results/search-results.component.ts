import { Component, Input, OnInit } from '@angular/core';

import { Gif } from 'src/app/_models/gif';
import { GifService } from 'src/app/_services/gif.service';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.scss']
})
export class SearchResultsComponent implements OnInit {
  
  @Input() term: string = "";
  @Input() searchResoults: Gif[] = [];

  constructor(private gifService: GifService) { }

  ngOnInit() {
    
  }

}
