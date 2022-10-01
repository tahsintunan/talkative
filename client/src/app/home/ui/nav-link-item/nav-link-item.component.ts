import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav-link-item',
  templateUrl: './nav-link-item.component.html',
  styleUrls: ['./nav-link-item.component.css'],
})
export class NavLinkItemComponent implements OnInit {
  @Input() link?: string;
  @Input() icon?: string;
  @Input() text?: string;

  constructor() {}

  ngOnInit(): void {}
}
