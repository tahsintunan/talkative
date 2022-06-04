import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { catchError, EMPTY } from 'rxjs';
import { UserService } from '../user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  rows = [];
  selected = [];

  editing = {};

  constructor(
    private _userService: UserService
  ) { }

  ngOnInit(): void {
    this._userService.getUsers().subscribe(res => {
      console.log(res);
      let response: any = res
      this.rows = response;
    }, catchError(err => {
      console.log(err);

      return EMPTY
    }))
  }

  convertToDate(date) {
    const dateOfBirth = new Date(date)
    let day = dateOfBirth.getDate();
    let month = dateOfBirth.getMonth() + 1;
    let year = dateOfBirth.getFullYear();
    const fullDate: string = year + "-" + month + "-" + (day < 10 ? "0" + day : day);
    return fullDate
  }

  updateValue(event, cell, rowIndex) {
    this.editing[rowIndex + '-' + cell] = false;
    this.rows[rowIndex][cell] = event.target.value;
    this.rows = [...this.rows];
  }

  deleteUser(id) {
    console.log(id)
  }

}
