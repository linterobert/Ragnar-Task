import { Component, OnDestroy, inject } from '@angular/core';
import { Router } from '@angular/router';
import { LibraryService } from '../../services/bookService';

@Component({
  selector: 'app-library-import',
  imports: [],
  templateUrl: './library-import.component.html',
  styleUrl: './library-import.component.css'
})
export class LibraryImportComponent implements OnDestroy {
  private readonly libraryService = inject(LibraryService);
  private readonly router = inject(Router);
  private redirectTimer: ReturnType<typeof setTimeout> | undefined;

  isDragging = false;
  selectedFile: File | null = null;
  invalidFile = false;
  isUploading = false;
  uploadMessage = '';
  uploadError = '';

  private readonly acceptedFileExtensions = ['csv'];

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    this.isDragging = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    this.isDragging = false;
    this.setSelectedFile(event.dataTransfer?.files[0] ?? null);
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.setSelectedFile(input.files?.[0] ?? null);
  }

  private setSelectedFile(file: File | null): void {
    this.invalidFile = file !== null && !this.isAcceptedFile(file);
    this.selectedFile = this.invalidFile ? null : file;

    this.uploadMessage = '';
    this.uploadError = '';

    if (this.selectedFile) {
      this.uploadFile(this.selectedFile);
    }
  }

  private uploadFile(file: File): void {
    this.isUploading = true;

    this.libraryService.importBooks(file).subscribe({
      next: (response) => {
        this.isUploading = false;
        this.uploadMessage = `${response.queuedBooks} books queued for import.`;
        this.redirectTimer = setTimeout(() => {
          void this.router.navigate(['/library']);
        }, 5000);
      },
      error: (error) => {
        this.isUploading = false;
        this.uploadError = error.error || 'The file could not be uploaded.';
      }
    });
  }

  ngOnDestroy(): void {
    if (this.redirectTimer) {
      clearTimeout(this.redirectTimer);
    }
  }

  private isAcceptedFile(file: File): boolean {
    const extension = file.name.split('.').pop()?.toLowerCase();
    return extension !== undefined && this.acceptedFileExtensions.includes(extension);
  }

}
