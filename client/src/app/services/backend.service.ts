import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from  '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BackendService {
  status: number
	constructor(private httpClient : HttpClient) {  }

  SERVER_URL: string = "https://localhost:5001/";
  public uploadCsv(formData: FormData) :  Observable<HttpResponse<any>> {

    return this.httpClient.post<HttpResponse<any>>(this.SERVER_URL + 'api/marketprices', formData, {
        observe: 'response'
      })
  }

  public getDataSets() :  Observable<HttpResponse<any>> {

    return this.httpClient.get<HttpResponse<any>>(this.SERVER_URL + 'api/marketprices', {
        observe: 'response'
      })
  }

  public getAnalysisData(id: number) :  Observable<HttpResponse<any>> {

    return this.httpClient.get<HttpResponse<any>>(this.SERVER_URL + 'api/marketprices/' + id + '/analysis', {
        observe: 'response',
      })
  }

  public deleteDataSet(id: number) :  Observable<HttpResponse<any>> {

    return this.httpClient.delete<HttpResponse<any>>(this.SERVER_URL + 'api/marketprices/' + id, {
        observe: 'response'
      })
  }
}
