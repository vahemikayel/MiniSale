import { AppConstants } from './appConstants.model'

export class ProductModel {
    id: string;
    name : string;
    price : number;
    barCode : string;
    PLU : number;

    constructor(data?:ProductModel){
        this.id = data?.id || AppConstants.emptyGuid;
        this.name = data?.name || '';
        this.price = data?.price || 0;
        this.PLU = data?.PLU || 0;
        this.barCode = data?.barCode || ''
    }
}