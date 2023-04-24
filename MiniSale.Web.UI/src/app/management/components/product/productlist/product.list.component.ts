import { Component, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource, MatTableDataSourcePaginator } from '@angular/material/table';
import { ProductModel } from '../../../../shared/models/product.model'
import { ManagementService } from './../../../services/management.service'
import { IDummyProductParams } from './../../../../shared/models/IDummyProductParams';
import {SelectionModel} from '@angular/cdk/collections';

@Component({
    selector: 'app-home',
    templateUrl: './product.list.component.html',
    styleUrls: ['./product.list.component.scss']
  })

export class ProductListComponent implements OnInit {
    displayedColumns = [ 'select', 'name', 'barCode', 'PLU', 'price'];
    dataSource: MatTableDataSource<ProductModel> = new MatTableDataSource<ProductModel>();
    selection = new SelectionModel<ProductModel>(false, []);
    selectedProduct: ProductModel = {barCode:'', id:'', name:'', PLU:0, price:0};
    formData: IDummyProductParams = {
      count: 50000,
      pluMin: 1,
      pluMax: 99999,
      priceMin: 10,
      priceMax: 5000
    }

    constructor(private managementService: ManagementService,
                
     ) {
      
    }
  

    
    ngOnInit(): void {
      this.getAllProducts();
    }
  
    getAllProducts() {
      this.managementService.getAllProducts().subscribe({
        next: (res) => {
          this.dataSource = new MatTableDataSource<ProductModel>(res);
          // if (res && res.length > 0) {
          //   res.forEach(item => {
          //     this.dataSource.data.push(item);
          //   })
          //   this.selectApp(this.dataSource[0]);
          // }
          // this.loaded = true;
        },
        error: (error) => {
          console.log(error);
        }
      });
    }

    onSubmitGenerate() {
      this.managementService.generateDummyProducts(this.formData).subscribe({
        next: (res) => {
          this.dataSource = new MatTableDataSource<ProductModel>(res);
        },
        error: (error) => {
          console.log(error);
        }
      });
    }

    onAdd() {
      console.log('Add button clicked');
      // Add your logic to handle the Add button click event
    }
  
    onEdit() {
      console.log('Edit button clicked');
      // Add your logic to handle the Edit button click event
    }
  
    onRemove() {
      if (this.selection.selected.length == 0)
        return;

      this.managementService.removeProduct(this.selection.selected[0].id).subscribe({
        next: (res) => {
        //   const index: number = this.dataSource.data.indexOf(this.selection.selected[0]);
        //   if (index !== -1) {
        //     this.dataSource.data.splice(index, 1);
        // } 
          this.dataSource = new MatTableDataSource<ProductModel>(this.dataSource.data.filter(x=>x != this.selection.selected[0]));
        },
        error: (error) => {
          console.log(error);
        }
      });
    }

    onSelect(product: ProductModel): void {
      this.selectedProduct = product;
    }

    isAllSelected() {
      const numSelected = this.selection.selected.length;
      const numRows = this.dataSource.data.length;
      return numSelected === numRows;
    }
  
    /** Selects all rows if they are not all selected; otherwise clear selection. */
    masterToggle() {
      this.isAllSelected() ?
          this.selection.clear() :
          this.dataSource.data.forEach(row => this.selection.select(row));
    }
  }
  