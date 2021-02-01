import { EventType } from '../event-types/event-type.model';
import { Guest } from '../guest/guest';
import { Supplier } from '../supplier/supplier';

export class SmtpConfig {
    id: string;
    host: string;
    port: number;
    username: string;
    password: string;
    
    constructor() { }
}
