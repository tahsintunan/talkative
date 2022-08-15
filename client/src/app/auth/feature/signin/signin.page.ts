import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../auth.service';
import { SignInReqModel } from '../../models/signin.model';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.page.html',
  styleUrls: ['./signin.page.css'],
})
export class SigninPage implements OnInit {
  hidePassword: boolean = true;

  signinForm: FormGroup = this.formBuilder.group({});

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.signinForm = this.formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', Validators.required],
    });
  }

  submit(): void {
    const signinData: SignInReqModel = this.signinForm.getRawValue();

    this.authService.signin(signinData).subscribe((result) => {
      console.log(result);
    });
  }
}
