import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.css'],
})
export class UserItemComponent implements OnInit {
  @Input() profile?: ProfileModel;

  constructor() {}

  ngOnInit(): void {}
}
