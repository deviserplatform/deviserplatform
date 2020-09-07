import { Directive, HostListener, Output, HostBinding, Renderer2, ElementRef, EventEmitter } from '@angular/core';


@Directive({
  selector: '[dndUpload]'
})
export class DndUploadDirective {

  @Output() fileDropped = new EventEmitter<FileList>();
  
  constructor(private renderer: Renderer2,
    private elementRef: ElementRef,) { }

  //Dragover listener
  @HostListener('dragover', ['$event']) onDragOver(evt) {
    evt.preventDefault();
    evt.stopPropagation();
    this.renderer.removeClass(this.elementRef.nativeElement, 'dnd-upload-dropped');
    this.renderer.addClass(this.elementRef.nativeElement, 'dnd-upload-drag-over')
  }

  //Dragleave listener
  @HostListener('dragleave', ['$event']) public onDragLeave(evt) {
    evt.preventDefault();
    evt.stopPropagation();
    this.renderer.removeClass(this.elementRef.nativeElement, 'dnd-upload-dropped');
    this.renderer.removeClass(this.elementRef.nativeElement, 'dnd-upload-drag-over')
  }

  //Drop listener
  @HostListener('drop', ['$event']) public ondrop(evt) {
    evt.preventDefault();
    evt.stopPropagation();
    this.renderer.addClass(this.elementRef.nativeElement, 'dnd-upload-dropped');
    let files = evt.dataTransfer.files;
    if (files.length > 0) {
      this.fileDropped.emit(files)
    }
  }

}
