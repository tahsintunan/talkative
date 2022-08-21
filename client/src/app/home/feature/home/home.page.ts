import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import jwtDecode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';
import { ProfileModel } from '../../models/profile.model';
import { ActiveChatService } from '../../services/active-chat.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.css'],
})
export class Homepage implements OnInit {
  profileId: string = this.getUserId();
  selectedUser?: ProfileModel;

  constructor(
    private cookie: CookieService,
    private activeChat: ActiveChatService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    if (this.route.snapshot.children.length > 0) {
      const childParameter = this.route.children[0].snapshot.params['userId'];
      this.profileId = childParameter;
    }

    this.activeChat.getActivatedChat().subscribe((res) => {
      if (res == '') {
        this.profileId = this.getUserId();
      } else {
        this.profileId = res;
      }
    });
  }

  getUserId(): string {
    const decodedToken: any = jwtDecode(this.cookie.get('authorization'));
    return decodedToken.user_id;
  }
}
