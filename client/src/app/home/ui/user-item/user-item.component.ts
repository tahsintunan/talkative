import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ProfileModel } from '../../Models/profile.model';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.css'],
})
export class UserItemComponent implements OnInit {
  @Input() profile?: ProfileModel;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void { }

  profileClicked() {
    this.router.navigate(['./', 'chat', this.profile?.userId], { relativeTo: this.activatedRoute })
  }
}
