import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GameDataService } from './services/game-data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';

  constructor(private gameDataService: GameDataService, private router: Router) { }

  async ngOnInit() {
    await this.gameDataService.fetchAsync();
    this.gameDataService.initGameState();

    this.router.navigate(["quiz"]);
  }

  delay(time: number) {
    return new Promise(resolve => setTimeout(resolve, time));
  }
}
