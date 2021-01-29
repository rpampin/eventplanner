import { SupplierType } from '../supplier-types/supplier-type.model';

export class Supplier {
    id: string;
    name: string;
    type: SupplierType;
    contactPerson: string;
    mobile: string;
    email: string;
    // attachment: IEnumerable;
    downPaymentRequired: boolean;
    packagePrice: number;
    otherPayments: number;
    firstDownPayment: number;
    secondDownPayment: number;
    thirdDownPayment: number;
    totalDown: number;
    balance: number;
    remarks: string;
}
