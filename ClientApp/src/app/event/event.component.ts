import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Event } from './event';
import { EventType } from '../event-types/event-type.model';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { faUser, faUserTie, faClipboard, faDownload, faTrash } from '@fortawesome/free-solid-svg-icons';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Attachment } from '../supplier-form/attachment';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {
  faUser = faUser;
  faUserTie = faUserTie;
  faClipboard = faClipboard;
  faDownload = faDownload;
  faTrash = faTrash;
  attachment: Attachment = new Attachment();
  attachments: Attachment[] = [];
  attachmentInput: any;
  allowUpload: boolean = false;
  id: string;
  event: Event = new Event();
  weddingEventTyperId: string;
  eventTypes: EventType[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.id = params.get('eventId');
    });
    this.http.get<EventType[]>(this.baseUrl + 'api/eventtypes').subscribe(result => {
      this.eventTypes = result;

      this.http.get<string>(this.baseUrl + 'api/eventtypes/wedding-type-id').subscribe(result => {
        this.weddingEventTyperId = result;

        if (!!this.id) {
          this.http.get<Event>(this.baseUrl + 'api/events/' + this.id).subscribe(result => {
            this.event = result;
            this.attachments = this.event.attachments;
          }, error => console.error(error));
        }

      }, error => console.error(error));

    }, error => console.error(error));

  }

  onSubmit(eventForm: NgForm) {
    if (eventForm.valid) {
      let url = 'api/events';
      if (this.event.type.id == this.weddingEventTyperId) {
        url += '/wedding';
        this.event.celebrant = this.event.brideName + ' | ' + this.event.groomName;
      }

      if (!this.event.id) {
        this.http.post<Event>(this.baseUrl + url, this.event).subscribe(result => {
          this.router.navigate(['/'])
        }, error => console.error(error));
      } else {
        this.http.put<Event>(this.baseUrl + url + '/' + this.event.id, this.event).subscribe(result => {
          this.router.navigate(['/'])
        }, error => console.error(error));
      }
    } else {
      Object.keys(eventForm.controls).forEach(key => {
        eventForm.controls[key].markAsDirty();
      });
    }
  }

  compareFn = (a, b) => this._compareFn(a, b);
  _compareFn(a, b) {
    // Handle compare logic (eg check if unique ids are the same)
    if (!!a && !!b)
      return a.id === b.id;
    return false;
  }

  handleUpload(event) {
    this.attachmentInput = event.target;
    const file = event.target.files[0];
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      this.allowUpload = true;
      this.attachment = new Attachment();
      this.attachment.name = file.name;
      this.attachment.base64 = reader.result.toString();
    };
  }

  postAttachment() {
    this.http.post<Attachment>(this.baseUrl + 'api/attachments', { attachment: this.attachment, eventId: this.event.id }).subscribe(result => {
      this.attachments.push(result);
      this.attachmentInput.value = null;
      this.attachment = new Attachment();
      this.allowUpload = false;
    }, error => console.error(error));
  }

  downloadAttachment(attachmentId: string) {
    this.http.get<Attachment>(this.baseUrl + 'api/attachments/' + attachmentId).subscribe(file => {
      var a = document.createElement("a"); //Create <a>
      a.href = file.base64; //Image Base64 Goes here
      a.download = file.name; //File name Here
      a.click(); //Downloaded file
    }, error => console.error(error));
  }

  deleteAttachment(index: number, attachmentId: string) {
    this.http.delete(this.baseUrl + 'api/attachments/' + attachmentId).subscribe(() => {
      this.attachments.splice(index, 1);
    }, error => console.error(error));
  }
}
