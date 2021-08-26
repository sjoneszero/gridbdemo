import { DatasetsComponent } from './components/datasets/datasets.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnalysisComponent } from './components/analysis/analysis.component';

const routes: Routes = [
  { path: '', redirectTo: 'datasets', pathMatch: 'full'},
  { path: 'datasets', component: DatasetsComponent },
  { path: 'datasets/:id', component: AnalysisComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
