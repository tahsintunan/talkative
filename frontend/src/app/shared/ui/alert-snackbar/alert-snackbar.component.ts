import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import {
  MatSnackBarRef,
  MAT_SNACK_BAR_DATA,
} from '@angular/material/snack-bar';

interface AlertSnackbarData {
  title: string;
  message: string;
  type: 'default' | 'info' | 'notice' | 'success' | 'warning' | 'danger';
}

@Component({
  selector: 'app-alert-snackbar',
  templateUrl: './alert-snackbar.component.html',
  styleUrls: ['./alert-snackbar.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AlertSnackbarComponent implements OnInit {
  timeoutRef: any;

  constructor(
    public snackbarRef: MatSnackBarRef<AlertSnackbarComponent>,
    @Inject(MAT_SNACK_BAR_DATA) public data: AlertSnackbarData
  ) {}

  ngOnInit(): void {
    this.timeoutRef = setTimeout(() => {
      this.snackbarRef.dismiss();
    }, 5000);
  }

  onClose(): void {
    clearTimeout(this.timeoutRef);
    this.snackbarRef.dismiss();
  }

  onMouseHover(): void {
    clearTimeout(this.timeoutRef);
  }

  onMouseLeave(): void {
    this.timeoutRef = setTimeout(() => {
      this.snackbarRef.dismiss();
    }, 5000);
  }
}
