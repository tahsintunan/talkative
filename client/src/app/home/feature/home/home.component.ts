import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserModel } from '../../models/user.model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  profileId: string = '';
  userAuth?: UserModel;

  constructor(
    private userService: UserService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.userService.init();

    if (this.route.snapshot.children.length > 0) {
      const childParameter = this.route.children[0].snapshot.params['userId'];
      this.profileId = childParameter;
    }
  }
}
