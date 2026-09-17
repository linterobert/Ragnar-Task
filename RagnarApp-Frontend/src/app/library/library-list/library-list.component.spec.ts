import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { LibraryListComponent } from './library-list.component';
import { LibraryService } from '../../services/bookService';

describe('LibraryListComponent', () => {
  let component: LibraryListComponent;
  let fixture: ComponentFixture<LibraryListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibraryListComponent],
      providers: [{
        provide: LibraryService,
        useValue: { getBooks: () => of([]) }
      }]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LibraryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
