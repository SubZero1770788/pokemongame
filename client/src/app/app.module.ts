import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { NavbarComponent } from './navbar/navbar.component';
import { WildComponent } from './wilderness/wild/wild.component';
import { ShopComponent } from './shop/shop.component';
import { RankingComponent } from './ranking/ranking.component';
import { PokedexComponent } from './pokedex/pokedex.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { WildadventureComponent } from './wilderness/wildadventure/wildadventure.component';
import { RegisterComponentComponent } from './register/register.component';
import { BackpackComponent } from './wilderness/backpack/backpack.component';
import { PokemonTeamComponent } from './wilderness/pokemon-team/pokemon-team.component';
import { BattleComponent } from './wilderness/battle/battle.component';
import { HealingComponent } from './wilderness/healing/healing.component';
import { jwtInterceptor } from './interceptors/jwt.interceptor';
import { PokemonsComponent } from './pokemons/pokemons.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    WildComponent,
    ShopComponent,
    RankingComponent,
    PokedexComponent,
    WildadventureComponent,
    RegisterComponentComponent,
    BackpackComponent,
    PokemonTeamComponent,
    BattleComponent,
    HealingComponent,
    PokemonsComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: jwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
