import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Book } from '../interfaces/book.interface';

@Injectable({
  providedIn: 'root'
})
export class LibraryService {

  private apiUrl = '/api/library';

  constructor(private http: HttpClient) {}

  getBooks() {
    return this.http.get<Book[]>(this.apiUrl);
  }

  importBooks(file: File) {
    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.http.post<{ queuedBooks: number }>(`${this.apiUrl}/imports`, formData);
  }
}