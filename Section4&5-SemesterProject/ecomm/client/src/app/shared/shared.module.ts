import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';


@NgModule({
  declarations: [PagingHeaderComponent, PagerComponent],
  imports: [
    CommonModule,
    PaginationModule.forRoot() //loads providers in ngx-bootstrap for root module, acts as singleton
  ],
  exports: [PaginationModule, PagingHeaderComponent, PagerComponent]
})
export class SharedModule { }
