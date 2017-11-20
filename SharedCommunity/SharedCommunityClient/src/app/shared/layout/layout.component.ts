import { Component, OnInit } from '@angular/core'
import { href } from '../../model/href'
import { AuthService, emitter } from '../../auth/auth.service'
declare var jQuery: any;
declare var Core: any;
declare var Demo: any;

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit {

  breadlist: any[];
  constructor(public auth: AuthService) {
    emitter.subscribe(result => {
      this.aaa(result)
    })
  }
  ngOnInit() {
  }
  aaa(result) {
    href.forEach(item => {
      let ex = new RegExp(item.key)
      if (ex.test(window.location.pathname)) {
        let tmp = item.value.slice(0);
        if (result) {
          tmp.push(result)
        }
        this.breadlist = tmp;
      }
    })
  }
  ngAfterViewInit() {
    jQuery(document).ready(() => {
      jQuery('body').removeClass().addClass('md-skin gallery-page pace-running sb-l-o sb-r-c onload-check pace-done')

      // Init Theme Core      
      //Core.init();

      // Init Demo JS
      //Demo.init();
    })
  }

}
