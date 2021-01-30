import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faTrash, faSave } from '@fortawesome/free-solid-svg-icons';
import { Plan, PlanPart, PlanStep } from './plan';

@Component({
  selector: 'app-plan',
  templateUrl: './plan.component.html',
  styleUrls: ['./plan.component.css']
})
export class PlanComponent implements OnInit {
  faTrash = faTrash;
  faSave = faSave;
  event: any = {};
  eventId: string;
  plan: Plan = new Plan();

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.eventId = params.get('eventId');

      this.http.get<any>(this.baseUrl + 'api/events/base-data/' + this.eventId).subscribe(result => {
        this.event = result;

        this.http.get<Plan>(this.baseUrl + 'api/plans/' + this.eventId).subscribe(result => {
          this.plan = result;
        }, error => console.error(error));

      }, error => console.error(error));
    });
  }

  // PARTS

  addNewPart() {
    this.http.post<PlanPart>(this.baseUrl + 'api/planparts/' + this.plan.id, null).subscribe(result => {
      this.plan.parts.push(result);
    }, error => console.error(error));
  }

  removePart(part: PlanPart) {
    this.http.delete(this.baseUrl + 'api/planparts/' + part.id).subscribe(() => {
      this.plan.parts = this.plan.parts.filter(function (p) {
        return p.id !== part.id;
      });
    }, error => console.error(error));
  }

  // STEPS

  addNewStep(part: PlanPart) {
    this.http.post<PlanStep>(this.baseUrl + 'api/plansteps/' + part.id, null).subscribe(result => {
      part.steps.push(result);
    }, error => console.error(error));
  }

  removeStep(step: PlanStep) {

  }
}
