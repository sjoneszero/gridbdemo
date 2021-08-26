import { BackendService } from './../../services/backend.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart } from 'chart.js'
import * as _ from 'underscore';
import * as moment from 'moment';
@Component({
  selector: 'app-analysis',
  templateUrl: './analysis.component.html'
})
export class AnalysisComponent implements OnInit {
  CURRENCY: string = '€'
  id: string
  chartData: any []
  responseData:  any
  loading: boolean;
  error: boolean;
  constructor(private route: ActivatedRoute, private backendService: BackendService) { }

  loadData() {
    this.loading = true;
    this.backendService.getAnalysisData(+this.id).subscribe(
      (response: any) => {
          console.log(response.body)
          this.responseData = response.body;
          this.chartData = this.responseData.MarketPrices.map((elem: any) => {
            return{
              x: moment(elem.TimeStamp),
              y: elem.Price
            };
          }
        );
        this.loading = false;
        this.populateChart();

        },
      (error: any) => {
        this.loading = false;
        this.error = true;
      }
    );
  }

  populateChart(): void {

    var canvas = <HTMLCanvasElement>document.getElementById("marketDataChart");
    var ctx = canvas.getContext("2d");
    let chartData = {
      datasets: [{
          data: this.chartData,
          lineTension : 0.0,
          fill: false,
          labels: true,
          borderColor: '#4BC0C0',
          tension: 1,
          label: 'Market Price',
          pointRadius: 0,
          pointHoverRadius: 15,
          pointHitRadius: 15,
          borderWidth: 2
      }]
  };

  var chart = new Chart(ctx, {
    type: 'line',
    data: chartData,
    options: {
        scales: {
          xAxes: [{
              type: 'time',
              time: {
                  unit: 'month',
                  displayFormats: {
                    month: 'MMM yyyy'
                },
                tooltipFormat: 'DD MMM:YY HH:mm',

              }
          }],
          yAxes: [{
            ticks: {
              callback: (value: string) => {
                  return this.addCurrency(value.toString());
              }
            }
          }]
        }
      }
    });
  }
  reformatDate(dateString: any) {
   return moment(dateString).format('DD MMM YYYY HH:mm');
  }
  reformatDateWithHourWindow(dateString: any) {
    var oneHourLater = moment(dateString).add(1, 'hours');
    return moment(dateString).format('DD MMM YYYY HH:mm')
      + "-"
      + moment(oneHourLater).format('HH:mm');
   }

  addCurrency(value: string) : string {
    return this.CURRENCY + value;
  }

  ngOnInit(): void {
    this.error = false;
    this.id = this.route.snapshot.paramMap.get('id');
    this.loadData();
  }
}
