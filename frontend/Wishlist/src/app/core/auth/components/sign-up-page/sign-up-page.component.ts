import { Component, Inject } from '@angular/core';
import { ISignUp } from '../../models/sign-up.model';
import { IToken } from '../../models/token.model';
import { LoginService } from '../../services/login.service';
import { SignUpService } from '../../services/sign-up.service';
import { LoginPageComponent } from '../login-page/login-page.component';

@Component({
  providers: [LoginPageComponent],
  selector: 'app-sign-up-page',
  templateUrl: './sign-up-page.component.html',
  styleUrls: ['./sign-up-page.component.sass']
})
export class SignUpPageComponent {
  

  constructor(private signUpService: SignUpService, private loginService: LoginService, @Inject(LoginPageComponent) private loginPageComponent: LoginPageComponent) {

  signUp(requestBody: ISignUp){
    this.signUpService.signUp(requestBody).subscribe(
      {
        next: (response: IToken) => {
          console.log("signUp() - next");
          console.log("Received token: ", response.token);
          this.loginPageComponent.setToken(response);
        },
        error: (err: Error) => {
          console.error(`Error signing up: ${err.message}`);
          this.signUpService.isLoading$.next(false);
        },
        complete: () => console.log("Signed up correctly.")
      }
      
    );
  }
}
