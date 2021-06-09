import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'time'
})
export class TimePipe implements PipeTransform {

  transform(items: any[]): string {
    if (!items) return;
    var name = '';
    var seconds = 0;
    var minute = 0;
    var mm = 0;
    var ss = 0;
    var hh = 0;

    items.forEach(element => {
      if (element.type != 2) {
        let firstLine = element.estimatedTime.toString().substring(0, 2);
        let secondLine = element.estimatedTime.toString().substring(2, element.estimatedTime.toString().length);
        seconds += parseInt(secondLine);
        minute += parseInt(firstLine);
      } else {
        minute += parseInt(element.testTime);
      }
    });

    if (seconds >= 60) {
      ss = Math.floor(seconds / 60);
      seconds = seconds % 60;

      minute += ss;
    }

    if (minute >= 60) {
      mm = Math.floor(minute / 60);
      minute = minute % 60;
      hh = mm;
    }

    if (hh >= 0) {
      if (hh > 10) {
        name += hh + " : ";
      } else {
        name += "0" + hh + " : ";
      }
    }

    if (minute >= 0) {
      if (minute > 10) {
        name += minute + " : ";
      } else {
        name += "0" + minute + " : ";
      }
    }

    if (seconds >= 0) {
      if (seconds > 10) {
        name += seconds;
      } else {
        name += "0" + seconds;
      }
    }

    return name;
  }

}
