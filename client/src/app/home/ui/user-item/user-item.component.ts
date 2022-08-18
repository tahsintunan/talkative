import { Component, Input, OnInit } from '@angular/core';
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

  getProfileImage(): string {
    return 'https://img.icons8.com/fluency/344/fox.png';
  }
}
