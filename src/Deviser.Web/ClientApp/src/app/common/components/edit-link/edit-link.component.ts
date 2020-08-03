import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Link } from '../../domain-types/link';
import { LinkType } from '../../domain-types/link-type';

@Component({
  selector: 'app-edit-link',
  templateUrl: './edit-link.component.html',
  styleUrls: ['./edit-link.component.scss']
})
export class EditLinkComponent implements OnInit {

  @Output() linkChanged: EventEmitter<Link> = new EventEmitter();

  selectedTab: string;
  linkType = LinkType;
  set link(value: Link) {
    this._link = value
    // this.init();
  }

  get link(): Link {
    return this._link;
  }

  private _link: Link;

  constructor(public bsModalRef: BsModalRef,) { }

  ngOnInit(): void {
  }

  cancel() {
    this.bsModalRef.hide();
  }

  selectLink() {    
    this.linkChanged.emit(this.link);
    this.bsModalRef.hide();
  }

}
