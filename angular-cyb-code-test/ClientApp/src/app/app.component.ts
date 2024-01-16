import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GameController } from './controllers/game.controller';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';

  constructor(private gameController: GameController, private router: Router) { }

  async ngOnInit() {
    await this.gameController.fetchAsync();
    this.gameController.initGameState();

    this.router.navigate(["quiz"]);
  }

  delay(time: number) {
    return new Promise(resolve => setTimeout(resolve, time));
  }
}
