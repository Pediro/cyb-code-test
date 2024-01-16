import { Component, Input } from '@angular/core';
import { Question } from '../../models/question.model';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent {

  @Input() question!: Question;
  @Input() saveAnswer!: (arg: any) => void;

  constructor() { }

  async ngOnInit(): Promise<void> {
    
  }

}
