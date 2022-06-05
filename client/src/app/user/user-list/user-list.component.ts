import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { catchError, EMPTY } from 'rxjs';
import { UserService } from '../user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css'],
})
export class UserListComponent implements OnInit {
  rows = [];
  selected = [];
  loading = false;
  editing = {};

  constructor(
    private _userService: UserService,
    private _snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.loading = true;
    this._userService.getUsers().subscribe(
      (res) => {
        console.log(res);
        let response: any = res;
        this.rows = response;
        this.loading = false;
      },
      catchError((err) => {
        console.log(err);

        return EMPTY;
      })
    );
  }

  convertToDate(date) {
    const dateOfBirth = new Date(date);
    let day = dateOfBirth.getDate();
    let month = dateOfBirth.getMonth() + 1;
    let year = dateOfBirth.getFullYear();
    const fullDate: string =
      year +
      '-' +
      (month < 10 ? '0' + month + '' : month) +
      '-' +
      (day < 10 ? '0' + day + '' : day);
    return fullDate;
  }

  updateValue(event, cell, rowIndex, row) {
    if (event.target.value === row[cell]) {
      this.editing[rowIndex + '-' + cell] = false;
      return;
    }
    if (event.target.value == '' || event.target.value == null) {
      this.rows[rowIndex][cell] = row[cell];
      this.editing[rowIndex + '-' + cell] = false;
      return;
    }

    const body = {
      ...row,
    };

    body[cell] = event.target.value;

    this.editing[rowIndex + '-' + cell] = false;
    this.updateUser(body);
  }

  updateUser(user) {
    this._userService.updateUser(user).subscribe({
      next: (res) => {
        console.log(res);
        this.getUsers();
      },
      error: (err) => {
        let error = err.error.errors;
        this.handleError(error);
      },
    });
  }

  handleError(error) {
    let errorMessage;

    if (error.Email) {
      errorMessage = error.Email[0];
    } else if (error.DateOfBirth) {
      errorMessage = error.DateOfBirth[0];
    } else {
      errorMessage = 'Something went wrong.';
    }

    this._snackBar.open(errorMessage, 'Undo', {
      duration: 2000,
    });
  }

  deleteUser(id) {
    this._userService.deleteUser(id).subscribe({
      next: (res) => this.getUsers(),
      error: (err) => {
        this._snackBar.open('Something went wrong.', 'Undo', {
          duration: 2000,
        });
      },
    });
  }
}
