import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css'],
})
export class SigninComponent implements OnInit {
  hidePassword: boolean = true;

  formData: FormGroup = this.formBuilder.group({
    username: [null, [Validators.required]],
    password: [null, [Validators.required]],
  });

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  onSubmit(): void {
    this.authService
      .signin(this.formData.getRawValue())
      .subscribe(() => this.router.navigate(['/']));
  }
}
