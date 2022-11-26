import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { debounceTime, distinctUntilChanged } from 'rxjs';
import { PaginationModel } from 'src/app/core/models/pagination.model';
import { UserModel } from 'src/app/core/models/user.model';
import { AdminService } from 'src/app/core/services/admin.service';
import { UserStore } from 'src/app/core/store/user.store';
import { ProfileUpdateDialogComponent } from 'src/app/features/profile/ui/profile-update-dialog/profile-update-dialog.component';

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
  userList: UserModel[] = [];

  userAuth?: UserModel;

  formControl = new FormControl<string>('');

  constructor(
    private adminService: AdminService,
    private userStore: UserStore,
    public dialog: MatDialog,
    private router: Router
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

  onRowClick(user: UserModel) {
    this.router.navigate(['/profile', user.userId]);
  }

  search(value: string) {
    this.pagination.pageNumber = 1;
    this.adminService.searchUser(value!, this.pagination).subscribe((res) => {
      this.userList = res;
    });
  }

  getUsers() {
    this.adminService.getUserList(this.pagination).subscribe((res) => {
      if (this.pagination.pageNumber === 1) this.userList = res;
      else this.userList = this.userList.concat(res);
    });
  }

  editUser(user: UserModel) {
    const dialogRef = this.dialog.open(ProfileUpdateDialogComponent, {
      width: '500px',
      data: user,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.adminService.updateUser(result).subscribe(() => {
          this.userList = this.userList.map((user) =>
            user.userId === result.userId ? result : user
          );
        });
      }
    });
  }

  banUser(userId: string) {
    this.adminService.banUser(userId).subscribe(() => {
      this.userList = this.userList.map((user) =>
        user.userId === userId ? { ...user, isBanned: true } : user
      );
    });
  }

  unbanUser(userId: string) {
    this.adminService.unbanUser(userId).subscribe(() => {
      this.userList = this.userList.map((user) =>
        user.userId === userId ? { ...user, isBanned: false } : user
      );
    });
  }
}
