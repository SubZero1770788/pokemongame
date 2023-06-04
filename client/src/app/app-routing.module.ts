import { NgModule } from '@angular/core';
import {RouterModule, Routes } from '@angular/router';
import { WildComponent } from './wilderness/wild/wild.component';
import { AppComponent } from './app.component';
import { ShopComponent } from './shop/shop.component';
import { RankingComponent } from './ranking/ranking.component';
import { PokedexComponent } from './pokedex/pokedex.component';
import { RegisterComponentComponent } from './register/register.component';
import { AuthGuard } from './guards/auth.guard';
import { BattleExitGuard } from './guards/battle-exit.guard';
import { PokemonsComponent } from './pokemons/pokemons.component';

const routes: Routes = [
  {path: '', component: AppComponent},
  {path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children:[
      {path: 'wild', component: WildComponent, canDeactivate: [BattleExitGuard]},
      {path: 'items', component: ShopComponent},
      {path: 'ranking', component: RankingComponent},
      {path: 'pokedex', component: PokedexComponent},
    ]
  },
  {path: 'register', component: RegisterComponentComponent},
  {path: 'pokemons', component: PokemonsComponent, runGuardsAndResolvers: 'always', canActivate: [AuthGuard]},
]

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
