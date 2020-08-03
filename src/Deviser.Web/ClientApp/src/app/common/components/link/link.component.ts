import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Link } from '../../domain-types/link';
import { ContentTypeField } from '../../domain-types/content-type-field';
import { Property } from '../../domain-types/property';
import { ModalOptions, BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EditLinkComponent } from '../edit-link/edit-link.component';
import { EditService } from '../../services/edit.service';
import { LinkType } from '../../domain-types/link-type';

@Component({
  selector: 'app-link',
  templateUrl: './link.component.html',
  styleUrls: ['./link.component.scss']
})
export class LinkComponent implements OnInit {

  @Input() set link(value: Link) {
    this._link = value
    this.init();
  }
  @Input() field: ContentTypeField;
  @Input() properties: Property[]

  @Output() linkChanged: EventEmitter<Link> = new EventEmitter();

  get link(): Link {
    return this._link;
  }

  bsModalRef: BsModalRef;

  private _link: Link;
  private _modalConfig: any = {
    ignoreBackdropClick: true
  }

  constructor(private _editService: EditService,
    private _modalService: BsModalService,) { }

  ngOnInit(): void {
  }

  showPopup() {
    let param: ModalOptions = JSON.parse(JSON.stringify(this._modalConfig));
    let linkChanged: EventEmitter<Link>;
    param.class = 'link-selector-modal';
    param.initialState = {
      link: this.link
    }
    if (this.bsModalRef && this.bsModalRef.content) {
      linkChanged = this.bsModalRef.content.linkChanged as EventEmitter<Link>;
      linkChanged.unsubscribe();
    }

    this.bsModalRef = this._modalService.show(EditLinkComponent, param), this._modalConfig;
    linkChanged = this.bsModalRef.content.linkChanged as EventEmitter<Link>;
    linkChanged.subscribe(image => this.onLinkChanged(image));
    this.bsModalRef.content.closeBtnName = 'Close';
  }

  onLinkChanged(link: Link) {
    this.link = link
    this.linkChanged.emit(this.link);
  }

  removeLink() {
    this.link.linkText = null;
    this.link.pageId= null;
    this.link.url = null;
    this.link.isNewWindow = false;
    this.link.linkType = LinkType.Url;
  }

  getLinkUrl() {
    return this._editService.getLinkUrl(this.link);
  }

  private init() {
  }

}
