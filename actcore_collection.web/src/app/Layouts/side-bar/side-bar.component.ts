import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, Input, OnChanges } from '@angular/core';
import { IsActiveMatchOptions, Router, RouterLink, RouterLinkActive, RouterModule, UrlTree } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { OverlayscrollbarsModule } from 'overlayscrollbars-ngx';


export interface MenuItem {
  name: string;
  link: string[] | string;   // your data uses string[] (e.g., ["security","user"])
  icon: string;
  isShow: boolean;
  children: MenuItem[];
  level?: number;
}


@Component({
    selector: 'app-side-bar',
    imports: [CommonModule, OverlayscrollbarsModule, TranslateModule , RouterLink , RouterLinkActive, RouterModule  ],
    templateUrl: './side-bar.component.html',
    styleUrl: './side-bar.component.scss'
})
export class SideBarComponent  implements AfterViewInit {
  @Input() showSideBar = true;

  @Input() menus = [];




  isOpen = false;

  isSuperVisor = false;

  constructor(private router : Router){

    const employee = JSON.parse(sessionStorage.getItem('employee') || '{}');
    this.isSuperVisor = employee?.isSupervisorCollector;

  }


  ngAfterViewInit() {


  }


  toggleMenu(event: Event): void {
    event.preventDefault();
    this.isOpen = !this.isOpen;
  }


  private matchSubset: IsActiveMatchOptions = {
    paths: 'subset',
    queryParams: 'ignored',
    matrixParams: 'ignored',
    fragment: 'ignored',
  };

  private toUrlTree(link: string[] | string): UrlTree {
    return Array.isArray(link)
      ? this.router.createUrlTree(link as string[])
      : this.router.parseUrl(link as string);
  }

  /** For branch nodes: highlight/open if ANY descendant is active */
  isBranchActive(node: MenuItem): boolean {
    // if node has its own link, consider it too
    const selfActive = node.link ? this.router.isActive(this.toUrlTree(node.link), this.matchSubset) : false;
    if (selfActive && node?.children?.length > 0) return true;

    if (!node.children?.length) return false;
    return node.children.some(c => this.isBranchActive(c));
  }


  isChildBranchActive(node: MenuItem): boolean {

    // // if node has its own link, consider it too
    if(node.level == 0) return false
    const selfActive = node.link ? this.router.isActive(this.toUrlTree(node.link), this.matchSubset) : false;
    if (selfActive && node?.children?.length > 0) return true;

    return false
  }


  isActive(node: MenuItem): boolean {
    // if node has its own link, consider it too
    const selfActive = node.link ? this.router.isActive(this.toUrlTree(node.link), this.matchSubset) : false;
    if (selfActive && node?.children?.length == 0) return true;
    if (!node.children?.length) return false;
    return false;
  }


}
