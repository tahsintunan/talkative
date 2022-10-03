import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-avater',
  templateUrl: './avater.component.html',
  styleUrls: ['./avater.component.css'],
})
export class AvaterComponent implements OnInit {
  @Input() alt?: string = '';
  @Input() src?: string = '';
  @Input() size?: string = '40px';
  @Input() generateAvater?: boolean = false;
  @Input() bgColor?: string = 'transparent';
  @Input() borderWidth?: string = '0px';
  @Input() borderColor?: string = 'transparent';
  @Input() borderRadius?: string = '50px';

  constructor() {}

  ngOnInit(): void {}
}
