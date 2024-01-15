import { Component, OnInit } from '@angular/core';
import { Question } from '../../models/question.model';
import { GameDataService } from '../../services/game-data.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-quiz',
  templateUrl: './quiz.component.html',
  styleUrls: ['./quiz.component.css']
})
export class QuizComponent implements OnInit {

  questions!: Question[];
  currentQuestionIndex!: number;
  currentQuestion!: Question;
  pageTitle!: string;

  constructor(private gameDataService: GameDataService, private router: Router) { }

  async ngOnInit(): Promise<void> {

    if (this.gameDataService.questions === undefined) {
      this.router.navigate([""]);
      return;
    }

    this.questions = this.gameDataService.questions;
    this.currentQuestionIndex = this.gameDataService.currentQuestionIndex;
    this.currentQuestion = this.gameDataService.currentQuestion;
    this.initPageTitle();
  }

  initPageTitle() {
    this.pageTitle = "Which film or tv show has " + this.currentQuestion.characterName + " been in?";
  }

  saveAnswer(selectedAnswer: string) {
    this.currentQuestion.selectedAnswer = selectedAnswer;
    this.goToNextQuestion();
  }

  goToNextQuestion() {
    this.currentQuestionIndex++;

    if (this.currentQuestionIndex > this.questions.length - 1) {
      this.router.navigate(["submit"]);
      return;
    }

    // Only go to questions that have not been answered, this allows us to do a skip function on the submit page
    if (this.questions[this.currentQuestionIndex].selectedAnswer === null) {
      this.currentQuestion = this.questions[this.currentQuestionIndex];
      this.initPageTitle();
      return;
    }

    this.goToNextQuestion();
  }

  goToPreviousQuestion() {
    this.currentQuestionIndex--;

    if (this.currentQuestionIndex < 0) {
      this.currentQuestionIndex = 0;
    }

    this.currentQuestion = this.questions[this.currentQuestionIndex];
    this.initPageTitle();
  }

}
