import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css'],
})
export class AlertComponent implements OnInit {
  @Input() type: 'default' | 'info' | 'success' | 'warning' | 'danger' =
    'default';
  @Input() title: string = 'Alert';
  @Input() message: string = 'Alert message';

  constructor() {}

  ngOnInit(): void {}
}
