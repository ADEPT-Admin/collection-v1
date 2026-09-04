import { GroupPermissionItem, Menu, UserGroup } from '../security.service';
import { CommonModule } from '@angular/common';
import { AfterViewInit, ChangeDetectorRef, Component,  OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { SecurityService } from '../security.service';
import { ToolbarComponent } from 'src/app/Shared/Components/toolbar/toolbar.component';
import { CanComponentDeactivate } from 'src/app/Shared/Guards/confirm-exit.guard';
import { Subscription, timer } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Utils } from 'src/app/Shared/Utilites/utils';
import { AppLoaderService } from 'src/app/Shared/Components/loader/loader.service';

import {  TreeGridComponent, TreeGridModule } from '@syncfusion/ej2-angular-treegrid';
import { HeaderComponent } from 'src/app/Shared/Components/header/header.component';

@Component({
    selector: 'app-group-detail',
    imports: [CommonModule,
        FormsModule,
        ToolbarComponent,
        ReactiveFormsModule,
        TranslateModule,
        TreeGridModule,
        HeaderComponent
    ],
    templateUrl: './group-detail.component.html',
    styleUrl: './group-detail.component.scss',
    providers: [SecurityService]
})
export class GroupDetailComponent implements OnInit , CanComponentDeactivate  ,AfterViewInit {

  @ViewChild('myForm') form : NgForm;
  @ViewChild('treegrid', { static: false }) treeGridObj!: TreeGridComponent;

  actionListbtn : any = {
    new : { show : true , text : null ,disabled : false},
    save : { show : true , text : null ,disabled : true},
    delete : { show : true , text : null ,disabled : false},
    cancel : { show : true , text : null ,disabled : true},
    back : { show : true , text : null ,disabled : false}
   }


  detail : UserGroup = { isActive : false };

  original : any  = {}
  companies = [
    {
      id : 1,
      code : '01',
      name : 'Adeptivate'
    },
  ]

  menu  :Menu[] = [];

  expandedRows: { [key: string]: boolean } = {};


  async canDeactivate(): Promise<boolean> {
    if (!this.form.dirty) return true;
    const result = await this.utils.dialogConfirmSaveChange()
    return result;
  }


  usergroupSub : Subscription = null;
  id : string = null;
  previousId : string = null;
  showTable = false;
  customAttributes = { class: 'customcss' };

  list = [];

  lang = 'en';
  menuPermission = [];
  permission = {
    "allowView": false,
    "allowNew": false,
    "allowEdit": false,
    "allowDelete": false
  };

  menuId = null;
  header = null;


  disableCheckAll = false;


  constructor(
    private routed : ActivatedRoute,
    private router : Router,
    private securityService : SecurityService,
    private utils : Utils,
    private translate : TranslateService,
    private loader : AppLoaderService,
    private cd : ChangeDetectorRef
  ){

      this.actionListbtn.cancel.disabled = !this.form?.dirty || true;
      this.lang = translate.currentLang;
      this.routed.paramMap.subscribe(async params => {
        this.menuId = this.routed.snapshot.data?.menuId || '9103';
        this.permission = await this.utils.getPermission(this.menuId);
        if (this.permission?.allowView) {
          this.cd.detectChanges();
        }


        this.header = await this.utils.getMenu(this.menuId)
        this.header.path[this.header.path.length -1].canLink = true;

        this.id = params.get('id');
        const loaderId = this.loader.show();

        if(this.id != 'new'){
          this.previousId = this.id;
        }else{
          this.id = null;
        }

        await this.getData();

        if(this.id == null){
          this.actionListbtn.delete.disabled = true;
          this.actionListbtn.new.disabled = true;
          this.header.path = [...this.header.path , ...[{itemName : {th : 'สร้าง' , en : 'New'}}]]

        }else{
          this.header.path = [...this.header.path , ...[{menuId : 'detail' , itemName : {th : `รายละเอียด (${this.detail.userGroupCode})` , en : `Detail (${this.detail.userGroupCode})`}}]]

        }



        if (!this.permission?.allowView || (!this.permission?.allowNew || !this.permission?.allowEdit )) {
          setTimeout(() => {
            this.form?.control.disable();
            this.disableCheckAll = true;
          }, 200);
        }

        if(this.permission?.allowNew == true && this.id == null){
          setTimeout(() => {
            this.form?.control.enable();
            this.disableCheckAll = false;
          }, 200);
        }


        if(this.permission.allowEdit && this.id != null){
          setTimeout(() => {
              this.form?.control.enable();
              this.disableCheckAll = false;
              this.form?.controls['userGroupCode']?.disable();

            }, 200);
        }




        this.loader.hide(loaderId)
      })

      this.translate.onLangChange.subscribe(data => {
        this.lang = data.lang
      });

  }

