import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Question } from '../../models/question.model';
import { GameDataService } from '../../services/game-data.service';

@Component({
  selector: 'app-submit',
  templateUrl: './submit.component.html',
  styleUrls: ['./submit.component.css']
})
export class SubmitComponent {

  questions!: Question[];
  hasUnansweredQuestions!: boolean;
  firstMissingAnswerIndex!: number | undefined;
  pageTitle!: string;

  constructor(private gameDataService: GameDataService, private router: Router) { }

  async ngOnInit(): Promise<void> {
    this.questions = this.gameDataService.questions;
    this.hasUnansweredQuestions = false;

    if (this.questions === undefined) {
      this.router.navigate([""]);
      return;
    }

    //Checking if all questions have been answered
    this.questions.forEach((question, index) => {
      if (question.selectedAnswer === null && this.firstMissingAnswerIndex === undefined) {
        this.firstMissingAnswerIndex = index;
        this.hasUnansweredQuestions = true;
        return;
      }
    });

    if (this.firstMissingAnswerIndex === undefined) {
      this.pageTitle = "Ready to submit your answers?";
    } else {
      this.pageTitle = "You've not answered all the questions."
    }
  }

  goToQuestion(index: number) {
    this.gameDataService.setCurrentQuestion(index);
    this.router.navigate(["quiz"]);
  }

  goToUnansweredQuestion() {
    if (this.firstMissingAnswerIndex === undefined) {
      return;
    }

    this.gameDataService.setCurrentQuestion(this.firstMissingAnswerIndex as number);
    this.router.navigate(["quiz"]);
  }

  async submitAnswersAsync() {
    await this.gameDataService.submitAnswers();
    this.router.navigate(["results"]);
  }
}
