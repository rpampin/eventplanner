import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-print-report',
  templateUrl: './print-report.component.html',
  styleUrls: ['./print-report.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class PrintReportComponent implements OnInit {
  eventId: string;
  reportName: string;
  html: string;

  constructor(
    private http: HttpClient, 
    @Inject('BASE_URL') private baseUrl: string, 
    private router: Router, 
    private route: ActivatedRoute,
    private location: Location) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.eventId = params.get('eventId');

      this.route.queryParams.subscribe(params => {
        this.reportName = params['reportName'];

        switch (this.reportName) {
          case "budget-form":
            this.getBudgetForm();
            break;
          case "checklist":
            this.getChecklist();
            break;
          case "program":
            this.getProgram();
            break;
          default:
            break;
        }
      });

    });
  }

  getBudgetForm() {
    this.http.get<any>(this.baseUrl + 'api/reports/budget-form/' + this.eventId).subscribe(result => {
      this.html = result.html;
    }, error => console.error(error));
  }

  getChecklist() {
    this.http.get<any>(this.baseUrl + 'api/reports/checklist/' + this.eventId).subscribe(result => {
      this.html = result.html;
    }, error => console.error(error));
  }

  getProgram() {
    this.http.get<any>(this.baseUrl + 'api/reports/program/' + this.eventId).subscribe(result => {
      this.html = result.html;
    }, error => console.error(error));
  }

  goBack() {
    this.location.back();
  }

  print() {
    const el1 = document.getElementById('back-btn');
    const el2 = document.getElementById('print-btn');
    const el3 = document.getElementById('nav-menu');

    el1.hidden = true;
    el2.hidden = true;
    el3.hidden = true;

    window.print();

    el1.hidden = false;
    el2.hidden = false;
    el3.hidden = false;
  }
}
