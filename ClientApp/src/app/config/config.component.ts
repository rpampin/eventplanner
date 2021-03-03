import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { forkJoin } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ToastService } from '../toast.service';
import { SmtpConfig } from './smtpConfig.model';

@Component({
  selector: 'app-config',
  templateUrl: './config.component.html',
  styleUrls: ['./config.component.css']
})
export class ConfigComponent implements OnInit {
  smtpConfig: SmtpConfig = new SmtpConfig();
  configuration: any = {};

  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    placeholder: 'Enter text here...',
    sanitize: false
  };

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    public toastService: ToastService) { }

  ngOnInit() {
    forkJoin([
      this.http.get<any>(this.baseUrl + `api/config/smtp`).pipe(tap(result => this.smtpConfig = result || new SmtpConfig())),
      this.http.get<any>(this.baseUrl + `api/config`).pipe(tap(result => this.configuration = result || {})),
    ]).subscribe(() => { });
  }

  submitSmtpConfig(form: NgForm) {
    if (form.valid) {
      this.http.post<SmtpConfig>(this.baseUrl + 'api/config/smtp', this.smtpConfig).subscribe(result => {
        this.smtpConfig = result;
        this.toastService.show(`SMTP updated successfuly`, { classname: 'bg-success text-light' });
      }, error => console.error(error));
    } else {
      Object.keys(form.controls).forEach(key => {
        form.controls[key].markAsDirty();
      });
    }
  }

  submitConfiguration(form: NgForm) {
    if (form.valid) {
      this.http.post<any>(this.baseUrl + 'api/config', this.configuration).subscribe(result => {
        this.configuration = result;
        this.toastService.show(`Configuration updated successfuly`, { classname: 'bg-success text-light' });
      }, error => console.error(error));
    } else {
      Object.keys(form.controls).forEach(key => {
        form.controls[key].markAsDirty();
      });
    }
  }

}
