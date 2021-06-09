import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'split'
})
export class SplitPipe {

  transform(value: string): string {
    if (value) {
      if (value.toString().length <= 2) {
        return value;
      }
      let firstLine = value.toString().substring(0, 2);
      let secondLine = value.toString().substring(2, value.toString().length);

      return firstLine + ' : ' + secondLine
    }
    return value;
  }

}
