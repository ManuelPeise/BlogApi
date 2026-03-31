import { Component, computed } from '@angular/core';
import { LoadingService } from '../../../services/LoadingService ';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  templateUrl: './loading-spinner.html',
  styleUrls: ['./loading-spinner.scss'],
  imports: [],
})
export class LoadingSpinner {
  readonly loading;

  constructor(private loadingService: LoadingService) {
    this.loading = this.loadingService.loading;
  }
}
