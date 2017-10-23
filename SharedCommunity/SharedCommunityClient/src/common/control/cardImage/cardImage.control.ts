import { Component, Input} from "@angular/core";
import { MaterialImportModule} from 'common/materialImportModule';
import { CardImageModel} from 'models/cardImageModel';

@Component({
    selector:'cardImage',
    styleUrls:['./cardImage.control.css'],
    templateUrl:'./cardImage.control.html'
})

export class CardImageControl{
    @Input() cardImage : CardImageModel
}