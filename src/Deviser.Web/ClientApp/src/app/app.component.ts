import { Component, ElementRef } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  title = 'ClientApp';
  private _startView: string;
  constructor(private _elementRef: ElementRef,
    private _router: Router) {
    this._startView = this._elementRef.nativeElement.getAttribute('initial-view');
    console.log(this._startView);
    this._router.navigateByUrl(this._startView);
  }
}
