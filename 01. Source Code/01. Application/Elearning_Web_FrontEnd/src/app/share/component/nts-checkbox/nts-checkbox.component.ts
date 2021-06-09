import { Component, OnInit, Input, Output, EventEmitter, forwardRef, ChangeDetectorRef, ViewEncapsulation } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'nts-checkbox',
  templateUrl: './nts-checkbox.component.html',
  styleUrls: ['./nts-checkbox.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => NtsCheckboxComponent),
    multi: true
  }],
  encapsulation: ViewEncapsulation.None
})
export class NtsCheckboxComponent implements OnInit {

  constructor(private _cd: ChangeDetectorRef,) { }

  _isDisabled: boolean;
  _value: any;
  @Input() valueTrue: any;
  @Input() valueFalse: any;

  @Output('change') changeEvent = new EventEmitter();

  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  writeValue(value: any): void {
    this._value = value;
    this._cd.markForCheck();
  }

  registerOnChange(fn: any): void {
    this._onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this._onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this._isDisabled = isDisabled;
    this._cd.markForCheck();
  }

  ngOnInit(): void {
  }

  checkChange($event) {
    if ($event.target.checked) {
      this._value = this.valueTrue;
    } else {
      this._value = this.valueFalse;
    }

    this._onChange(this._value ? this._value : null);
    this.changeEvent.emit(this._value ? this._value : null);
  }

}