  async getData(){
    this.actionListbtn.new.disabled = !this.permission.allowNew;
    this.actionListbtn.delete.disabled = !this.permission.allowDelete;

    if(this.id == null){
      this.actionListbtn.new.disabled = true;
      this.actionListbtn.delete.disabled = true;
    }

    const res = await this.securityService.getUserGroup(this.id)



    if(res.status == true){
      this.original = _.cloneDeep(res.data);
      this.list = (res as any).data.menuItems;
      this.detail = (res as any).data.userGroup || {};
      await this.buildMenu();
      timer(30).subscribe(() =>{
        this.treeGridObj?.refresh();

      })
    }else{
      const message = res.message[this.translate.currentLang];
      this.loader.hideAll();
      await this.utils.dialogMessageAll('error', message);
      this.doBack();
    }




  }

  async buildMenu(){
    this.menuPermission = this.buildNestedTree(this.list);
    this.setParents(this.menuPermission);
  }


  ngOnDestroy(){
    if(this.usergroupSub){
      this.usergroupSub.unsubscribe();
    }
  }

  ngAfterViewInit(): void {
    this.cd.detectChanges();
  }

  async getTemplate(){
    const template = await this.securityService.getPermissionTeamplete();
    this.menu = await this.transformMenu(template);


  }

  async transformMenu(items: GroupPermissionItem[]) {
    const mainMenus = items.filter(i => i.itemLevel === 1);
    const subMenus = items.filter(i => i.itemLevel === 2);


    return mainMenus.map(main => {
      const subs: any[] = subMenus
        .filter(sub => sub.parentId === main.itemId)
        .map(sub => {

          const selectItem = [];
            const item = {
              id: sub.itemId,
              function: sub.itemNameEn,
              parentId: sub.parentId,
              access: {
                allowNew: selectItem.length > 0 ? selectItem[0].allowNew : sub.allowNew,
                allowEdit: selectItem.length > 0 ? selectItem[0].allowEdit : sub.allowEdit,
                allowDelete: selectItem.length > 0 ? selectItem[0].allowDelete : sub.allowDelete,
                allowCancel: selectItem.length > 0 ? selectItem[0].allowCancel : sub.allowCancel,
                allowQuery: selectItem.length > 0 ? selectItem[0].allowQuery : sub.allowQuery,
                allowPrint: selectItem.length > 0 ? selectItem[0].allowPrint : sub.allowPrint,
                allowAccess : selectItem.length > 0 ? selectItem[0].allowAccess : sub.allowAccess,
              },
            }

            return item;
        });
      return {
        id: main.itemId,
        main: main.itemNameEn,
        sub: subs,
      };
    });
  }

  ngOnInit(){

  }




  async doNew(){
    let res = false
    if(this.form.dirty){
      const result = await this.utils.dialogConfirmSaveChange()
      res = result;
    }else{
      res = true;
    }

    if (res == true) this.router.navigate(['../new'], { relativeTo: this.routed });
  }

  doBack(){

    this.router.navigate(['..'] , {relativeTo : this.routed})

  }


  async doSave() {
    this.form.onSubmit(null);
    this.form.control.markAllAsTouched();
    if (this.form.valid) {
      const flatten = this.revertToflatten(this.menuPermission)
      let menuList = this.original.menuItems;
      const menuItems = menuList.map(menu => {
        const selectItem = flatten.filter(item => item.itemId == menu.itemId);
        if (selectItem?.length > 0) {
          menu.allowView = selectItem[0].allowView;
          menu.allowNew = selectItem[0].allowNew;
          menu.allowEdit = selectItem[0].allowEdit;
          menu.allowDelete = selectItem[0].allowDelete;
           menu.allowAccess = selectItem[0].allowAccess;
        }
        return menu
      })

      const payload = { userGroup: this.detail, menuItems: menuItems };
      let res = null
      const loaderId = this.loader.show();
      if (this.id != null) {
        res = await this.securityService.updateUserGroup(payload);
      } else {
        res = await this.securityService.createUserGroup(payload);
      }


      this.loader.hide(loaderId)
      if (res?.status) {
        const message = res.message[this.translate.currentLang];
        this.utils.dialogMessageAll('success', message);
        this.markFormPristine();
        if (this.id == null) {
          this.router.navigate(['../', res.data.userGroup.userGroupId], { relativeTo: this.routed })
        } else {
          this.getData();
        }
      } else {
          const message = res.message[this.translate.currentLang];
          this.loader.hideAll();
          await this.utils.dialogMessageAll('error', message);
      }
    }
  }

  async doDel() {
    let messagebodyDialogDelete = this.translate.instant('DialogDelete');
    let messagebody = `${messagebodyDialogDelete}`
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then(async (confirm) => {
      if (confirm) {
        const loaderId = this.loader.show();
        const payload = [{ userGroupId: this.id }];
        const res = await this.securityService.deleteUserGroup(payload);
        this.loader.hide(loaderId);
        if (res.status == true) {
          const message = res.message[this.translate.currentLang];
          this.utils.dialogMessageAll('success', message);
          if (res) {
            this.markFormPristine();
            this.router.navigate(['..'], { relativeTo: this.routed });
          }
        } else {
          const message = res.message[this.translate.currentLang];
          this.loader.hideAll();
          await this.utils.dialogMessageAll('error', message);
        }
      }
    })
  }

