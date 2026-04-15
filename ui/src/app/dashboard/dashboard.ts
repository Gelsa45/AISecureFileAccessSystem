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
  this.api.getAlerts().subscribe((data: any) => {
    console.log("API DATA:", data);

    this.alerts = data;
    this.loading = false;   
    this.cdr.detectChanges(); 
  });
}
}