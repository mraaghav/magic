import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpService } from 'src/app/services/http-service';

@Component({
  selector: 'app-security',
  templateUrl: './security.component.html',
  styleUrls: ['./security.component.scss']
})
export class SecurityComponent {

  public password = '';
  public passwordRepeat = '';

  constructor(
    private snackBar: MatSnackBar,
    private service: HttpService) { }

  save() {
    if (this.password !== this.passwordRepeat || this.password === '') {
      this.snackBar.open('Passwords must match and be an actual password', 'Close', {
        duration: 5000,
        panelClass: ['error-snackbar'],
      });
      return;
    }
    this.service.changeMyPassword(this.password).subscribe(res => {
      this.snackBar.open('Your password was successfully changed', 'Close', {
        duration: 2000,
      });
    }, error => {
      this.snackBar.open(error.error.message, 'Close', {
        duration: 5000,
        panelClass: ['error-snackbar'],
      });
    });
  }
}
