import { Component, Input } from '@angular/core';
import { ISuperGame } from '../../../Interfaces/SuperGame';
import { MiniGameComponent } from '../mini-game/mini-game.component';

@Component({
  selector: 'app-super-game',
  standalone: true,
  imports: [MiniGameComponent],
  templateUrl: './super-game.component.html',
  styleUrl: './super-game.component.scss'
})
export class SuperGameComponent {
	@Input() superGame: ISuperGame | null = null;
}
