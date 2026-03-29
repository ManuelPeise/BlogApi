import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-formbutton',
  imports: [],
  templateUrl: './formbutton.html',
  styleUrls: ['./formbutton.scss'],
  standalone: true,
})
export class Formbutton {
  @Input() label: string = 'Button';
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() disabled: boolean = false;
  @Input() class: string = 'btn btn-primary';

  @Output() onClick = new EventEmitter<void>();

  handleClick() {
    if (!this.disabled) {
      this.onClick.emit();
    }
  }
}
