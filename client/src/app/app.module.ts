import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { UploadmodalComponent } from './components/uploadmodal/uploadmodal.component';
import { DatasetsComponent } from './components/datasets/datasets.component';
import { AnalysisComponent } from './components/analysis/analysis.component';


@NgModule({
  declarations: [
    AppComponent,
    UploadmodalComponent,
    DatasetsComponent,
    AnalysisComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
