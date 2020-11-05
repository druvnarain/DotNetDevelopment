import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Super Awesome E-Commerce Store';

  // services are injected into constructor
  constructor() {}

  ngOnInit(): void {
  }
}
