import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'lowercaseFirst' })
export class LowercaseFirstPipe implements PipeTransform {
  transform(value: string): string {
    return value.charAt(0).toLowerCase() + value.slice(1);
  }
}
