import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { PaginationModel } from 'src/app/core/models/pagination.model';
import { UserModel } from 'src/app/core/models/user.model';
import { AdminService } from 'src/app/core/services/admin.service';
import { UserStore } from 'src/app/core/store/user.store';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css'],
})
export class UserManagementComponent implements OnInit {
  pagination: PaginationModel = {
    pageNumber: 1,
  };
  displayedColumns: string[] = [
    'userId',
    'username',
    'email',
    'dateOfBirth',
    'isBanned',
  ];
  dataSource: UserModel[] = [];

  userAuth?: UserModel;

  formControl = new FormControl<string>('');

  constructor(
    private adminService: AdminService,
    private userStore: UserStore
  ) {}

  ngOnInit(): void {
    this.userStore.userAuth.subscribe((user) => (this.userAuth = user));

    this.getUsers();

    this.formControl.valueChanges
      .pipe(debounceTime(1000), distinctUntilChanged())
      .subscribe((value) => {
        if (!value?.trim()) this.getUsers();
        else this.search(value?.trim()!);
      });
  }

  onScroll() {
    this.pagination.pageNumber++;
    if (!this.formControl.value?.trim()) this.getUsers();
    else this.search(this.formControl.value?.trim()!);
  }

  search(value: string) {
    this.pagination.pageNumber = 1;
    this.adminService.searchUser(value!, this.pagination).subscribe((res) => {
      this.dataSource = res;
    });
  }

  getUsers() {
    this.adminService.getUserList(this.pagination).subscribe((res) => {
      if (this.pagination.pageNumber === 1) this.dataSource = res;
      else this.dataSource = this.dataSource.concat(res);
    });
  }

  banUser(userId: string) {
    this.adminService.banUser(userId).subscribe(() => {
      this.dataSource = this.dataSource.map((user) =>
        user.userId === userId ? { ...user, isBanned: true } : user
      );
    });
  }

  unbanUser(userId: string) {
    this.adminService.unbanUser(userId).subscribe(() => {
      this.dataSource = this.dataSource.map((user) =>
        user.userId === userId ? { ...user, isBanned: false } : user
      );
    });
  }
}
