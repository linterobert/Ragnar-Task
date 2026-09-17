import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';

import { LibraryImportComponent } from './library-import.component';
import { LibraryService } from '../../services/bookService';

describe('LibraryImportComponent', () => {
  let component: LibraryImportComponent;
  let fixture: ComponentFixture<LibraryImportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibraryImportComponent],
      providers: [{
        provide: LibraryService,
        useValue: { importBooks: () => of({ queuedBooks: 1 }) }
      }, provideRouter([])]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LibraryImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
