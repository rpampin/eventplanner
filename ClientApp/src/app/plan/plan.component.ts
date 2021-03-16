import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faTrash, faSave } from '@fortawesome/free-solid-svg-icons';
import { ToastService } from '../toast.service';

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
  plan: string = '';

  public options: Object = {
    placeholderText: 'Edit Your Content Here!',
    heightMin: 500,
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

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    private route: ActivatedRoute,
    public toastService: ToastService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.eventId = params.get('eventId');

      this.http.get<any>(this.baseUrl + 'api/events/base-data/' + this.eventId).subscribe(result => {
        this.event = result;

        this.http.get<any>(this.baseUrl + `api/events/${this.eventId}/plan`).subscribe(result => {
          this.plan = result.plan;
        });

      });
    });
  }

  // PLAN
  onPlanSubmit(plan: string) {
    this.http.put<string>(this.baseUrl + `api/events/${this.eventId}/plan`, { plan: plan }).subscribe(result => {
      this.toastService.show(`Program updated successfuly`, { classname: 'bg-success text-light' });
    });
  }
}
