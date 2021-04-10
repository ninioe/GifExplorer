import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

import { Gif } from '../_models/gif';

@Injectable({
  providedIn: 'root'
})
export class GifService {

  private baseURL = environment.API;

  constructor(private http: HttpClient) { }

  getTrending(): Observable<Gif[]> {
    return this.http.get<Gif[]>(this.baseURL + "trending");
  }

  search(term: string): Observable<Gif[]> {
    return this.http.get<Gif[]>(this.baseURL + "search/?term=" + encodeURIComponent(term));
  }

}
