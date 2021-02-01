import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastService } from '../toast.service';
import { SmtpConfig } from './smtpConfig.model';

@Component({
  selector: 'app-config',
  templateUrl: './config.component.html',
  styleUrls: ['./config.component.css']
})
export class ConfigComponent implements OnInit {
  smtpConfig: SmtpConfig = new SmtpConfig();

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    public toastService: ToastService) { }

  ngOnInit() {
    this.http.get<any>(this.baseUrl + 'api/config').subscribe(result => {
      this.smtpConfig = result.smtpConfig || new SmtpConfig();
    }, error => console.error(error));
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

}
