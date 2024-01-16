import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Environment } from "../models/environment.model";
import { Question } from "../models/question.model";
import { Result } from "../models/result.model";

@Injectable({ providedIn: 'root' })
export class GameController {

  questions!: Question[];
  currentQuestionIndex!: number;
  currentQuestion!: Question;
  results!: Result[];

  constructor(private environment: Environment, private httpClient: HttpClient) { }

  async fetchAsync(): Promise<Question[]> {
    const hostName = this.environment.getHostname();
    const getGameDataRequest = this.httpClient.get<Question[]>(`${hostName}/api/fetchGameData`).toPromise();

    let questions!: Question[];
    await getGameDataRequest.then((response: any) => {
      questions = response;
    });

    this.questions = questions;

    return questions;
  }

  initGameState() {
    this.currentQuestionIndex = 0;
    this.currentQuestion = this.questions[0];
  }

  setCurrentQuestion(index: number) {
    this.currentQuestionIndex = index;
    this.currentQuestion = this.questions[index];
  }

  async submitAnswers() {
    const hostName = this.environment.getHostname();
    const submitAnswersRequest = this.httpClient.post<Result[]>(`${hostName}/api/submitAnswers`, this.questions).toPromise();

    let results!: Result[];
    await submitAnswersRequest.then((response: any) => {
      results = response;
    });

    this.results = results;
  }
}
