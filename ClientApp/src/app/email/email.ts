import { Attachment } from '../supplier-form/attachment';

export class Email {
    constructor(){}

    body: string;
    subject: string;
    to: string[];
    cc: string[];
    bcc: string[];
    attachments: Attachment[] = [];
}
