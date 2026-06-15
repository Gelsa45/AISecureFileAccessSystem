import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileAccessService } from '../services/file-access.service';
import { ApiService } from '../services/api';

@Component({
  selector: 'app-user-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-dashboard.html',
  styleUrls: ['./user-dashboard.css']
})
export class UserDashboardComponent implements OnInit {

  currentUserId = 0;
  userName = '';

  files: any[] = [];

  constructor(
    private fileAccessService: FileAccessService,
    private api: ApiService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

    console.log('User dashboard loaded');

    this.currentUserId =
      Number(localStorage.getItem('userId'));

    this.userName =
      localStorage.getItem('userName') || '';

    this.api.getFiles().subscribe({
      next: (data: any) => {

        console.log('FILES RECEIVED:', data);

        this.files = [...data];

        console.log('AFTER ASSIGN:', this.files);

        this.cdr.detectChanges();

        setTimeout(() => {
          console.log('FILES LENGTH AFTER 1 SECOND:', this.files.length);
        }, 1000);
      },
      error: (err) => {
        console.error('Error loading files', err);
      }
    });
  }

  accessFile(fileId: number): void {

    const userId = this.currentUserId;

    this.fileAccessService
      .logAccess(userId, fileId)
      .subscribe({
        next: () => {
          window.open(
            `http://localhost:5088/api/fileaccess/open/${fileId}`,
            '_blank'
          );
        },
        error: () => {
          alert('Failed to log access');
        }
      });
  }
}