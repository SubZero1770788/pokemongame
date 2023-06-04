import { Injectable } from '@angular/core';
import { PokemonsService } from './pokemons.service';

@Injectable({
  providedIn: 'root'
})
export class ColorService {

  constructor() { }

  getMessageColor(message: string, color: string)
  {
    switch(message)
    {
      case "You did it !!":
        color = 'rgb(155, 220, 155)';
      break;
      case "Your pokemon was healed !!":
        color = 'rgb(155, 220, 155)';
      break;
      case "Victory!!":
        color = 'rgb(155, 220, 155)';
      break;
      default:
        color = 'rgb(220, 155, 155)';
      break;
    }

    return color;
  }

  getBackgroundColor(type: string, color: string) {
    switch (type) {
      case 'Ground':
        color = "rgb(250,200,100)";
        break;
      case 'Water':
        color = "rgb(160,160,250)";
        break;
      case 'Fire':
        color = "rgb(250,160,160)";
        break;
      case 'Grass':
        color = "rgb(160,250,160)";
        break;
      case 'Flying':
        color = "rgb(190,190,225)";
        break;
      case 'Dragon':
        color = "rgb(180,180,220)";
        break;
      case 'Electric':
        color = "rgb(220,220,80)";
        break;
      case 'Steel':
        color = "rgb(180,180,180)";
        break;
      case 'Ghost':
        color = "rgb(110,50,190)";
        break;
      case 'Dark':
        color = "rgb(80,80,80)";
        break;
      case 'Fighting':
        color = "rgb(200,140,140)";
        break;
      case 'Poison':
        color = "rgb(220,160,220)";
        break;
      case 'Psychic':
        color = "rgb(220,160,180)";
        break;
      case 'Fairy':
        color = "rgb(240,160,200)";
        break;
      case 'Ice':
        color = "rgb(120,200,240)";
        break;
      case 'Bug':
        color = "rgb(120,200,180)";
        break;
      case 'Normal':
        color = "rgb(180,180,150)";
        break;
      case 'Rock':
        color = "rgb(150,150,120)";
        break;
    }
    return color;
  }
}

