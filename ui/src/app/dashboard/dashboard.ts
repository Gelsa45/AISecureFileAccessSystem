import {
  Component,
  OnInit,
  OnDestroy,
  ChangeDetectorRef
} from '@angular/core';

import { ApiService } from '../services/api';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class DashboardComponent
  implements OnInit, OnDestroy {

  loading = true;

  alerts: any[] = [];
  activityLogs: any[] = [];

  private refreshInterval: any;

  constructor(
    private api: ApiService,
    private cdr: ChangeDetectorRef
  ) {}

  get totalActivities(): number {
    return this.activityLogs.length;
  }

  get highRiskAlerts(): number {
    return this.alerts.length;
  }

  ngOnInit(): void {

    this.loadAlerts();
    this.loadActivityLogs();

    this.refreshInterval = setInterval(() => {

      this.loadAlerts();
      this.loadActivityLogs();

    }, 1000);
  }

  ngOnDestroy(): void {

    if (this.refreshInterval) {
      clearInterval(this.refreshInterval);
    }
  }

  loadAlerts(): void {

    this.api.getAlerts().subscribe({

      next: (data: any) => {

        this.alerts = [...data];

        this.loading = false;

        this.cdr.detectChanges();
      },

      error: (err) => {

        console.error(err);

        this.loading = false;
      }
    });
  }

  loadActivityLogs(): void {

    this.api.getActivityLogs().subscribe({

      next: (data: any) => {

        console.log('ACTIVITY RECEIVED:', data);

        this.activityLogs = [...data];

        this.loading = false;

        this.cdr.detectChanges();
      },

      error: (err) => {

        console.error(err);
      }
    });
  }

  clearLogs(): void {

    this.api.clearLogs().subscribe({

      next: () => {

        this.alerts = [];
        this.activityLogs = [];

        this.loadAlerts();
        this.loadActivityLogs();

        this.cdr.detectChanges();
      },

      error: (err) => {

        console.error(err);
      }
    });
  }
}