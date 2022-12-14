import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IAuthenticated } from '../../models/authenticated.model';
import { ILogin } from '../../models/login.model';
import { IToken, UserRole } from '../../models/token.model';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.sass']
})
export class LoginPageComponent{

  constructor(private loginService: LoginService, private router: Router) { 
  }

  logIn(requestBody: ILogin){
    console.log("logIn()");
    this.loginService.isLoading$.next(true);
    this.loginService.logIn(requestBody).subscribe(
      {
        next: (response: IToken) => {
          console.log("logIn() - next");
          // Set token
          console.log("Login data: ", requestBody);
          this.setToken(response);
        },
        error: (err: Error) => {
          console.error(`Error logging in: ${err.message}`);
          this.loginService.isLoading$.next(false);
        },
        complete: () => { }
      }
    );
  }

  async setToken(tokenResponse: IToken) {
    console.log("setToken()");
    this.loginService.isLoading$.next(true);
    this.loginService.setToken(tokenResponse.token).subscribe({
      next: (response: HttpResponse<IAuthenticated>) => {
        console.log("setToken() - next");
        const userInfo: IToken = {
          userId: tokenResponse.userId,
          userName: tokenResponse.userName,
          firstName: tokenResponse.firstName,
          lastName: tokenResponse.lastName,
          email: tokenResponse.email,
          expiration: tokenResponse.expiration,
          validity: tokenResponse.validity,
          refreshToken: "",
          token: "",
          role: tokenResponse.role,
          welcomeMessage: tokenResponse.welcomeMessage
        }
        localStorage.setItem("user_info", JSON.stringify(userInfo));
        this.navigateToHome()
      },
      error: (err: Error) => {
        console.error("Error checking token cookie: ", err.message);
        this.loginService.isLoading$.next(false);
      },
      complete: () => console.log("Token cookie check complete")
    });
  }

  navigateToHome() {
    console.log("navigateToHome()");
    this.loginService.checkTokenCookie().subscribe({
      next: (response: HttpResponse<IAuthenticated>) => {
        console.log("navigateToHome() - next");
        // Check token
        this.loginService.isLoggedIn = response.body?.authenticated;
        const isLoggedIn = response.body?.authenticated;
        console.log("authenticated: ", response.body?.authenticated)
        console.log("isLoggedIn: ", isLoggedIn);
        if (this.loginService.isLoggedIn === true) {
          console.log("You are logged in");
          this.loginService.userRole = JSON.parse(localStorage.getItem("user_info")!).role as UserRole
          if (this.loginService.userRole == UserRole.Administrator)
            this.router.navigate(['admin']);
          else
            this.router.navigate(['']);
        }
        else {
          console.log("Could not log in");
        }
      },
      error: (err: Error) => console.error("Error checking token cookie: ", err.message),
      complete: () => {
        console.log("Token cookie check complete");
        this.loginService.isLoading$.next(false);
      }
    });
  }
}
