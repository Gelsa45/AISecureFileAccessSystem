import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html'
})
export class DashboardComponent implements OnInit {
  loading = true;
  alerts: any[] = [];
  
  constructor(private api: ApiService, private cdr: ChangeDetectorRef) {}
  
  ngOnInit() {
    this.loadAlerts();
  }

  loadAlerts() {
    this.api.getAlerts().subscribe((data: any) => {
      console.log("API DATA:", data);

      this.alerts = data;
      this.loading = false;   
      this.cdr.detectChanges(); 
    });
  }
simulateAccess() {
  this.api.logAccess(1, 1).subscribe({
    next: (res) => {
      console.log("Access simulated", res);
      this.loadAlerts();
    },
    error: (err) => {
      console.error(err);
    }
  });
}
}