import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MasterMenu, SecurityService } from '../security.service';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import * as _ from 'lodash';
import { ActivatedRoute, Router } from '@angular/router';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';
import { SelectComponent } from 'src/app/Shared/Components/select/select.component';
import { InputWithButtonComponent } from 'src/app/Shared/Components/input-with-button/input-with-button.component';
import { ModalService } from 'src/app/Shared/Services/modal.service';
import { IconModalComponent } from 'src/app/Modals/icon-modal/icon-modal.component';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-master-menu-detail',
    imports: [
        CommonModule,
        FormsModule,
        ToolbarComponent,
        ReactiveFormsModule,
        TranslateModule,
        HeaderComponent,
        SelectComponent,
        InputWithButtonComponent,
        InputNumberComponent,
    ],
    templateUrl: './master-menu-detail.component.html',
    styleUrl: './master-menu-detail.component.scss',
    providers: [SecurityService, Utils]
})
export class MasterMenuDetailComponent implements OnInit , AfterViewInit, CanComponentDeactivate  {

  async canDeactivate(): Promise<boolean> {
    if (!this.form?.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }

  @ViewChild('myForm') form! : NgForm ;
  actionListbtn : any = {
    new : { show : false , text : null ,disabled : false},
    save : { show : true , text : null ,disabled : true},
    delete : { show : false , text : null ,disabled : false},
    cancel : { show : true , text : null ,disabled : false},
    back : { show : true , text : null ,disabled : false}
    };

  detail : any = [];
  original : MasterMenu = { };
  id = null;
  permission = {
      "allowView": null,
      "allowNew": false,
      "allowEdit": false,
      "allowDelete": false
    };
  payload: any = {
    pageNumber: 1,
    pageSize: 10,
    sortColumn: null,
    sortDirection: 'asc',
    filters: {}
  }

  menuId = null;
  header = null;
  selectMenuId : any;

  icon : string = '';

  lang$ : Subscription = null;



  constructor(
      private routed : ActivatedRoute,
      private router : Router,
      private securityService : SecurityService,
      private cdRef: ChangeDetectorRef,
      private utils : Utils,
      private translate : TranslateService,
      private modal: ModalService,
      private loader: AppLoaderService
    ){
      this.actionListbtn.cancel.disabled = !this.form?.dirty;
      this.routed.paramMap.subscribe(async params => {
        this.id = params.get('id');
        this.detail = await this.securityService.getMenuDetail(this.id);


        if (this.detail.status == true) {
          this.detail = (this.detail as any).data || {};
          this.original = _.cloneDeep(this.detail);
        if(this.detail.icon){
            this.icon = `<i class="${this.detail.icon}"></i>`;

        }else{
           this.icon = '';
        }
        this.menuId = this.routed.snapshot.data?.menuId || '0906';
        this.permission = await this.utils.getPermission(this.menuId);
        this.header = await this.utils.getMenu(this.menuId)
        this.header.path[this.header.path.length -1].canLink = true;
        this.header.path = [...this.header.path , ...[{menuId : 'detail' , itemName : {th : `รายละเอียด (${this.id})` , en : `Detail (${this.id})`}} ]];
        if (!this.permission?.allowEdit) {
          setTimeout(() => {
            this.form?.control.disable();
          }, 200);
        }
        this.cdRef.detectChanges();
            } else {
              const message =  this.detail.message[this.translate.currentLang];
              this.loader.hideAll();
              await this.utils.dialogMessageAll('error', message);
              this.doBack();
            }

      });
      this.getMenuListUrl();

      this.translate.onLangChange.subscribe(data => {
        this.selectMenuId = this.selectMenuId.map(c => ({
          label: `${c.value} - ${c.itemName[this.translate.currentLang]}`,
          value: c.value,
          itemName : c.itemName
        }));

      });



    }

  async getMenuListUrl() {
    delete this.payload.filters.selectFilter;
    this.securityService.getMenuListUrl().then(res => {
       let result : any = res;
      this.selectMenuId = result.data.filter(item => item.itemLevel == 1 && item.isActive);
      this.selectMenuId = this.selectMenuId.map(c => ({
        label: `${c.itemId} - ${c.itemName[this.translate.currentLang]}`,
        value: c.itemId,
        itemName : c.itemName,
      }));

    })
  }

  ngOnInit(){}

  ngOnDestroy(){
    if(this.lang$){ this.lang$.unsubscribe()}
  }

  ngAfterViewInit(): void {
    this.cdRef.detectChanges();
  }


  async openicon(){
    const res: any = await this.modal.open(IconModalComponent, { header: this.translate.instant('SelectIcon'), width: '45%'  });
    if (res) {
      this.detail.icon = res.code
      this.icon = `<i class="${res.code}"></i>`;
      this.form.control.markAsDirty();
    }
  }


  async clearIcon(){
    this.icon = '';
    this.detail.icon = '';
    this.form.control.markAsDirty();
  }

  doBack(){
    this.router.navigate(['..'] , {relativeTo : this.routed})
  }

  async doSave() {
    const res = await this.securityService.updateMenu(this.detail);
    if (res.status) {
      const message = res.message[this.translate.currentLang];
      this.utils.dialogMessageAll('success', message);
      this.original = _.cloneDeep(this.detail);
      this.markFormPristine();
    } else {
      const message = res.message[this.translate.currentLang];
      this.utils.dialogMessageAll('error', message);
    }
  }

  async doCancel(){
    const messagebody =  this.translate.instant('DialogCancel');
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then((res) => {
      if(res){
        this.detail = _.cloneDeep(this.original);
        if(this.detail.icon){
            this.icon = `<i class="${this.detail.icon}"></i>`;
        }else{
           this.icon = '';
        }
        this.markFormPristine();
      }
    });
  }

  markFormPristine() {
    Object.values(this.form.controls).forEach(control => {
      control.markAsPristine();
      control.markAsUntouched(); // Optional: also reset "touched"
      control.updateValueAndValidity();
    });
  }


}
