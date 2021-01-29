import { Event } from '../event/event';

export class Guest {
    id: string;
    name: string;
    email: string;
    mobile: string;
    willAttend: Boolean;
    event: Event;

    constructor() { }
}
