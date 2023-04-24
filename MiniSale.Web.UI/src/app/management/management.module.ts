import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductListComponent } from './components/product/productlist/product.list.component';
import { ManagementRoutingModule } from './management-routing.module';
import { SharedModule } from '../shared/shared.module';
import { MaterialComModule } from '../shared/material.module';

@NgModule({
    declarations: [
        ProductListComponent
    ],
    imports: [
      CommonModule,
      ManagementRoutingModule,
      SharedModule,
      MaterialComModule
    ]
  })
  export class ManagementModule { }