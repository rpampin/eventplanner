import { EventType } from '../event-types/event-type.model';
import { Guest } from '../guest/guest';
import { Supplier } from '../supplier/supplier';

export class Event {
    id: string;
    type: EventType;
    date: Date;
    celebrant: string;
    address: string;
    mobile: string;
    email: string;
    package: string;
    downPayment: number;
    balance: number;

    brideName: string;
    groomName: string;
    ceremonyVenue: string;
    ceremonyTime: string;
    receptionVenue: string;
    receptionTime: string;

    emailSubject: string;
    emailTemplate: string;
    
    guests: Guest[] = [];
    suppliers: Supplier[] = [];
    // plan: Plan;
    
    constructor() { }
}
