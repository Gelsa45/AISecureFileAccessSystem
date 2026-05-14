import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css'] 
  
})
export class DashboardComponent implements OnInit {

  loading = true;
  alerts: any[] = [];
  users: any[] = [];
  files: any[] = [];

  selectedUserId = 1;
  selectedFileId = 1;

  constructor(private api: ApiService, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.loadAlerts();
    this.loadUsers();
    this.loadFiles();
  }

  // 🔹 Load alerts from backend
  loadAlerts() {
    this.loading = true;

    this.api.getAlerts().subscribe({
      next: (data: any) => {
        console.log("API DATA:", data);
        this.alerts = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error("Error loading alerts:", err);
        this.loading = false;
      }
    });
  }
  loadUsers() {
  this.api.getUsers().subscribe((data: any) => {
    this.users = data;
  });
}

loadFiles() {
  this.api.getFiles().subscribe((data: any) => {
    this.files = data;
  });
}

  // 🔹 Simulate file access
  simulateAccess() {
    console.log("Simulate clicked");

    this.api.logAccess(this.selectedUserId, this.selectedFileId).subscribe({
      next: (res) => {
        console.log("Access simulated:", res);
        this.loadAlerts();
      },
      error: (err) => {
        console.error("Simulate error:", err);
      }
    });
  }

  // 🔹 Clear logs
  clearLogs() {
    console.log("Clear button clicked");

    this.api.clearLogs().subscribe({
      next: (res) => {
        console.log("Logs cleared:", res);
        this.loadAlerts();
      },
      error: (err) => {
        console.error("Clear error:", err);
      }
    });
  }
}