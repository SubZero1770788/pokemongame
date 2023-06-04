import { AfterViewInit, Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { pokemon } from '../../TypescriptTypes/Pokemon';
import { WildBattlerService } from '../../services/wild-battler.service';
import { chosenAttack } from '../../TypescriptTypes/chosenAttack';
import { catchString } from 'src/app/TypescriptTypes/catchString';
import { PokemonsService } from 'src/app/services/pokemons.service';
import { ColorService } from 'src/app/services/color.service';

@Component({
  selector: 'app-battle',
  templateUrl: './battle.component.html',
  styleUrls: ['./battle.component.css']
})
export class BattleComponent implements OnInit, OnDestroy, AfterViewInit {
  battlePokemons: Array<pokemon> | null = null;
  enemyCurrentHp: number | null = null;
  enemyHpDifference: number = 1;
  enemyMaxHp: number = 0;
  userMaxHp: number = 0;
  userCurrentHp: number | null = null;
  userHpDifferenece: number = 1;
  hpSet: boolean = false;
  battle: boolean = false;
  message: catchString | null = null;
  newAttack: catchString | null = null;
  color: string = 'rgb(155, 220, 155)';
  attack1color: string = 'rgb(155, 220, 155)';
  attack2color: string = 'rgb(155, 220, 155)';
  attack3color: string = 'rgb(155, 220, 155)';
  attack4color: string = 'rgb(155, 220, 155)';
  typeUser1color: string = 'rgb(155, 220, 155)';
  typeUser2color: string = 'rgb(155, 220, 155)';
  typeEntity1color: string = 'rgb(155, 220, 155)';
  typeEntity2color: string = 'rgb(155, 220, 155)';
  @Output() returnable = new EventEmitter();

  constructor(public _wildS: WildBattlerService, public _pokS: PokemonsService, public _colS: ColorService) { }

  ngOnDestroy(): void {
    this._pokS.health[0] = this.enemyCurrentHp!;
    this._pokS.health[1] = this.userCurrentHp!;
  }

  ngAfterViewInit(): void {
    this.setDifferences();
  }

  ngOnInit(): void {
    this._pokS.outputter.subscribe(
      cs => {
        this.message = cs;
        this.battle = false;
        this.color = this._colS.getMessageColor(this.message.message, this.color);
      }
    )
    this.battleSetter();
  }



  battleSetter()
  {
    this.battle = true;
    this.battlePokemons = this._wildS.pokemonBattle;
    if (!this.hpSet) {
      this.enemyMaxHp = this.battlePokemons![0].hp;
      this.userMaxHp = this.battlePokemons![1].hp;
      if(this._pokS.health[0] != 0)
      {
        this.enemyCurrentHp = this._pokS.health[0];
        this.userCurrentHp = this._pokS.health[1];
      }
      else
      {
        this.enemyCurrentHp = this.battlePokemons![0].hp;
        this.userCurrentHp = this.battlePokemons![1].hp;
      }
    }
    this.hpSet = true;
    this.attack1color = this._colS.getBackgroundColor(this.battlePokemons![1].attack1Type!, this.attack1color);
    this.attack2color = this._colS.getBackgroundColor(this.battlePokemons![1].attack2Type!, this.attack2color);
    this.attack3color = this._colS.getBackgroundColor(this.battlePokemons![1].attack3Type!, this.attack3color);
    this.attack4color = this._colS.getBackgroundColor(this.battlePokemons![1].attack4Type!, this.attack4color);
    this.typeUser1color = this._colS.getBackgroundColor(this.battlePokemons![1].pokemonType1Name!, this.typeUser1color);
    this.typeEntity1color = this._colS.getBackgroundColor(this.battlePokemons![0].pokemonType1Name!, this.typeEntity1color);
    if (this.battlePokemons![0].pokemonType2Name) {
      this.typeEntity2color = this._colS.getBackgroundColor(this.battlePokemons![0].pokemonType2Name, this.typeEntity2color);
    }
    if (this.battlePokemons![1].pokemonType2Name) {
      this.typeUser2color = this._colS.getBackgroundColor(this.battlePokemons![1].pokemonType2Name!, this.typeEntity2color);
    }
  }

  return() {
    this.newAttack = null;
    this.returnable.emit(0);
  }

  continue() {
    this.newAttack = null;
    this.returnable.emit(4);
  }

  chooseAttack(n: number) {
    let thischosenAttack: chosenAttack =
    {
      chosenAttackId: n,
      pokemonId: this.battlePokemons![1].id!,
    };
    this._wildS.chooseAttack(thischosenAttack).subscribe(
      response => {
        this.message = response;
        this.userCurrentHp = this.message!.userCurrentHp!;
        this.enemyCurrentHp = this.message!.enemyCurrentHp!;
        this.setDifferences()
        if (this.message.message == "Defeat!!" || this.message.message == "Victory!!") {
          this.color = this._colS.getMessageColor(this.message.message, this.color);
          this.battle = false;
        }
      }
    )
    this.newAttack = null;
  }

  replaceAttack(n: number) {
    let thischosenAttack: chosenAttack =
    {
      chosenAttackId: n,
      pokemonId: this.battlePokemons![1].id!,
    };
    this._wildS.changeAttack(thischosenAttack).subscribe(
      r => {
        this.newAttack = r;
      }
    )
  }

  setDifferences() {
    this.userHpDifferenece = this.userCurrentHp! / this.userMaxHp;
    this.enemyHpDifference = this.enemyCurrentHp! / this.enemyMaxHp;
    var hpenemy = document.getElementById('hpEnemyNumber');
    hpenemy!.style.width = this.enemyHpDifference * 100 + '%';
    var hpuser = document.getElementById('hpUserNumber');
    hpuser!.style.width = this.userHpDifferenece * 100 + '%';
  }

  public getBattle() {
    return this.battle;
  }

}
