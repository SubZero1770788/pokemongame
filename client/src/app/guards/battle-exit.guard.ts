import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { WildComponent } from '../wilderness/wild/wild.component';

@Injectable({
  providedIn: 'root'
})
export class BattleExitGuard implements CanDeactivate<WildComponent> {
  canDeactivate(component: WildComponent): boolean {
    if ((component.battleMode == 2 || component.battleMode == 3 && component.previousMode == 2) && (component.currentBattle?.battle == true || component.healing?.currentItem || component.previousMode )) {
      return confirm("Do you want to exit battle? You won't be able to enter it again")
    }
    if ((component.currentBattle?.battle == false && component.currentBattle.message?.attackName != null) && component.currentBattle?.newAttack == null) {
      return confirm("If you exit now your pokemon won't learn the attack")
    }
    return true;
  }
}
