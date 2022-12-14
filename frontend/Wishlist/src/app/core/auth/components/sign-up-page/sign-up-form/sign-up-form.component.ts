import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { ISignUp } from '../../../models/sign-up.model';

@Component({
  selector: 'app-sign-up-form',
  templateUrl: './sign-up-form.component.html',
  styleUrls: ['./sign-up-form.component.sass']
})
export class SignUpFormComponent implements OnInit{
  signUpForm: FormGroup = new FormGroup({});
  signUpValues: ISignUp | undefined;

  hide = true;
  showProgressBar = false;

  @Output() onSignUp = new EventEmitter<ISignUp>
  //((requestBody: ISignUp) => void) | undefined;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(){
    this.signUpForm = this.formBuilder.group({
      firstName: new FormControl(''),
      lastName: new FormControl(''),
      email: new FormControl(''),
      username: new FormControl(''),
      password: new FormControl('')
    });
  }

  submitForm(){
    this.signUpValues = this.signUpForm.value;
    console.log(`Form values: ${JSON.stringify(this.signUpValues)}`);
    if (this.signUpValues)
      this.onSignUp.emit(this.signUpValues);
    else
      console.error("Sign up form not submitted correctly");
  }

  resetForm(){
    this.signUpForm.reset();
  }

}
