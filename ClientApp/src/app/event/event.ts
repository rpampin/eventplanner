import { EventType } from '../event-types/event-type.model';
import { Guest } from '../guest/guest';
import { Attachment } from '../supplier-form/attachment';
import { Supplier } from '../supplier/supplier';

export class Event {
    id: string;
    type: EventType;
    date: Date;
    celebrant: string;
    address: string;
    mobile: string;
    email: string;
    package: any;
    packagePrice: number;
    balance: number;
    downPayment: number;
    notes: number;
    additionalCharges: number;

    brideName: string;
    groomName: string;
    ceremonyVenue: string;
    ceremonyTime: string;

    brideAddress: string;
    brideMobile: string;
    brideEmail: string;
    groomAddress: string;
    groomMobile: string;
    groomEmail: string;
    brideSocial: string;
    groomSocial: string;

    preparationVenue: string;
    preparationTime: string;
    receptionVenue: string;
    receptionTime: string;

    emailSubject: string;
    emailTemplate: string;

    guests: Guest[] = [];
    suppliers: Supplier[] = [];
    attachments: Attachment[] = [];
    // plan: Plan;

    constructor() { }
}
