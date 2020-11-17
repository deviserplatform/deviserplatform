import { Component, OnInit, Input, SimpleChanges, OnChanges, EventEmitter, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators, ValidatorFn, AsyncValidatorFn } from '@angular/forms';
import { Field } from '../common/domain-types/field';
import { FieldType } from '../common/domain-types/field-type';
import { repeat, pairwise, startWith, map } from 'rxjs/operators';
import { Observable, BehaviorSubject, Subscription } from 'rxjs';
import { ValidationType } from '../common/domain-types/validation-type';
import { EmailExistValidator } from '../common/validators/async-email-exist.validator';
import { PasswordValidator } from '../common/validators/async-password.validator';
import { UserExistValidator } from '../common/validators/async-user-exist.validator';
import { LookUpDictionary } from '../common/domain-types/look-up-dictionary';

import { RelatedField } from '../common/domain-types/related-field';
import { RelationType } from '../common/domain-types/relation-type';
import { CustomValidator } from '../common/validators/async-custom.validator';
import { FormType } from '../common/domain-types/form-type';
import { FormControlService } from '../common/services/form-control.service';
import { AdminService } from '../common/services/admin.service';
import { FieldOption } from '../common/domain-types/field-option';
import { CheckBoxMatrix } from '../common/domain-types/checkbox-matrix';
import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap/modal';
import { Image, Link, EditLinkComponent, LinkType, PageService, Page, WINDOW, PageContext, ImageSelectorComponent } from 'deviser-shared';
import { FormResult } from '../common/domain-types/form-result';

var formControlComponent: FormControlComponent;

@Component({
  selector: 'app-form-control',
  templateUrl: './form-control.component.html',
  styleUrls: ['./form-control.component.scss']
})
export class FormControlComponent implements OnInit {
  // @Input() entityType: string;
  @Input() field: Field;
  @Input() form: FormGroup;
  @Input() isShown: boolean;
  @Input() isDisabled: boolean;
  @Input() isValidate: boolean;
  // @Input() keyFields: Field[];
  @Input() lookUps: LookUpDictionary;
  @Input() formType: FormType;
  @Input() formName: string;
  @Input() formItemId: string;

  //To access FieldType enum
  fieldType = FieldType;
  bsModalRef: BsModalRef;
  private _lookUpDataSubject = new BehaviorSubject<any[]>([]);
  lookUpData$ = this._lookUpDataSubject.asObservable();

