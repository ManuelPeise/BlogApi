import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Textfield } from '../customComponents/textfield/textfield';
import { Formbutton } from '../customComponents/formbutton/formbutton';
import i18n from '../../I18nService';
import { HttpService } from '../../services/HttpService';
import { HttpClient } from '@angular/common/http';
import { updatePasswordMatchValidator } from '../../lib/validation';
import { IChangePasswordRequest } from './interfaces/IChangePasswordRequest';
import { IResponseModel } from '../../lib/models/IResponse';
import { ITokenData } from '../../lib/models/ITokenData';
import { LocalStorageService } from '../../services/LocalStorageService';
import { LocalStorageEnum } from '../../lib/enums/LocalStorageEnum';
import { AuthenticationService } from '../../services/AuthenticationService';

interface ChangePasswordLabels {
  captionChangePassword: string;
  labelCurrentPassword: string;
  labelNewPassword: string;
  labelConfirmNewPassword: string;
  labelCancel: string;
  labelSave: string;
  errorChangePassword: string;
  labelPasswordChanged: string;
}

@Component({
  selector: 'app-change-password',
  imports: [ReactiveFormsModule, Textfield, Formbutton],
  templateUrl: './change-password.html',
  styleUrl: './change-password.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChangePassword {
  private readonly localStorageService = new LocalStorageService<ITokenData>(
    LocalStorageEnum.AuthState,
  );
  private readonly authenticationService = inject(AuthenticationService);
  private readonly formBuilder = inject(FormBuilder);
  private readonly httpService = new HttpService(inject(HttpClient));

  readonly changePasswordForm = this.formBuilder.group(
    {
      currentPassword: ['', [Validators.required, Validators.minLength(8)]],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(8)]],
    },
    { validators: updatePasswordMatchValidator },
  );

  readonly message = signal<string | null>(null);

  readonly labels: ChangePasswordLabels = {
    captionChangePassword: i18n.t('captionChangePassword', { ns: 'common' }),
    labelCurrentPassword: i18n.t('labelCurrentPassword', { ns: 'common' }),
    labelNewPassword: i18n.t('labelNewPassword', { ns: 'common' }),
    labelConfirmNewPassword: i18n.t('labelConfirmNewPassword', { ns: 'common' }),
    labelCancel: i18n.t('labelCancel', { ns: 'common' }),
    labelSave: i18n.t('labelSave', { ns: 'common' }),
    errorChangePassword: i18n.t('errorChangePassword', { ns: 'common' }),
    labelPasswordChanged: i18n.t('labelPasswordChanged', { ns: 'common' }),
  };

  onCancel(): void {
    this.changePasswordForm.reset();
  }

  onSave(): void {
    if (this.changePasswordForm.valid) {
      this.message.set(null);

      const changePasswordData: IChangePasswordRequest = {
        userId: this.authenticationService.currentUserSignal()?.id ?? 0,
        currentPassword: this.changePasswordForm.value.currentPassword!,
        updatedPassword: this.changePasswordForm.value.newPassword!,
      };

      this.httpService
        .sendRequest<
          IResponseModel<string>
        >('POST', 'authentication/changepassword', changePasswordData)
        .subscribe((response) => {
          if (response.success && response.data?.length > 0) {
            const tokenData: ITokenData = { jwt: response.data };
            this.localStorageService.setItem(tokenData);
            this.message.set(this.labels.labelPasswordChanged);
          } else {
            this.message.set(this.labels.errorChangePassword);
          }
          this.changePasswordForm.reset();
        });
    }
  }
}
