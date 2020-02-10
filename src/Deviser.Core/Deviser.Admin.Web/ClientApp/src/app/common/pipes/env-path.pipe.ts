import { Pipe, PipeTransform, Inject } from '@angular/core';
import { WINDOW } from '../services/window.service';
import { DAConfig } from '../domain-types/da-config';

@Pipe({
  name: 'envPath'
})
export class EnvPathPipe implements PipeTransform {

  private daConfig: DAConfig;

  constructor(@Inject(WINDOW) private window: any) {
    this.daConfig = window.daConfig;
  }

  transform(value: string, args?: any): any {
    if (this.daConfig.isEmbedded) {
      return `${window.location.origin}${this.daConfig.assetPath}/${value}`;
    }

    return `../../${value}`;
  }
}