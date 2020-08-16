import { Component, OnInit, Input } from '@angular/core';
import { Alert } from '../../domain-types/alert';

@Component({
  selector: 'dev-alert',
  templateUrl: './dev-alert.component.html',
  styleUrls: ['./dev-alert.component.scss']
})
export class DevAlertComponent implements OnInit {

  @Input() alerts: Alert[];
  
  constructor() { }

  ngOnInit(): void {
  }

}
