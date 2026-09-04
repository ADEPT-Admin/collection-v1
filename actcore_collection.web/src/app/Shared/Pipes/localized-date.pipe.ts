import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import dayjs from 'dayjs';
@Pipe({
  name: 'localizedDate',
  pure: false,
})
export class LocalizedDatePipe implements PipeTransform {

  constructor(private translateService: TranslateService) { }

  transform(value: Date | string, format = 'mediumDate'): string | null {
    if (!value) return null;
    // Multilang 03 Pipe module for ปีพศ
    if (this.translateService.currentLang == 'th') {
      value = dayjs(value).add(543, 'year').toDate();
    }
    const datePipe = new DatePipe(this.translateService.currentLang || 'en');
    return datePipe.transform(value, format);
  }

}
