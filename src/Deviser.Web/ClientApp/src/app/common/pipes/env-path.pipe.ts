import { Pipe, PipeTransform, Inject } from '@angular/core';
import { WINDOW } from '../services/window.service';
import { PageContext } from '../domain-types/page-context';


@Pipe({
  name: 'envPath'
})
export class EnvPathPipe implements PipeTransform {

  private pageContext: PageContext;

  constructor(@Inject(WINDOW) private window: any) {
    this.pageContext = window.pageContext;
  }

  transform(value: string, args?: any): any {
    if (this.pageContext.isEmbedded) {
      return `${window.location.origin}${this.pageContext.assetPath}/${value}`;
    }

    return `../../${value}`;
  }
}