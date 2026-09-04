import { AfterContentInit, Component, ContentChildren, QueryList } from '@angular/core';
import { TabComponent } from './tab/tab.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tabs',
  imports: [CommonModule],
  templateUrl: './tabs.component.html',
  styleUrl: './tabs.component.scss',
})
export class TabsComponent implements AfterContentInit {
  @ContentChildren(TabComponent) tabs!: QueryList<TabComponent>;

  ngAfterContentInit() {
    const activeTab = this.tabs.find(t => t.active);
    if (!activeTab && this.tabs.first) {
      this.selectTab(0);
    }
  }

  selectTab(index: number) {
    this.tabs.forEach((tab, i) => {
      tab.active = (i === index);

      if (tab.active || !tab.lazy) {
        tab.loaded = true; // preload content if not lazy
      }
    });
  }
}
