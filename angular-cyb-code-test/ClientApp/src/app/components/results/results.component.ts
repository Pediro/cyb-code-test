import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Result } from '../../models/result.model';
import { GameDataService } from '../../services/game-data.service';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.css']
})
export class ResultsComponent {

  results!: Result[];
  pageTitle!: string;

  constructor(private gameDataService: GameDataService, private router: Router) { }

  async ngOnInit(): Promise<void> {

    if (this.gameDataService.questions === undefined) {
      this.router.navigate([""]);
      return;
    }

    if (this.gameDataService.results === undefined) {
      return;
    }

    this.results = this.gameDataService.results;

    let correctCount = 0;
    this.results.forEach((result) => {
      if (result.isCorrectAnswer) {
        correctCount++;
      }
    });

    this.pageTitle = `You got ${correctCount} of the questions correct.`;
  }

  restartQuiz() {
    window.location.href = "";
  }
}
