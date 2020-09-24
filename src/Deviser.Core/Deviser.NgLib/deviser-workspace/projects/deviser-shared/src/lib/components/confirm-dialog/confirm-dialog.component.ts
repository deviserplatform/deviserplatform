import { Component, OnInit, Input, Output, EventEmitter, TemplateRef, ViewChild, ElementRef } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent implements OnInit {

  @Input() title: string;
  @Input() content: string;
  @Input() btnConfirmText: string;
  @Input() btnCancelText: string;

  @ViewChild('modelTemplate')
  private modelTemplate: TemplateRef<any>;

  @Output() confirm = new EventEmitter<any>();
  @Output() cancel = new EventEmitter<any>();

  private modalRef: BsModalRef;
  private data: any;

  constructor(private modalService: BsModalService) { }

  ngOnInit() {
  }

  onCancel(): void {
    this.cancel.emit(this.data);
    this.modalRef.hide();
  }

  onConfirm(): void {
    this.confirm.emit(this.data);
    this.modalRef.hide();
  }

  openModal(data: any) {
    this.data = data;
    this.modalRef = this.modalService.show(this.modelTemplate);
  }

}
