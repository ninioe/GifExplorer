import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm } from '@angular/forms';

import { GifService } from 'src/app/_services/gif.service';
import { SearchResultsComponent } from '../search-results/search-results.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  isShowSearchResults = false;
  _searchResoults: SearchResultsComponent;

  @ViewChild('searchResoults', { static: false })  
  set searchResoults(value: SearchResultsComponent) {
    if (value) {
      this._searchResoults = value;
    }
  }

  get searchResoults() {
    return this._searchResoults;
  }

  constructor(private gifService: GifService) { }

  ngOnInit() {

  }

  onSubmit(form: NgForm) {
    this.isShowSearchResults = true;
    const term = form.value.term;
    this.gifService.search(term).subscribe(res => {
      this.searchResoults.term = term;
      this.searchResoults.searchResoults = res;
    }, err => {
      console.log(err);
    });


  }

}
