import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css'],
})
export class AlertComponent implements OnInit {
  @Input() alertType: string = 'default';
  @Input() alertTitle: string = 'Alert';
  @Input() massage: string = 'Alert message';

  constructor() {}

  ngOnInit(): void {}
}
