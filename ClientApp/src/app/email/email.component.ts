import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { Attachment } from '../supplier-form/attachment';
import { ToastService } from '../toast.service';
import { Email } from './email';

@Component({
  selector: 'app-email',
  templateUrl: './email.component.html',
  styleUrls: ['./email.component.css']
})
export class EmailComponent implements OnInit {
  faTrash = faTrash;

  email: Email = new Email();
  attachment: Attachment;
  attachmentInput: any;
  allowUpload: boolean = false;

  public options: Object = {
    placeholderText: 'Edit Your Content Here!',
    events: {
      "image.beforeUpload": function (files) {
        var editor = this;
        if (files.length) {
          // Create a File Reader.
          var reader = new FileReader();
          // Set the reader to insert images when they are loaded.
          reader.onload = function (e) {
            var result = e.target.result;
            editor.image.insert(result, null, null, editor.image.get());
          };
          // Read image as base64.
          reader.readAsDataURL(files[0]);
        }
        editor.popups.hideAll();
        // Stop default upload chain.
        return false;
      }
    }
  }

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    public toastService: ToastService) { }

  ngOnInit() {
  }

  handleUpload(event) {
    this.attachmentInput = event.target;
    const file = event.target.files[0];
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      this.attachment = new Attachment();
      this.attachment.name = file.name;
      this.attachment.base64 = reader.result.toString();
      this.allowUpload = true;
    };
  }

  postAttachment() {
    this.email.attachments.push({ ...this.attachment });
    this.attachmentInput.value = null;
    this.attachment = new Attachment();
    this.allowUpload = false;
  }

  deleteAttachment(index: number) {
    debugger;
    this.email.attachments.splice(index, 1);
  }

  clearEmail() {
    this.email = new Email();
  }

  submit() {
    this.http.post(this.baseUrl + "api/email", this.email).subscribe(() => {
      this.toastService.show(`Email sent successfuly`, { classname: 'bg-success text-light' });
      this.email = new Email();
    });
  }
}
