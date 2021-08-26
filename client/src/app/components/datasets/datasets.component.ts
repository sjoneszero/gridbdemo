import { BackendService } from './../../services/backend.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as moment from 'moment';
import * as _ from 'underscore';
@Component({
  selector: 'app-datasets',
  templateUrl: './datasets.component.html'
})
export class DatasetsComponent implements OnInit {

  router: Router;
  datasets: any [];
  loading : boolean;
  constructor(private backendService: BackendService) { }

  getDatasets() {
    this.loading = true;
    this.backendService.getDataSets().subscribe(
      (response: any) => {
          this.datasets = response.body;
      });
      this.loading = false
  }
  deleteDataSet (id : number) {
    this.backendService.deleteDataSet(id).subscribe(
      (response: any) => {
          this.getDatasets();
      });
  }
  reformatDate(dateString: any) {
    return moment(dateString).format('DD MMM YYYY HH:mm');
  }

  ngOnInit(): void {
    this.getDatasets()
  }
}
