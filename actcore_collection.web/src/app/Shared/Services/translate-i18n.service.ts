import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TranslateI18nService {

  private translations: { [key: string]: any } = {};

  constructor() { }

  getTranslations(){
    return this.translations;
  }

  setTranslations(translations: { [key: string]: any }){
    this.translations = translations;
  }

}
