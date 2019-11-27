import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  values: any;
  constructor() {}

  ngOnInit() {}

  setRegisterMode() {
    this.registerMode = true;
  }

  // For child(register) to parent(this) communication
  cancelRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }
}
