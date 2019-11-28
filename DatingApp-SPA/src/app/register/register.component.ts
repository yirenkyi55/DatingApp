import { AlertifyService } from './../_services/alertify.service';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  constructor(
    private authService: AuthService,
    private alertify: AlertifyService
  ) {}

  // @Input() valuesFromHome: any; // From parent component(home) to child(register)

  @Output() cancelRegister = new EventEmitter(); // From child(register) to parent(home)

  ngOnInit() {}

  register() {
    this.authService.register(this.model).subscribe(
      next => {
        this.alertify.success('Registered successfully');
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
