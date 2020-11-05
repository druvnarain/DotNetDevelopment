import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    PaginationModule.forRoot() //loads providers in ngx-bootstrap for root module, acts as singleton
  ],
  exports: [PaginationModule]
})
export class SharedModule { }
