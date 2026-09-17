import { Routes } from '@angular/router';
import { LibraryListComponent } from './library/library-list/library-list.component';
import { LibraryImportComponent } from './library/library-import/library-import.component';

export const routes: Routes = [
    {path: 'library', component: LibraryListComponent},
    {path: 'import', component: LibraryImportComponent}
];
