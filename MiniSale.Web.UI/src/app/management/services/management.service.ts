import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProductModel } from '../../shared/models/product.model';
import {IDummyProductParams} from './../../shared/models/IDummyProductParams'
import { environment } from 'src/environments/environment';
import { map, Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
  })
export class ManagementService {
    baseUrl: string = environment.apiUrl;
    baseApiVersion: string = environment.apiVersion;
    baseUrlWithVersion = this.baseUrl + this.baseApiVersion;
    baseManagementUrl = this.baseUrlWithVersion + 'management/';

    constructor(private http: HttpClient) { }

    getAllProducts(): Observable<Array<ProductModel>> {
        return this.http.get<Array<ProductModel>>(this.baseManagementUrl + 'getProducts');
      }

    generateDummyProducts(conf:IDummyProductParams): Observable<Array<ProductModel>> {
        return this.http.post<Array<ProductModel>>(this.baseManagementUrl + 'GenerateProductData', conf);
      }

    removeProduct(id:string): Observable<boolean> {
        let params = new HttpParams();
        params = params.append('Id', id);
        return this.http.delete<boolean>(this.baseManagementUrl + 'ProductRemove', { params });
      }

    addProduct(prod:ProductModel): Observable<ProductModel> {
        return this.http.post<ProductModel>(this.baseManagementUrl + 'ProductAdd', prod);
      }

    editProduct(prod:ProductModel): Observable<ProductModel> {
        return this.http.post<ProductModel>(this.baseManagementUrl + 'ProductEdit', prod);
      }
}