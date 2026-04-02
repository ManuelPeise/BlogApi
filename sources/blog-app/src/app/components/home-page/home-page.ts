import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import i18n from '../../I18nService';
import { AuthenticationService } from '../../services/AuthenticationService';
import { HttpService } from '../../services/HttpService';
import { IBlogMetaData } from '../../lib/models/IBlogMetaData';
import { IResponseModel } from '../../lib/models/IResponse';
import { HttpClient } from '@angular/common/http';

const pageSize = 10;

@Component({
  selector: 'app-home-page',
  imports: [],
  templateUrl: './home-page.html',
  styleUrl: './home-page.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomePage {
  labels: any;
  authService = inject(AuthenticationService);
  httpService: HttpService | null = null;
  blogMetaData = signal<IBlogMetaData[]>([]);
  page: number = 1;

  constructor() {
    this.loadLabels();
    this.httpService = new HttpService(inject(HttpClient));
    this.loadBlogMetaData();
  }

  loadBlogMetaData() {
    this.httpService
      ?.sendRequest<
        IResponseModel<IBlogMetaData[]>
      >('GET', `blog/getpublicblogmetadatacollection?loadPrivate=false&page=${this.page}`)
      .subscribe((response) => {
        if (response.success) {
          this.blogMetaData.set(response.data);
        }
      });
  }

  loadLabels() {
    this.labels = {
      captionHomeUserGreeting: i18n
        .t('captionHomeUserGreeting', {
          ns: 'common',
        })
        .replace(
          '{firstName}',
          this.authService.currentUserSignal()?.firstName ?? i18n.t('labelYou'),
        ),
      labelHomeUserGreetingSlogan: i18n.t('labelHomeUserGreetingSlogan', { ns: 'common' }),
      labelMyBlogs: i18n.t('labelBlogs', { ns: 'common' }),
      labelNoBlogsYet: i18n.t('labelNoPublicBlogs', { ns: 'common' }),
      labelPosts: i18n.t('labelPosts', { ns: 'common' }),
      labelLastPost: i18n.t('labelLastPost', { ns: 'common' }),
    };
  }

  getBlogImageUrl(blog: IBlogMetaData): string {
    const src = blog.image;
    if (!src) return 'assets/images/no_image.png';
    if (src.startsWith('data:image/')) return src;
    return 'data:image/jpeg;base64,' + src;
  }

  formatDate(dateString: string): string {
    return dateString?.split('T')[0];
  }
}
