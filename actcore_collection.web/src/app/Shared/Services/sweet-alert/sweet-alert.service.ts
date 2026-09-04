import { Injectable } from '@angular/core';
import Swal, {SweetAlertOptions} from 'sweetalert2';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import * as _ from 'lodash';
import { DialogType, Icon } from '../../Enums/common-enum';
@Injectable({
  providedIn: 'root'
})
export class SweetAlertService {

  strOK : string = '';
  strCancel : string = '';

  customClass = {
    animation : true,
    customClass : {
      confirmButton: ' e-btn  e-primary',
      cancelButton: 'e-btn btn-secondary',
      inputLabel: 'swal2-input-label-centered',
      htmlContainer : 'my-0 px-2 py-0 ms-2 me-1',
    }
  }

  inputAttributes ={
    inputAttributes: {
      readonly: 'true',
      style: 'font-size: 16px'
    }
  }


  constructor(
    private translate: TranslateService
  ) {



    this.initVal();
    this.translate.onLangChange.subscribe( async ( event : LangChangeEvent) => {
      this.strOK = await this.translate.get('OK').toPromise();
      this.strCancel = await this.translate.get('Cancel').toPromise();
    });
  }

  async initVal(){
      this.strOK = await this.translate.get('OK').toPromise();
      this.strCancel = await this.translate.get('Cancel').toPromise();
  }

  async showConfirmation(options: SweetAlertOptions , isConfirmSaveChage = false): Promise<any> {

    const  title = await this.translate.get( DialogType.Warning).toPromise();
    const customOption = _.cloneDeep(this.customClass);
    if(isConfirmSaveChage){
      customOption.customClass.confirmButton = 'e-btn btn-secondary';
      customOption.customClass.cancelButton = 'e-btn  e-primary';
    }else{
      customOption.customClass.confirmButton = 'e-btn e-primary ';
      customOption.customClass.cancelButton = 'e-btn  btn-secondary';
    }


    if(isConfirmSaveChage){
      if(Swal.isVisible()){
        return false;
      }
    }




    const result = await Swal.fire({
      icon: DialogType.Warning,
      title: title,
      allowOutsideClick: false,
      showCancelButton: true,
      // showCloseButton: true,
      confirmButtonText: `${Icon.Confirmed} ${this.strOK}`,
      cancelButtonText: `${Icon.Cancel} ${this.strCancel}`,
      // confirmButtonColor: Color.Blue,
      ...options,
      ...customOption
    });


    return result.isConfirmed;


  }

  async showConfirmationRestore(options: SweetAlertOptions): Promise<any> {

    const  title = await this.translate.get( DialogType.Warning).toPromise();
    const  validateText = await this.translate.get(`RequiredField`).toPromise();
    const  EnterValidText = await this.translate.get(`MustEnterValidInfo`).toPromise();
    const customOption = _.cloneDeep(this.customClass);
    const result = await Swal.fire({
      icon: DialogType.Warning,
      input: 'text',
      allowOutsideClick: false,
      showCancelButton: true,
      confirmButtonText: `${Icon.Confirmed} ${this.strOK}`,
      cancelButtonText: `${Icon.Cancel} ${this.strCancel}`,
      ...options,
      ...customOption,
      title: title,
      preConfirm: (inputValue) => {
        if (!inputValue) {
          Swal.showValidationMessage(validateText)
          return false;  // Prevents dialog from closing
        }else if(inputValue !== 'Restore'){
          Swal.showValidationMessage(EnterValidText)
          return false;  // Prevents dialog from closing
        }
        return inputValue;
      }
    });

    return result;

  }

  async showHTML2Buttons(options: SweetAlertOptions): Promise<any> {

    const result = await Swal.fire({
      allowOutsideClick: false,
      showCancelButton: true,
      width: '600px',
      confirmButtonText: `${Icon.Confirmed} ${this.strOK}`,
      cancelButtonText: `${Icon.Cancel} ${this.strCancel}`,
      input: 'textarea',
      ...options,
      ...this.customClass,
      ...this.inputAttributes
    });

    return result.isConfirmed;
  }

  async showHTML1Button(options: SweetAlertOptions): Promise<any> {

    const result = await Swal.fire({
      allowOutsideClick: false,
      showCancelButton: true,
      width: '600px',
      confirmButtonText: `${Icon.Confirmed} ${this.strOK}`,
      input: 'textarea',
      ...options,
      ...this.customClass,
      ...this.inputAttributes
    });

    return result.isConfirmed;
  }
  async showMessage(options: SweetAlertOptions): Promise<any> {
    const result = await Swal.fire({
      icon: DialogType.Info,
      allowOutsideClick: false,
      confirmButtonText: `${Icon.Confirmed} ${this.strOK}`,
      // confirmButtonColor: Color.Blue,
      ...options,
      ...this.customClass
    });

    return result.isConfirmed;
  }

  async showErrorMessage(options: SweetAlertOptions): Promise<any> {
    let optionsData:any = options.customClass;
    const customClass = {customClass:{...this.customClass.customClass , ...optionsData }}
    if(Array.isArray(options.html)){
      options.html = options.html.join("");
    }

    const findIconErr = Swal.getIcon()?.classList.contains('swal2-error');
    if(!Swal.isVisible() ||  !findIconErr){
      const result = await Swal.fire({
        icon: DialogType.Error,
        allowOutsideClick: false,
        confirmButtonText: `${Icon.Confirmed} ${this.strOK}`,
        // confirmButtonColor: Color.Blue,
        ...options,
        ...customClass
      });

      return result.isConfirmed;
    }else{
      return false;
    }

  }

  async showWarningMessage(options: SweetAlertOptions): Promise<any> {
    let optionsData:any = options.customClass;
    const customClass = {customClass:{...this.customClass.customClass , ...optionsData }}
    if(Array.isArray(options.html)){
      options.html = options.html.join("");
    }
    const result = await Swal.fire({
      icon: DialogType.Warning,
      allowOutsideClick: false,
      confirmButtonText: `${Icon.Confirmed} ${this.strOK}`,
      // confirmButtonColor: Color.Blue,
      ...options,
      ...customClass
    });

    return result.isConfirmed;
  }

  async showSuccessMessage(options: SweetAlertOptions): Promise<any> {

    const customOption = _.cloneDeep(this.customClass);
    customOption.customClass.htmlContainer = '';
    const result = await Swal.fire({
      icon: DialogType.Success,
      allowOutsideClick: false,
      confirmButtonText: `${Icon.Confirmed} ${this.strOK}`,
      // confirmButtonColor: Color.Blue,
      ...options,
      ...customOption
    });

    return result.isConfirmed;
  }
}
