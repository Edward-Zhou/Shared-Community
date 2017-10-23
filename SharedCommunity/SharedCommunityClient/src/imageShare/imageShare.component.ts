import { Component} from "@angular/core";
import { MaterialImportModule} from 'common/materialImportModule';
import { CardImageModel } from "models/cardImageModel";

@Component({
    selector:'imageShare',
    templateUrl:'imageShare.component.html'
})

export class ImageShareComponent{
    cardImage: CardImageModel = {
        id:'I1',
        title:'',
        subtitle:''
    }    
}