  rowLookUp: any[];
  colLookUp: any[];
  colLookUpKey: string;
  checkBoxMatrix: CheckBoxMatrix;
  modules: any = {
    toolbar: {
      container: [
        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
        ['bold', 'italic', 'underline', 'strike'],
        ['blockquote', 'code-block'],
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'indent': '-1' }, { 'indent': '+1' }],
        [{ 'align': [] }],
        ['clean'],
        ['link', 'image']
      ],
      handlers: {
        'link': function (value) {
          formControlComponent.onEditorLinkClicked(this.quill, value);
        },
        'image': function (callback) {
          formControlComponent.showImagePopup(this.quill);
        }
      }
    }
  }
  pageContext: PageContext;
  rowLookUpKey: string;
  // _lookUpData: any[];

  get bsConfig(): any {
    return {
      dateInputFormat: this.field.fieldOption.format.replace(/y/g, 'Y').replace(/d/g, 'D')
    };
  }



  private _modalConfig: any = {
    ignoreBackdropClick: true
  }
  private _valChangeSubscription: Subscription;

  constructor(private _emailExistValidator: EmailExistValidator,
    private _adminService: AdminService,
    private _customValidator: CustomValidator,
    private _formControlService: FormControlService,
    private _modalService: BsModalService,
    private _pageService: PageService,
    private _passwordValidator: PasswordValidator,
    private _userExistValidator: UserExistValidator,
    @Inject(WINDOW) private _window: any) {
    // this._lookUpData = [];
    this.pageContext = _window.pageContext;
    formControlComponent = this;
  }

  ngOnInit() {
    this.init();
  }

  ngOnDestroy() {
    if (this._valChangeSubscription) {
      this._valChangeSubscription.unsubscribe();
    }
  }



  ngOnChanges(changes: SimpleChanges) {

    // if (changes.isValidate && changes.isValidate.currentValue != null && changes.isValidate.currentValue !== "") {
    //   //Dynamic validation can be changed only when this field is not required by backend
    //   if (!this.field.fieldOption.isRequired || this.field.fieldOption.validationType) {
    //     this.onIsValidateChange(changes.isValidate.currentValue);
    //   }
    // }

    // if (this.field.fieldOption.hasLookupFilter) {

    // }
  }

  onEditorLinkClicked(quill: any, value: any) {
    console.log('from angular component!');
    if (value) {
      // let href = prompt('Enter the URL');
      // quill.format('link', href);
      this.showLinkPopup(quill);
    } else {
      quill.format('link', false);
    }
  }

  showLinkPopup(quill: any) {
    let param: ModalOptions = JSON.parse(JSON.stringify(this._modalConfig));
    let linkChanged: EventEmitter<Link>;
    param.class = 'link-selector-modal';
    param.initialState = {
      link: {}
    }
    if (this.bsModalRef && this.bsModalRef.content && this.bsModalRef.content.linkChanged) {
      linkChanged = this.bsModalRef.content.linkChanged as EventEmitter<Link>;
      linkChanged.unsubscribe();
    }

    this.bsModalRef = this._modalService.show(EditLinkComponent, param), this._modalConfig;
    linkChanged = this.bsModalRef.content.linkChanged as EventEmitter<Link>;
    linkChanged.subscribe(link => this.onLinkChanged(quill, link));
    this.bsModalRef.content.closeBtnName = 'Close';
  }

  showImagePopup(quill: any) {
    let param: ModalOptions = JSON.parse(JSON.stringify(this._modalConfig));
    let imageSelected: EventEmitter<Image>;
    param.class = 'image-selector-modal';
    param.initialState = {
      image: {}
    }
    if (this.bsModalRef && this.bsModalRef.content && this.bsModalRef.content.imageSelected) {
      imageSelected = this.bsModalRef.content.imageSelected as EventEmitter<any>;
      imageSelected.unsubscribe();
    }

    this.bsModalRef = this._modalService.show(ImageSelectorComponent, param), this._modalConfig;
    imageSelected = this.bsModalRef.content.imageSelected as EventEmitter<Image>;
    imageSelected.subscribe(image => this.onImageSelected(image, quill));
    this.bsModalRef.content.closeBtnName = 'Close';


    // showImageManager().then(function (selectedImage) {
    //   vm.src = selectedImage.imageSource;
    //   vm.alt = selectedImage.imageAlt;
    //   vm.focusPoint = selectedImage.focusPoint;
    //   //setFocusPoint(vm.focusPoint);
    // }, function (response) {
    //   console.log(response);
    // });
  }

  onLinkChanged(quill: any, link: Link) {
    if (link.linkType === LinkType.File || link.linkType === LinkType.Url) {
      quill.format('link', link.url);
    } else {
      this._pageService.getPage(link.pageId).subscribe((page: Page) => {
        let translation = page.pageTranslation.find(pt => pt.locale === this.pageContext.currentLocale);
        let url = translation.redirectUrl || `${this.pageContext.siteRoot}\\${translation.url}`;
        quill.format('link', url);
      });
    }
  }

  onImageSelected(image: Image, quill: any) {
    let imageUrl = `${this.pageContext.siteRoot}\\${image.imageUrl}`
    quill.insertEmbed(10, 'image', imageUrl);
  }

  addTagPromise = (value: string) => {
    if (this.field.fieldOption.addItemBy) {
      let fieldName = this.field.fieldOption.addItemBy.fieldNameCamelCase;
      let newObj = {};
      newObj[fieldName] = value;
      let promise = this._adminService
        .createRecordFor(this.field.fieldClrType, newObj)
        .pipe(
          map(result => {
            let newRecordResult = (result as FormResult).result;
            this.refreshLookUp().subscribe(adminConfig => {
              this.lookUps = adminConfig.lookUps;
              this.init();
            })
            return newRecordResult
          })
        ).toPromise();


      return promise;
    }
    // this._adminService.createRecordFor()

    return false;

  };

  // addTagPromise(value: string) {

  //   if (this.field.fieldOption.addItemBy) {
  //     let fieldName = this.field.fieldOption.addItemBy.fieldNameCamelCase;
  //     this._adminService
  //     .createRecordFor(this.field.fieldClrType, { fieldName: value })
  //     .pipe(
  //       map(result => {
  //         return (result as FormResult).result
  //       })
  //     ).toPromise();
  //   }
  //   // this._adminService.createRecordFor()

  //   return false;
  // }

  parseControlValue(lookUpGeneric: any) {
    if (this.field && this.field.fieldOption && this.field.fieldOption.relationType) {

      const fieldOption: FieldOption = this.field.fieldOption;

      if (!lookUpGeneric &&
        (fieldOption.relationType === RelationType.ManyToMany ||
          fieldOption.relationType === RelationType.ManyToOne)) {
        // lookUpGeneric = this.lookUps.lookUpData[fieldOption.lookupModelTypeCamelCase];
        lookUpGeneric = this.lookUps.lookUpData[this.field.fieldNameCamelCase];
      }


      if (fieldOption.relationType === RelationType.ManyToMany) {
        this.parseM2mControlVal(lookUpGeneric);
      } else if (fieldOption.relationType === RelationType.ManyToOne) {
        this.parseM2oControlVal(lookUpGeneric);
      } else if (this.field.fieldType === FieldType.CheckBoxMatrix && fieldOption.checkBoxMatrix) {
        const rowLookUpGeneric: any = this.lookUps.lookUpData[fieldOption.checkBoxMatrix.rowTypeCamelCase];
        const colLookUpGeneric: any = this.lookUps.lookUpData[fieldOption.checkBoxMatrix.columnTypeCamelCase];
        const rowKeyNames = Object.keys(rowLookUpGeneric[0].key);
        const colKeyNames = Object.keys(colLookUpGeneric[0].key);
        if (rowLookUpGeneric && colLookUpGeneric) {
          this.rowLookUp = rowLookUpGeneric;
          this.colLookUp = colLookUpGeneric;
          this.rowLookUpKey = rowKeyNames[0];
          this.colLookUpKey = colKeyNames[0];
          this.checkBoxMatrix = fieldOption.checkBoxMatrix;
        }
      }
    }

  }

  parseM2mControlVal(lookUpGeneric: any) {

    if (!this.field || !this.field.fieldOption || !lookUpGeneric) return;

    let formVal = this.form.value;
    let controlVal = formVal[this.field.fieldNameCamelCase];
    let lookUpKeys = this._formControlService.getLookUpKeys(lookUpGeneric);
    let lookUp: any[] = lookUpGeneric;
    let selectedItems = this._formControlService.getSelectedItemsFor(lookUp, lookUpKeys, controlVal);

    let patchVal: any = {};
    patchVal[this.field.fieldNameCamelCase] = selectedItems;

    setTimeout(() => {
      this.form.patchValue(patchVal);
    });

    this._lookUpDataSubject.next(lookUp);
  }

  parseM2oControlVal(lookUpGeneric: any) {
    let formVal = this.form.value;
    let controlVal = formVal[this.field.fieldNameCamelCase];
    let lookUpKeys = this._formControlService.getLookUpKeys(lookUpGeneric);
    let lookUp: any[] = lookUpGeneric;
    let selectedItem = this._formControlService.getSelectedItemFor(lookUp, lookUpKeys, controlVal);

    let patchVal: any = {};
    patchVal[this.field.fieldNameCamelCase] = selectedItem;

    setTimeout(() => {
      this.form.patchValue(patchVal);
    });

    this._lookUpDataSubject.next(lookUp);
  }

  private init() {
    if (this.field && this.field.fieldOption && this.field.fieldOption.hasLookupFilter) {
      console.log(this.field.fieldOption);
      let filterFormCtrl = this.form.get(this.field.fieldOption.lookupFilterField.fieldNameCamelCase);
      this._valChangeSubscription = filterFormCtrl.valueChanges
        .pipe(startWith(null), pairwise())
        .subscribe(([prev, next]: [any, any]) => {
          let val = next ? next : prev;
          console.log(val);
          this._adminService.getLookUp(this.formType, this.formName, this.field.fieldName, val)
            .subscribe(lookupResult => {
              // console.log(lookupResult);
              this.parseControlValue(lookupResult);
            });
        });
    }
    else {
      this.parseControlValue(null);
    }
  }

  private refreshLookUp() {
    return this._adminService.getAdminConfig(true);
  }

  get f() { return this.form.controls; }

  hasError(field: Field): boolean {
    return this.f[field.fieldNameCamelCase] && this.f[field.fieldNameCamelCase].errors && this.f[field.fieldNameCamelCase].touched;
  }

  // onIsValidateChange(isValidate: boolean): void {
  //   let formControl = this.f[this.field.fieldNameCamelCase];
  //   if (isValidate && !this.isDisabled) {
  //     let syncValidators: ValidatorFn[] = [];
  //     let asyncValidators: AsyncValidatorFn[] = [];

  //     syncValidators.push(Validators.required);


  //     switch (this.field.fieldOption.validationType) {
  //       case ValidationType.Email:
  //         syncValidators.push(Validators.email);
  //         // syncValidators.push(Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'));
  //         break;
  //       case ValidationType.NumberOnly:
  //         syncValidators.push(Validators.pattern("^[0-9]*$"));
  //         break;
  //       case ValidationType.LettersOnly:
  //         syncValidators.push(Validators.pattern("^[a-zA-Z]*$"));
  //         break;
  //       case ValidationType.RegEx:
  //         syncValidators.push(Validators.pattern(this.field.fieldOption.validatorRegEx));
  //         break;
  //       case ValidationType.UserExist:
  //         asyncValidators.push(this.userExistValidator.validate.bind(this.userExistValidator));
  //         break;
  //       case ValidationType.UserExistByEmail:
  //         asyncValidators.push(this.emailExistValidator.validate.bind(this.emailExistValidator));
  //         break;
  //       case ValidationType.Password:
  //         asyncValidators.push(this.passwordValidator.validate.bind(this.passwordValidator));
  //         break;
  //       case ValidationType.Custom:
  //         this.customValidator.formType = this.formType;
  //         this.customValidator.formName = this.formName;
  //         this.customValidator.fieldName = this.field.fieldName;
  //         asyncValidators.push(this.customValidator.validate.bind(this.customValidator));
  //         break;
  //     }

  //     formControl.setValidators(syncValidators);

  //     if (asyncValidators.length > 0) {
  //       formControl.setAsyncValidators(asyncValidators);
  //     }
  //   }
  //   else {
  //     formControl.setValidators(null);

  //     if (formControl.asyncValidator && formControl.asyncValidator.length > 0) {
  //       formControl.setAsyncValidators(null);
  //     }

  //   }
  //   formControl.updateValueAndValidity({
  //     onlySelf: true,
  //     emitEvent:false    
  //   });
  // }

}
