import { SupplierType } from '../supplier-types/supplier-type.model';
import { Event } from '../event/event';
import { Attachment } from '../supplier-form/attachment';

export class Supplier {
    id: string;
    name: string;
    type: SupplierType;
    contactPerson: string;
    mobile: string;
    email: string;
    attachments: Attachment[];
    downPaymentRequired: boolean;
    packagePrice: number;
    discount: number;
    otherPayments: number;
    firstDownPayment: number;
    secondDownPayment: number;
    thirdDownPayment: number;
    totalDown: number;
    balance: number;
    remarks: string;
    event: Event;

    constructor() { }
}
