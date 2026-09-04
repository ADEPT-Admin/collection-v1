import { Injectable } from '@angular/core';
import { Action, Type, DialogType,RoutingUrl } from '../Enums/common-enum';
import { ShareService } from '../Services/share.service';
import { SweetAlertService } from '../Services/sweet-alert/sweet-alert.service';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Subject, timer, lastValueFrom, firstValueFrom, filter, map } from 'rxjs';
import { Router } from '@angular/router';
import { ConfigService } from '../Services/config.service';
import { TranslateI18nService } from '../Services/translate-i18n.service';
import * as _ from 'lodash';
import { AppLoaderService } from '../Components/loader/loader.service';
import { PermissionService } from '../Services/permission.service';
import dayjs from 'dayjs';



interface MenuOutput {
  name: string;
  link: string[];
  icon: string;
  isShow: boolean;
  children?: MenuOutput[];
  level?: number;
  menuId?: string;
  itemName?: any;
  canLink?: boolean
}



@Injectable()
export class Utils {
  numberFirst:any;
  dataGetVariable: any;
  dataString : any;
  namesoft :any = [];
  strIndex :string = '';
  language = sessionStorage.getItem("language");
  languageStocks: any = {};
  resourceIDList : any = [];
  userPrefSubject$ : Subject<any> = new Subject<any>();
  lockScreen$ : BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  detectDataChange$ : BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);
  userPrefHis : any = null;
  fromHistoryState$: Subject<any> = new Subject<any>();
  resizeScreen$ : Subject<any> = new Subject<any>();
  dataStageNotes$: Subject<any> = new Subject<any>();
  dataProjectNotes$: Subject<any> = new Subject<any>();
  enableUserPreference : boolean;


  menuData: any[];

  myWindow = null;
  constructor(
    private share: ShareService,
    private sweetAlertService : SweetAlertService,
    private translate : TranslateService,
    private router : Router,
    private configService: ConfigService,
    private loader: AppLoaderService,
    private translateI18n: TranslateI18nService,
    private permissionService : PermissionService
  )
  {
      this.enableUserPreference = this.configService.setting.enableUserPreference;
  }


  /// not allow space ///
  hasSpecialCharacters(inputText: string , checkFirstNum = true): boolean{

    let num1New = inputText.slice()[0];

    if (num1New == '_') {
      return true;
    }


   if(checkFirstNum){

      this.numberFirst = Number(num1New)
      if (!isNaN(parseInt(this.numberFirst))) {
        return true;
      }
   }

    const regex = /^\w+$/;
    return !regex.test(inputText);
  }

  /// allow space ///
  hasSpecialCharactersSpace(inputText: string, checkFirstNum = true): boolean{

    if(checkFirstNum){
        let num1New = inputText.slice()[0];
        if (num1New == '_') {
        return true;
      }
        this.numberFirst = Number(num1New)
        if (!isNaN(parseInt(this.numberFirst))) {
        return true;
      }
    }

    const regex = /^[\w\s]+$/;
    return !regex.test(inputText);
  }



  // not use in anything
  // hasSpecialCharactersForLabel(inputText: string): boolean{
  //   const regex = /^[a-zA-Z0-9_]+$/;
  //   //  const regex: RegExp = /[`!@#$%^&*()+\-=\[\]{};':"\\|,.<>\/?~กขฃคฅฆงจฉชซฌญฎฏฐฑฒณดตถทธนบปผฝพฟภมยรฤลฦวศษสหฬอฮฯะัาำิีึืฺุูเแโใไๅๆ็่้๊๋์]/;
  //    return !regex.test(inputText);
  //  }

  hasSpecialCharactersForThai(inputText: string): boolean{
    // const regex: RegExp = /[~กขฃคฅฆงจฉชซฌญฎฏฐฑฒณดตถทธนบปผฝพฟภมยรฤลฦวศษสหฬอฮฯะัาำิีึืฺุูเแโใไๅๆ็่้๊๋์]/;
    const regex: RegExp = /^[\u0E00-\u0E7F]+$/;
    return regex.test(inputText);
  }

  CheckListNamefilter(itemsName: any, HeaderListNew: string): boolean {
    let itemsNameList: any[] = [];
    itemsName.forEach((item) => {
      const value: any = {
        text: item.text,
      }
      itemsNameList.push(value);
    });
    let nameItemstoLowerCase = itemsNameList.filter((f) => f.text.toLowerCase() == HeaderListNew.toLowerCase());
    if (nameItemstoLowerCase.length == 0) {
      return false;
    }
    return true;
  }

  hasUTF8Characters(inputText: string) {
    for (let i = 0; i < inputText.length; i++) {
      if (inputText.charCodeAt(i) > 127) return true;
    }
    return false;
  }


  replaceComma2NewLine(data) {
    //convert string to array and remove whitespace
    let dataToArray = data.split(',').map(item => item.trim());
    //convert array to string replacing comma with new line
    return dataToArray.join("\n");;
  }

  Utf8Encode(strUni: string) {
    return String(strUni).replace(
      /[\u0080-\u07ff]/g,  // U+0080 - U+07FF => 2 bytes 110yyyyy, 10zzzzzz
      function (c) {
        let cc = c.charCodeAt(0);
        return String.fromCharCode(0xc0 | cc >> 6, 0x80 | cc & 0x3f);
      }
    ).replace(
      /[\ud800-\udbff][\udc00-\udfff]/g,  // surrogate pair
      function (c) {
        let high = c.charCodeAt(0);
        let low = c.charCodeAt(1);
        let cc = ((high & 0x03ff) << 10 | (low & 0x03ff)) + 0x10000;
        // U+10000 - U+10FFFF => 4 bytes 11110www 10xxxxxx, 10yyyyyy, 10zzzzzz
        return String.fromCharCode(0xf0 | cc >> 18, 0x80 | cc >> 12 & 0x3f, 0x80 | cc >> 6 & 0x3f, 0x80 | cc & 0x3f);
      }
    ).replace(
      /[\u0800-\uffff]/g,  // U+0800 - U+FFFF => 3 bytes 1110xxxx, 10yyyyyy, 10zzzzzz
      function (c) {
        let cc = c.charCodeAt(0);
        return String.fromCharCode(0xe0 | cc >> 12, 0x80 | cc >> 6 & 0x3f, 0x80 | cc & 0x3f);
      }
    );
  }

  Utf8Decode(strUtf: string) {
    // note: decode 2-byte chars last as decoded 2-byte strings could appear to be 3-byte or 4-byte char!
    return String(strUtf).replace(
      /[\u00f0-\u00f7][\u0080-\u00bf][\u0080-\u00bf][\u0080-\u00bf]/g,  // 4-byte chars
      function (c) {  // (note parentheses for precedence)
        let cc = ((c.charCodeAt(0) & 0x07) << 18) | ((c.charCodeAt(1) & 0x3f) << 12) | ((c.charCodeAt(2) & 0x3f) << 6) | (c.charCodeAt(3) & 0x3f);
        let tmp = cc - 0x10000;
        // TODO: throw error(invalid utf8) if tmp > 0xfffff
        return String.fromCharCode(0xd800 + (tmp >> 10), 0xdc00 + (tmp & 0x3ff)); // surrogate pair
      }
    ).replace(
      /[\u00e0-\u00ef][\u0080-\u00bf][\u0080-\u00bf]/g,  // 3-byte chars
      function (c) {  // (note parentheses for precedence)
        let cc = ((c.charCodeAt(0) & 0x0f) << 12) | ((c.charCodeAt(1) & 0x3f) << 6) | (c.charCodeAt(2) & 0x3f);
        return String.fromCharCode(cc);
      }
    ).replace(
      /[\u00c0-\u00df][\u0080-\u00bf]/g,                 // 2-byte chars
      function (c) {  // (note parentheses for precedence)
        let cc = (c.charCodeAt(0) & 0x1f) << 6 | c.charCodeAt(1) & 0x3f;
        return String.fromCharCode(cc);
      }
    );
  }



  listDialogCheckError(data: any, type: any): void {
    this.dataString = '';
      if (type == "User must contain") {
        let messageUsermustcontainatleast = this.listMessageLanguage(this.translate.currentLang, 'Usermustcontainatleast');
        let messagecharactors = this.listMessageLanguage(this.translate.currentLang, 'charactors');
        this.dataString = `${messageUsermustcontainatleast} ${data} ${messagecharactors}`;
      } else if (type == "User size over limit") {
        let messageUsersizeoverlimit = this.listMessageLanguage(this.translate.currentLang, 'Usersizeoverlimit');
        this.dataString = `${messageUsersizeoverlimit} (maximum = ${data}) ! `;
      } else if (type == "Password must contain") {
        let messagePasswordmustcontainatleast = this.listMessageLanguage(this.translate.currentLang, 'Passwordmustcontainatleast');
        let messagecharactors = this.listMessageLanguage(this.translate.currentLang, 'charactors');
        this.dataString = `${messagePasswordmustcontainatleast} ${data} ${messagecharactors}`;
      } else if (type == "Password size over limit") {
        let messagePasswordsizeoverlimit = this.listMessageLanguage(this.translate.currentLang, 'Passwordsizeoverlimit');
        this.dataString = `${messagePasswordsizeoverlimit} (maximum = ${data}) ! `;
      } else {
        this.dataString = 'ไม่มีเงื่อนไข';
      }
      this.dialogMessageAll('error', this.dataString);
  }




  isStatusProjectAccessControl(dataAll: any ,dataProject): any {
    if(dataAll <= dataProject){
      return true;
    }else{
      return false;
    }
  }

  mergeMessage(msg1 : any,msg2 :any){
    return `<p class="text-center">${msg1} </p> ${msg2} `
  }

  async dialogMessageAll(type:any,message:any, isConfirmSaveChange = false , title = null) {
    if(Array.isArray(message)){
      message = this.listMessage(message);
    }



    switch(type) {
      case DialogType.Error:



        return await this.sweetAlertService.showErrorMessage({title :  title ? title : this.translate.instant(this.toPascalCase(DialogType.Error)) , html : message ,customClass: { htmlContainer : 'px-2 ms-2 me-1 my-0 py-0' }});
      case DialogType.Warning:
        return await this.sweetAlertService.showWarningMessage({title :  title ? title : this.translate.instant(this.toPascalCase(DialogType.Warning)) , html : message ,customClass: { htmlContainer : 'px-2 ms-2 me-1 my-0 py-0' }});
      case DialogType.Success:
        return this.sweetAlertService.showSuccessMessage({title :  title ? title : this.translate.instant(this.toPascalCase(DialogType.Success)) , html : message });
        case DialogType.ConfirmRestore:
          return await this.sweetAlertService.showConfirmationRestore({title :  title ? title : this.translate.instant(this.toPascalCase(DialogType.ConfirmRestore)) , html : message });
        case DialogType.Confirm:
        if(!isConfirmSaveChange){
          return this.sweetAlertService.showConfirmation({title :  title ? title : this.translate.instant(this.toPascalCase(DialogType.Confirm)), html : message });
        }else{

          return this.sweetAlertService.showConfirmation({title :  title ? title : this.translate.instant(this.toPascalCase(DialogType.Warning)), html : message , focusCancel : true  } , true);
        }
      default:
        return await this.sweetAlertService.showMessage({title :  title ? title : this.translate.instant(this.toPascalCase(DialogType.Info)) , html : message });
    }

  }

  async dialogMessageHTML(dialogType:any,title: string, messageLabel: string, messageBody: string){
    if(Array.isArray(messageBody)){
      messageBody = this.listMessage(messageBody);
    }
    switch (dialogType) {
      case DialogType.Confirm:
        return await this.sweetAlertService.showHTML2Buttons({icon: DialogType.Warning, title: title, inputLabel: messageLabel, inputValue: messageBody});
      case DialogType.Success:
        return await this.sweetAlertService.showHTML1Button({icon: DialogType.Success, title: title, inputLabel: messageLabel, inputValue: messageBody});
      default:
        return await this.sweetAlertService.showHTML1Button({icon: DialogType.Info, title: title, inputLabel: messageLabel, inputValue: messageBody});
    }

  }

  async dialogConfirmSaveChange(message:any = null) {
    if(message == null){
      message = this.translate.instant("ConfirmSaveChangeDialog")
    }

    const res = await this.sweetAlertService.showConfirmation({title : this.translate.instant(this.toPascalCase(DialogType.Warning))  , html : message , focusCancel : true  } , true);
    return res
  }

  async isConfirmedCancelandShowDialog(type : string = DialogType.Close): Promise<boolean> {
    let messagebody : string;
    if (type == DialogType.Close) {
      messagebody = this.translate.instant('DialogCancel');
    } else {
      messagebody = this.translate.instant('DialogNew');
    }

    return this.sweetAlertService.showConfirmation({title : this.translate.instant(DialogType.Confirm) , html : messagebody });
  }

  listMessage(itemsMessage: any) {
    let MessageString : any = [];
    itemsMessage.forEach((item) => {
      if(item){
        let statusCodeNumber = Number(item.statusCode);
        let message = '';
        let messageType = '';
        if(statusCodeNumber > 0 && statusCodeNumber <= 9999){
           messageType = 'Information'
        }
        else if(statusCodeNumber >= 10000 && statusCodeNumber <= 19999){
          messageType = 'Warning'
        }
        else if(statusCodeNumber >= 20000 && statusCodeNumber <= 59999){
           messageType = 'Validation'
        }
        else if(statusCodeNumber >= 60000 && statusCodeNumber <= 79999){
           messageType = 'Fatal Error'
        }
        else if(statusCodeNumber >= 80000 && statusCodeNumber <= 89999){
          messageType = 'System Error'
        }
        if(statusCodeNumber != 0){
          message = `<p class="text-start"> <strong> [${messageType}] : </strong> ${item.statusCode} <br> [Message] : ${item.message}</p>`;
          MessageString.push(message);
        }
      }
    });
    //การทำ Dialog แบบแยกบรรทัด
    // this.MessageString.forEach(element => {
    //   let dataelement = element.split(" \t ");

    //   this.GGG.push(dataelement)
    // });
    return MessageString;
  }

  messageLanguage : string = '';
  listMessageLanguage(languageCode: string, messageCode : string): string {
    const translations = this.translateI18n.getTranslations();
    const languageData = translations[languageCode];
    if(languageData && languageData.hasOwnProperty(messageCode)) {
      return languageData[messageCode];
    } else if(languageCode === 'en') {
      return messageCode;
    } else {
      return this.listMessageLanguage('en', messageCode);
    }
  }


  uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
    .replace(/[xy]/g, function (c) {
        const r = Math.random() * 16 | 0,
            v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
  }

  hideTooltip(){
    const tooltip = document.querySelectorAll('.e-tooltip-wrap.e-popup.e-popup-open');
    tooltip.forEach(item => {
      item.setAttribute('style' , 'display:none')
    });
  }


  getLockScreen(){
    return this.lockScreen$.asObservable();
  }


  setLockScreen(){
    const lockScreen = JSON.parse(sessionStorage.getItem("IsScreenLock")!) || false;
    this.lockScreen$.next(lockScreen);
  }


  async updateUserPrefNoAPI(ResourceName : any , ResourceID : any){
    const data = {ResourceName : ResourceName , ResourceID : ResourceID  };
    this.userPrefSubject$.next(data);
  }


  setDetectDataChange(state){
    const loaderId = this.loader.show();
    setTimeout(() => {
      this.loader.hide(loaderId);
    }, 1500);
    this.detectDataChange$.next(state);
  }

  getDetectDataChange(){
    return this.detectDataChange$.asObservable();
  }


  getFromHistorySubject(){
    return this.fromHistoryState$.asObservable();
  }

  setFromHistoryStateSubject(projectID){
    this.fromHistoryState$.next(projectID);

  }

  getUpdateStageNotes(){
    return this.dataStageNotes$.asObservable();
  }

  setUpdateStageNotes(StageNotes){
    this.dataStageNotes$.next(StageNotes);

  }

  getUpdateProjectNotes(){
    return this.dataProjectNotes$.asObservable();
  }

  setUpdateProjectNotes(ProjectNotes){
    this.dataProjectNotes$.next(ProjectNotes);

  }

  async clearUserPrefHis(){
    this.userPrefHis = null;
  }

  setResizeScreen(){
     this.resizeScreen$.next(true);
  }

  getResizeScreen(){
    return this.resizeScreen$.asObservable();
  }

  ValidateEmail(inputText) {
    let mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,100})+$/;
    if (inputText.match(mailformat)) {
      return true;
    }
    return false;
  }

  setDataGridFilter(grid: any){
    let localeStrings = grid?.localeObj?.localeStrings;
    const translations = this.translate.translations[this.translate.currentLang];
    if (localeStrings !== undefined) {
      for(let item in localeStrings){
        if(translations[item] != undefined){
            grid.localeObj.currentLocale[item] = translations[item];

        }
      }
    }

    let localePagerStrings = grid?.pagerModule?.pagerObj?.localeObj?.localeStrings;
    if (localePagerStrings !== undefined) {
      for(let item in localePagerStrings){
        if(translations[item] != undefined){
            grid.pagerModule.pagerObj.localeObj.currentLocale[item] = translations[item];
            grid.pagerModule.pagerObj.localeObj.currentLocale[this.toPascalCase(item)] = translations[item];
        }
      }
    }



    grid.refresh();
    grid.hideSpinner();
    grid.pagerModule.refresh();



  }


  print(strReportData , resourceName= ''){
    let myWindow: Window | null = null;
    myWindow = window.open("", "_blank", "width=800,height=400");
    if(!_.isNil(myWindow)){
        const findString =  strReportData.search('<title></title>');
        if(findString > -1){
          const title = `${this.listMessageLanguage(this.translate.currentLang, 'ADEPT-Print')}-${resourceName}`;
          strReportData = strReportData.replace("<title></title>", `<title>${title}</title>`);
        }
        myWindow.document.write(strReportData);
        myWindow.stop();
    }else{
      const dialogType = DialogType.Warning;
      const dialogBody = this.listMessageLanguage(this.translate.currentLang,'PleaseAllowPopup');
      this.dialogMessageAll(dialogType,dialogBody);
    }
  }


  async decodeWindows1252ToUTF8(text) {
      // Create a Uint8Array from the Windows-1252 encoded text
      let bytes: number[] = [];
      for (let i = 0; i < text.length; i++) {
          bytes.push(text.charCodeAt(i));
      }

      // Convert the byte array to Uint8Array
      let uint8Array = new Uint8Array(bytes);

      // Decode the byte array to UTF-8
      let decoder = new TextDecoder('utf-8');
      let decodedText = decoder.decode(uint8Array);
      return decodedText;
  }

  titleCase(str) {
    return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
  }


  setMenuData(menu){
    this.menuData = menu;
  }

  scrollToIndex(index: number, grid: any) {
    const targetRowElement = grid?.getRowByIndex(index);
    if (targetRowElement) {
      const scrollableContent = grid?.getContent()?.firstElementChild;
      if (scrollableContent) {
        scrollableContent.scrollTop = targetRowElement.offsetTop;
      }
    }
  }


  loadScript(url ,async = false) {

    const existingScript = Array.from(document.getElementsByTagName('script'))
                              .find(s => s.src === url);

    if (existingScript) {
      return; // ถ้ามีแล้วก็ไม่โหลดซ้ำ
    }


    const body = <HTMLDivElement> document.body;
    const script = document.createElement('script');
    script.innerHTML = '';
    script.src = url;
    script.async = async;
    script.defer = true;
    body.appendChild(script);
  }

  handleGridAction(event: any, payload: any): any {
    if (!event?.requestType) return payload;
    switch (event.requestType) {
      case "sorting":
        payload.sortColumn = this.toPascalCase(event.columnName);
        payload.sortDirection = event.direction === "Ascending" ? "asc" : "desc";
        break;
      case "paging":
        payload.pageNumber = event.currentPage;
        payload.pageSize = event.pageSize;
        break;
      case "filtering":
        let filter = {};
        if (event.columns) {
          event.columns.forEach((item: any) => {
            const f = { [this.toPascalCase(item.field)]: item.value };
            filter = { ...filter, ...f };
          });
        }
        payload.filters = filter;
        break;
    }
    return payload;
  }

  toPascalCase(input: string): string {
      if (input == null) return null;
      return input.replace(/[_\- ]+/g, " ").split(" ").map(word => word.charAt(0).toUpperCase() + word.slice(1)).join("");
  }





  getMenuPermission(data){



    const map = new Map<string, MenuOutput>();

    const lang = this.translate.currentLang;

    const items = data.menuItem;
    let whitelist = [];
    let permissions = [];

    items.forEach(item => {
      // console.log(item)
      if (item.itemLevel === 0) return; // skip level 0

      permissions.push({itemId : item.itemId.toString() , permission : item.permission })

      map.set(item.itemId, {
          name: item.itemName[lang],
          link: item.parentId ? [item.routeName] : [item.routeName], // adjust later
          icon: item.icon || "fa-regular fa-circle",
          isShow: item.permission.allowView,
          level: item.itemLevel,
          menuId : item.itemId,
          children: [],
          itemName : item.itemName,
          canLink : false
      });
    });

    let menus: MenuOutput[] = [];

    items.forEach(item => {
      if (item.itemLevel === 0) return;
      const menuItem = map.get(item.itemId)!;


      if (item.parentId && map.has(item.parentId)) {
        const parent = map.get(item.parentId)!;

        // fix link to include parent path
        menuItem.link = [...parent.link, item.routeName];
        parent.children = parent.children || [];
        parent.children.push(menuItem);
      } else {
        if(menuItem.level == 1){
          menus.push(menuItem); // top-level (itemLevel 1)

        }
      }
    })


    menus = this.updateIcons(menus)

    whitelist = this.flattenLeafRoutes(menus);
    return {menu : menus , whitelist : whitelist , permission : permissions}
  }


  flattenLeafRoutes(items: any[]): string[] {
    let routes: string[] = [];

    for (const item of items) {
      const path = '/' + item.link.join('/');

      if (item.children && item.children.length > 0) {
        routes = [...routes, ...this.flattenLeafRoutes(item.children)];
      } else {
        routes.push(path);
      }
    }

    return routes;
  }


  async getPermission(itemId: string) {
    return await firstValueFrom(
      this.permissionService.getPermission().pipe(
        filter(items => items && items.some(item => item.itemId === itemId)),
        map(items => items.find(item => item.itemId === itemId).permission ?? {})
      )
    );
  }


  async getMenu(menuId: string){
    const menuList =  await firstValueFrom(this.permissionService.getMenu());
    const menuData = this.findMenuWithPath(menuList , menuId);
    return menuData

    // for (const menu of menuList) {
    //   const newPath = [...path, { ...menu, children: undefined }]; // clone menu ไม่เอา children

    //   if (menu.menuId === targetId) {
    //     return { menu, path: newPath };
    //   }

    //   if (menu.children && menu.children.length > 0) {
    //     const result = this.findMenuWithPath(menu.children, targetId, newPath);
    //     if (result) {
    //       return result;
    //     }
    //   }
    // }
    // return null;
  }


  updateIcons(menu: any[]): any[] {
  return menu.map(item => {
    const updatedItem = {
      ...item,
      // recursive call for children
      children: item.children && item.children.length > 0
        ? this.updateIcons(item.children)
        : []
    };

    // if no children → make icon solid
    if (!updatedItem.children || updatedItem.children.length === 0) {
      updatedItem.icon = "fa-solid fa-circle";
    }

    return updatedItem;
  });
}

 formatDataTimeOnsave(date: Date){
    if(!_.isNil(date)){

      const formatDate = dayjs(date);
      const formatTime = dayjs(date);

      const merged = formatDate.hour(formatTime.hour()).minute(formatTime.minute()).second(formatTime.second()).millisecond(formatTime.millisecond());

      date = new Date(Date.UTC(merged.get('year'), merged.get('month') , merged.get('date'), merged.get('hours'), merged.get('minutes'), merged.get('second')));
     return date.toISOString();
    }else{
      return null;
    }
  }


  formatDateToUTCString(dateInput: any): string | null {
    if (!dateInput) return null;

    const date = dayjs(dateInput);
    return date.format('YYYY-MM-DDTHH:mm:ss')
  }

  formatDateString(dateInput: any): string | null {
    if (!dateInput) return null;

    const date = dayjs(dateInput);
    return date.format('YYYY-MM-DD')
  }


  transformToLocalDateTime(data){
    if(data == null){
      return null
    }
    const dateTime =  new Date(data);
    return dateTime;
  }


  findMenuWithPath(menuList, targetId, path = []) {
    for (const menu of menuList) {
      const newPath = [...path, { ...menu, children: undefined }]; // clone menu ไม่เอา children

      if (menu.menuId === targetId) {
        return { menu, path: newPath };
      }

      if (menu.children && menu.children.length > 0) {
        const result = this.findMenuWithPath(menu.children, targetId, newPath);
        if (result) {
          return result;
        }
      }
    }
    return null;
  }


  getFirstChildLinkFromArray(menuList: any[], index: number): string[] {
    const target = menuList[index];
    return this.getFirstChildLinkRecursive(target);
  }


  getFirstChildLinkRecursive(menu: any): string[] {
    // If this item has children, go deeper
    if (menu?.children && menu.children.length > 0) {
      return this.getFirstChildLinkRecursive(menu.children[0]);
    }

    // Otherwise, return its link
    return menu?.link ?? [];
  }

  getAlign(type: string) {
    switch (type) {
      case 'Integer': return 'Right';
      case 'Decimal': return 'Right';
      case 'DateTime': return 'Center';
      case 'Date': return 'Center';
      case 'Enum': return 'Center';
      default: return 'Left';
    }
  }

}
