import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { QuestionComponent } from './components/question/question.component';
import { SubmitComponent } from './components/submit/submit.component';
import { ResultsComponent } from './components/results/results.component';
import { Environment } from './models/environment.model';
import { QuizComponent } from './components/quiz/quiz.component';
import { SplashComponent } from './components/splash/splash.component';

@NgModule({
  declarations: [
    AppComponent,
    QuestionComponent,
    SubmitComponent,
    ResultsComponent,
    QuizComponent,
    SplashComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: SplashComponent },
      { path: 'quiz', component: QuizComponent },
      { path: 'submit', component: SubmitComponent },
      { path: 'results', component: ResultsComponent },
    ])
  ],
  providers: [Environment],
  bootstrap: [AppComponent]
})
export class AppModule { }
