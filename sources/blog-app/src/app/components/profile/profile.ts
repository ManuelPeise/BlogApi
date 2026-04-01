import { Component, inject, signal, effect } from '@angular/core';
import { AuthenticationService } from '../../services/AuthenticationService';
import { Formbutton } from '../customComponents/formbutton/formbutton';
import { Textfield } from '../customComponents/textfield/textfield';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { IUserModel } from '../../lib/models/IUserModel';
import { HttpService } from '../../services/HttpService';
import { IResponseModel } from '../../lib/models/IResponse';
import { HttpClient } from '@angular/common/http';
import { LoadingSpinner } from '../customComponents/loading-spinner/loading-spinner';
import i18n from '../../I18nService';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, ReactiveFormsModule, Formbutton, Textfield, LoadingSpinner],
  templateUrl: './profile.html',
  styleUrls: ['./profile.scss'],
})
export class Profile {
  router = inject(Router);
  httpService: HttpService;
  authService = inject(AuthenticationService);

  profileSignal = this.authService.currentUserSignal();
  profileImageSrcSignal = signal(
    this.normalizeImageSrc(this.profileSignal?.profileImage as Blob | string | null | undefined),
  );
  profileForm: FormGroup;
  labels: any;
  private normalizeImageSrc(src: Blob | string | null | undefined): string {
    if (!src) return 'assets/images/no_image.png';
    if (src instanceof Blob) {
      return URL.createObjectURL(src);
    }
    if (typeof src === 'string') {
      if (src.startsWith('data:image/')) return src;
      if (
        src.length > 100 &&
        !src.startsWith('http') &&
        !src.startsWith('/assets/') &&
        !src.startsWith('./') &&
        !src.startsWith('../')
      ) {
        return 'data:image/png;base64,' + src;
      }
      if (src.startsWith('/') && src.length > 100) {
        return 'data:image/png;base64,' + src.substring(1);
      }
      return src;
    }
    return 'assets/images/no_image.png';
  }

  constructor(private formBuilder: FormBuilder) {
    this.loadLabels();
    this.httpService = new HttpService(inject(HttpClient));
    this.profileForm = this.formBuilder.group({
      profileImage: [
        this.normalizeImageSrc(
          this.profileSignal?.profileImage as Blob | string | null | undefined,
        ),
      ],
      firstName: [this.profileSignal?.firstName ?? '', Validators.required],
      lastName: [this.profileSignal?.lastName ?? '', Validators.required],
      email: [this.profileSignal?.email ?? '', [Validators.required, Validators.email]],
      dateOfBirth: [this.profileSignal?.dateOfBirth?.split('T')[0] ?? null],
      street: [this.profileSignal?.address?.street ?? ''],
      houseNumber: [this.profileSignal?.address?.houseNumber ?? ''],
      postalCode: [this.profileSignal?.address?.postalCode ?? ''],
      city: [this.profileSignal?.address?.cityName ?? ''],
      country: [this.profileSignal?.address?.countryName ?? ''],
    });
    this.profileForm.markAsPristine();
  }

  loadLabels() {
    this.labels = {
      captionProfileInformation: i18n.t('captionProfileInformation', { ns: 'common' }),
      captionAddressInformation: i18n.t('captionAddressInformation', { ns: 'common' }),
      labelFirstName: i18n.t('labelFirstName', { ns: 'common' }),
      labelLastName: i18n.t('labelLastName', { ns: 'common' }),
      labelEmail: i18n.t('labelEmail', { ns: 'common' }),
      labelDateOfBirth: i18n.t('labelDateOfBirth', { ns: 'common' }),
      labelStreet: i18n.t('labelStreet', { ns: 'common' }),
      labelHouseNumber: i18n.t('labelHouseNumber', { ns: 'common' }),
      labelPostalCode: i18n.t('labelPostalCode', { ns: 'common' }),
      labelCity: i18n.t('labelCity', { ns: 'common' }),
      labelCountry: i18n.t('labelCountry', { ns: 'common' }),
      labelChangePassword: i18n.t('labelChangePassword', { ns: 'common' }),
      labelSaveChanges: i18n.t('labelSave', { ns: 'common' }),
      labelRevertChanges: i18n.t('labelCancel', { ns: 'common' }),
    };
  }

