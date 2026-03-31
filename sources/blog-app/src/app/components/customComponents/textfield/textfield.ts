import {
  Component,
  Input,
  forwardRef,
  ElementRef,
  ViewChild,
  AfterViewInit,
  OnDestroy,
} from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import flatpickr from 'flatpickr';
import { Instance } from 'flatpickr/dist/types/instance';

@Component({
  selector: 'app-textfield',
  templateUrl: './textfield.html',
  standalone: true,
  styleUrls: ['./textfield.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => Textfield),
      multi: true,
    },
  ],
})
export class Textfield implements ControlValueAccessor, AfterViewInit, OnDestroy {
  @Input() label = '';
  @Input() type = 'text';
  @Input() required = false;
  @Input() disabled = false;
  @Input() className = '';
  @Input() placeHolder = '';

  @ViewChild('inputRef') inputRef!: ElementRef<HTMLInputElement>;

  value: string = '';
  private flatpickrInstance: Instance | null = null;

  onChange = (value: unknown) => {};
  onTouched = () => {};

  ngAfterViewInit(): void {
    if (this.type === 'date') {
      const originalInput = this.inputRef.nativeElement;
      this.flatpickrInstance = flatpickr(originalInput, {
        dateFormat: 'Y-m-d',
        altInput: true,
        altFormat: 'd.m.Y',
        altInputClass: 'form-control custom-textfield',
        allowInput: true,
        defaultDate: this.value || undefined,
        onReady: (_dates, _dateStr, instance) => {
          if (instance.altInput) {
            // Transfer attributes so the label's `for` and placeholder work
            instance.altInput.id = originalInput.id;
            instance.altInput.name = originalInput.name;
            instance.altInput.placeholder = this.placeHolder || this.label;
            originalInput.removeAttribute('id');
          }
        },
        onChange: (_, dateStr) => {
          this.value = dateStr;
          this.onChange(dateStr);
          this.onTouched();
        },
      }) as Instance;
    }
  }

  ngOnDestroy(): void {
    this.flatpickrInstance?.destroy();
  }

  writeValue(value: unknown): void {
    this.value = (value as string) ?? '';
    if (this.flatpickrInstance) {
      this.flatpickrInstance.setDate(this.value, false);
    }
  }

  registerOnChange(fn: (value: unknown) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
    if (this.flatpickrInstance) {
      isDisabled ? this.flatpickrInstance.destroy() : void 0;
    }
  }

  onInput(event: Event): void {
    if (this.type === 'date') return; // handled by flatpickr
    const value = (event.target as HTMLInputElement).value;
    this.value = value;
    this.onChange(value);
  }
}
