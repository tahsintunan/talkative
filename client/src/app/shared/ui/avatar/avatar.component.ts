import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-avatar',
  templateUrl: './avatar.component.html',
  styleUrls: ['./avatar.component.css'],
})
export class AvatarComponent implements OnInit {
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
