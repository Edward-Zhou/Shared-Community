import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes, UrlSerializer} from '@angular/router';

import { AppComponent } from './app.component';
import { ImageShareComponent} from 'imageShare/imageShare.component';
import { FunnyShareComponent} from 'funnyShare/funnyShare.component';
import { NavMenuComponent} from 'navmenu/navmenu.component';
import { SlideMenuComponent } from 'slidemenu/slidemenu.component';
import {ContentBodyComponent} from 'contentbody/contentbody.component';

//command method
import { LowerCaseUrlSerializer} from 'common/lowerCaseUrlSerializer';
import { MaterialImportModule} from 'common/materialImportModule';

//control
import { CardImageControl} from 'common/control/cardImage/cardImage.control';


const appRoutes: Routes = [
  { path: 'imageshare', component:ImageShareComponent},
  { path: 'funnyshare', component:FunnyShareComponent},
  { path: '', redirectTo:'/imageshare', pathMatch: 'full'}
]

@NgModule({
  declarations: [
    AppComponent,
    ImageShareComponent,
    FunnyShareComponent,
    NavMenuComponent,
    SlideMenuComponent,
    ContentBodyComponent,
    //control
    CardImageControl
  ],
  imports: [
    RouterModule.forRoot(
      appRoutes,
      {enableTracing : true}
    ),
    BrowserModule,
    MaterialImportModule,
    FormsModule,
    HttpModule
  ],
  providers: [{
    provide:UrlSerializer,
    useClass:LowerCaseUrlSerializer
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
