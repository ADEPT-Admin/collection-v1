import { CommonModule } from '@angular/common';
import { AfterViewInit, ChangeDetectorRef, Component, Input } from '@angular/core';
import { ContractDescription } from '../../../task.model';
import { TranslateModule } from '@ngx-translate/core';
import { DateTimePickerModule } from '@syncfusion/ej2-angular-calendars';
import { FormsModule } from '@angular/forms';
import { InputNumberComponent } from 'src/app/Shared/Components/input-number/input-number.component';
import { WorkService } from '../../../work.service';
import { timer } from 'rxjs';
import { CalendarComponent } from 'src/app/Shared/Components/calendar/calendar.component';

@Component({
  selector: 'app-contract-description',
  imports: [CommonModule , TranslateModule , DateTimePickerModule , FormsModule , InputNumberComponent,CalendarComponent],
  templateUrl: './contract-description.component.html',
  styleUrl: './contract-description.component.scss',
})
export class ContractDescriptionComponent implements AfterViewInit {
  @Input() key : any;


  constructor(
    private workService : WorkService,
    private cd : ChangeDetectorRef
  ){

  }


  detail : ContractDescription = {}



  ngAfterViewInit(): void {
    this.cd.detectChanges()
    timer(10).subscribe(() => {
      this.getData();
    })
  }


  async getData(){
    if(this.key != undefined){
      const res = await this.workService.getContractDetail(this.key);
      this.detail = res.data
    }

  }

}