  async doCancel(){
    const messagebody =  this.translate.instant('DialogCancel');
    const isConfirmed = this.utils.dialogMessageAll('confirm', messagebody);
    isConfirmed.then((res) => {
      if(res){
        if(this.id == null){
          this.markFormPristine();
          if(this.previousId != null){
              this.router.navigate(['..',this.previousId], { relativeTo: this.routed });
          }else{
              this.doBack();
          }
        }else{
          const original = _.cloneDeep(this.original);
          this.list = (original as any).menuItems;
          this.detail = (original as any).userGroup || {};
          this.buildMenu();
          this.markFormPristine();
        }
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



  buildNestedTree(items: any[]): any[] {
    const lookup: { [key: string]: any } = {};
    const roots: any[] = [];

    items.forEach(item => {
      if (item.itemLevel > 0) { // ignore level 0
        lookup[item.itemId] = { ...item, children: [] };
      }
    });

    // Build the tree
    Object.values(lookup).forEach(item => {
      if (item.parentId && lookup[item.parentId]) {
        // Add to direct parent
        lookup[item.parentId].children.push(item);
      } else if (!item.parentId || item.itemLevel === 1) {
        // Top-level roots (level 1)
        roots.push(item);
      }
    });

    return roots;
  }

  revertToflatten(tree: any[]): any[] {
    const result: any[] = [];

    function recurse(node: any, parentId: string | null, level: number) {
      const { children, ...rest } = node; // remove children
      result.push({ ...rest, parentId, itemLevel: level });

      if (children && children.length > 0) {
        children.forEach((child: any) => recurse(child, node.itemId, level + 1));
      }
    }

    tree.forEach(node => recurse(node, node.parentId ?? null, node.itemLevel));
    return result;
  }


  onCheckboxChange(rowData: any, key: any ,event: any) {
    rowData[key] = event.target.checked;

    // Find and update original object in menuPermission
    const original = this.findById(this.menuPermission, rowData.itemId);
    if (original) {
      original[key] = rowData[key];
      this.treeGridObj.refresh();
    }
  }

  findById(data: any[], id: string): any {
    for (let item of data) {
      if (item.itemId === id) return item;
      if (item.children?.length) {
        const found = this.findById(item.children, id);
        if (found) return found;
      }
    }
    return null;
  }


  onRowDataBound(args: any) {
    if (args.data.hasChildRecords) {
      // parent row
      args.row.classList.add('treegrid-parent');
      if (!args.data.expanded) {
        args.row.classList.add('treegrid-collapsed');
      }
    }
  }

  // Toggle all children when parent checkbox clicked
  toggleChildren(parent: any, field: string, event: any) {
    const checked = event.target.checked;
    parent.children.forEach((child: any) => {
      this.setRecursive(child, field, checked);
      this.updateParentStatus(child, field);
    });

    this.refreshTreeGrid(); // force UI update
    this.form.control.markAsDirty();
  }

  refreshTreeGrid() {
    this.treeGridObj.refresh();
  }

  // Recursive helper
  setRecursive(node: any, field: string, value: boolean) {
    if (!node.children?.length) {
      node[field] = value;


    } else {
      node.children.forEach((child: any) => this.setRecursive(child, field, value));
    }
  }

  // Check if all children are checked
  isAllChecked(children: any[] , field: string , item = null): boolean {
    const checked = children.every(child =>
      child.children?.length
        ? this.isAllChecked(child.children, field , child)
        : child[field]
    );

    return checked
  }

  // Check if some are checked (indeterminate)
  isIndeterminate(children: any[], field: string , item = null): boolean {
    const someChecked = children.some(child =>
      child.children?.length
        ? this.isIndeterminate(child.children, field , child) || this.isAllChecked(child.children, field)
        : child[field]
    );


    const checkAll = this.isAllChecked(children, field);

    const res = someChecked && !checkAll;
    return res;
  }


  updateParentStatus(node: any, field: string) {
    if (!node.parent) return;

    const parent = node.parent;

    const allChecked = parent.children.every((child: any) =>
      child.children?.length
        ? this.isAllChecked(child.children, field)
        : child[field]
    );

    const someChecked = parent.children.some((child: any) =>
      child.children?.length
        ? this.isIndeterminate(child.children, field) || this.isAllChecked(child.children, field)
        : child[field]
    );

    parent[field] = allChecked;
    parent.indeterminate = someChecked && !allChecked;

    // recurse upward
    this.updateParentStatus(parent, field);
  }


  setParents(nodes: any[], parent: any = null) {
    nodes.forEach(node => {
      node.parent = parent;
      if (node.children?.length) {
        this.setParents(node.children, node);
      }
    });
  }

}