  onRevertChanges() {
    this.profileForm.patchValue({
      firstName: this.profileSignal?.firstName ?? '',
      lastName: this.profileSignal?.lastName ?? '',
      email: this.profileSignal?.email ?? '',
      profileImage: this.normalizeImageSrc(
        this.profileSignal?.profileImage as Blob | string | null | undefined,
      ),
      street: this.profileSignal?.address?.street ?? '',
      houseNumber: this.profileSignal?.address?.houseNumber ?? '',
      postalCode: this.profileSignal?.address?.postalCode ?? '',
      city: this.profileSignal?.address?.cityName ?? '',
      country: this.profileSignal?.address?.countryName ?? '',
    });
    this.profileImageSrcSignal.set(
      this.normalizeImageSrc(this.profileSignal?.profileImage as Blob | string | null | undefined),
    );
    this.profileForm.markAsPristine();
  }

  onSaveChanges() {
    if (this.profileForm.valid) {
      // Strip data URL prefix if present
      let profileImageValue = this.profileForm.value.profileImage;
      if (typeof profileImageValue === 'string' && profileImageValue.startsWith('data:image/')) {
        profileImageValue = profileImageValue.split(',')[1];
      }
      const updatedProfile: IUserModel = {
        id: this.profileSignal?.id ?? 0,
        firstName: this.profileForm.value.firstName,
        lastName: this.profileForm.value.lastName,
        email: this.profileForm.value.email,
        dateOfBirth: this.profileForm.value.dateOfBirth,
        profileImage: profileImageValue,
        isMarkedAsDeleted: this.profileSignal?.isMarkedAsDeleted ?? false,
        isMarkedAsDeletedAt: this.profileSignal?.isMarkedAsDeletedAt ?? null,
        blogs: this.profileSignal?.blogs ?? [],
        addressId: this.profileSignal?.addressId ?? null,
        address: {
          id: this.profileSignal?.addressId ?? 0,
          street: this.profileForm.value.street,
          houseNumber: this.profileForm.value.houseNumber,
          postalCode: this.profileForm.value.postalCode,
          cityId: this.profileSignal?.address?.cityId ?? 0,
          cityName: this.profileForm.value.city,
          countryId: this.profileSignal?.address?.countryId ?? 0,
          countryName: this.profileSignal?.address?.countryName ?? '',
        },
        createdAt: this.profileSignal?.createdAt ?? '',
        createdBy: this.profileSignal?.createdBy ?? '',
        updatedAt: new Date().toISOString(),
        updatedBy: this.profileSignal?.updatedBy ?? '',
      };

      this.httpService
        .sendRequest<IResponseModel<IUserModel>>('PUT', 'user/updateuser', updatedProfile)
        .subscribe((response) => {
          if (response.success) {
            this.authService.currentUserSignal.set(response.data);

            this.profileImageSrcSignal.set(
              this.normalizeImageSrc(
                response.data?.profileImage as Blob | string | null | undefined,
              ),
            );
            this.profileForm.markAsPristine();
          }
        });
    }
  }

  onChangePassword() {
    this.router.navigate(['/change-password']);
  }

  onChangeProfileImageClick() {
    const inputElement = document.getElementById('profile-image-selector') as HTMLInputElement;
    if (inputElement) {
      inputElement.click();
    }
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];

    if (file) {
      const reader = new FileReader();

      reader.onload = () => {
        const base64 = (reader.result as string).split(',')[1];
        const dataUrl = 'data:image/png;base64,' + base64;
        this.profileForm.patchValue({ profileImage: dataUrl });
        this.profileForm.get('profileImage')?.markAsDirty();
        this.profileImageSrcSignal.set(dataUrl);
      };

      reader.readAsDataURL(file);
    }
  }
}
