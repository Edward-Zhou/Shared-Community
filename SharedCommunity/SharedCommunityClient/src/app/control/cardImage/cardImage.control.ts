import { Component, Input} from "@angular/core";
import { MaterialImportModule} from '../../shared/materialImportModule';
import { CardImageModel} from '../../model/cardImageModel';

@Component({
    selector:'cardImage',
    styleUrls:['./cardImage.control.css'],
    templateUrl:'./cardImage.control.html'
})

export class CardImageControl{
    @Input() cardImage : CardImageModel
}