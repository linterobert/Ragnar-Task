import { Component, OnInit, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EMPTY, timer } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { Book } from '../../interfaces/book.interface';
import { LibraryService } from '../../services/bookService';

@Component({
  selector: 'app-library-list',
  imports: [],
  templateUrl: './library-list.component.html',
  styleUrl: './library-list.component.css'
})
export class LibraryListComponent implements OnInit {
  private readonly libraryService = inject(LibraryService);
  private readonly destroyRef = inject(DestroyRef);

  books: Book[] = [];

  ngOnInit(): void {
    timer(0, 3000).pipe(
      switchMap(() => this.libraryService.getBooks().pipe(
        catchError((error) => {
          console.error('Could not load libraries', error);
          return EMPTY;
        })
      )),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe((books) => {
      this.books = books;
    });
  }

}
