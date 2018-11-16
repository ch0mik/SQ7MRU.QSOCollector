/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpResponse, HttpHeaders } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { Observable } from 'rxjs';
import { map as __map, filter as __filter } from 'rxjs/operators';

import { Station } from '../models/station';
import { Qso } from '../models/qso';
@Injectable({
  providedIn: 'root',
})
class PublicService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @return Success
   */
  ApiStationsGetResponse(): Observable<StrictHttpResponse<Array<Station>>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      'GET',
      this.rootUrl + `/api/stations`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as StrictHttpResponse<Array<Station>>;
      })
    );
  }
  /**
   * @return Success
   */
  ApiStationsGet(): Observable<Array<Station>> {
    return this.ApiStationsGetResponse().pipe(
      __map(_r => _r.body as Array<Station>)
    );
  }

  /**
   * @param stationId undefined
   */
  ApiStationsByStationIdGetResponse(stationId: number): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    let req = new HttpRequest<any>(
      'GET',
      this.rootUrl + `/api/stations/${stationId}`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as StrictHttpResponse<null>;
      })
    );
  }
  /**
   * @param stationId undefined
   */
  ApiStationsByStationIdGet(stationId: number): Observable<null> {
    return this.ApiStationsByStationIdGetResponse(stationId).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param stationId undefined
   * @return Success
   */
  ApiStationsByStationIdLogGetResponse(stationId: number): Observable<StrictHttpResponse<Array<Qso>>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    let req = new HttpRequest<any>(
      'GET',
      this.rootUrl + `/api/stations/${stationId}/log`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as StrictHttpResponse<Array<Qso>>;
      })
    );
  }
  /**
   * @param stationId undefined
   * @return Success
   */
  ApiStationsByStationIdLogGet(stationId: number): Observable<Array<Qso>> {
    return this.ApiStationsByStationIdLogGetResponse(stationId).pipe(
      __map(_r => _r.body as Array<Qso>)
    );
  }

  /**
   * @param stationId undefined
   */
  ApiStationsByStationIdExportGetResponse(stationId: number): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    let req = new HttpRequest<any>(
      'GET',
      this.rootUrl + `/api/stations/${stationId}/export`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as StrictHttpResponse<null>;
      })
    );
  }
  /**
   * @param stationId undefined
   */
  ApiStationsByStationIdExportGet(stationId: number): Observable<null> {
    return this.ApiStationsByStationIdExportGetResponse(stationId).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param params The `PublicService.ApiStationsByStationIdLogByQsoIdGetParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `qsoId`:
   */
  ApiStationsByStationIdLogByQsoIdGetResponse(params: PublicService.ApiStationsByStationIdLogByQsoIdGetParams): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;


    let req = new HttpRequest<any>(
      'GET',
      this.rootUrl + `/api/stations/${params.stationId}/log/${params.qsoId}`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as StrictHttpResponse<null>;
      })
    );
  }
  /**
   * @param params The `PublicService.ApiStationsByStationIdLogByQsoIdGetParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `qsoId`:
   */
  ApiStationsByStationIdLogByQsoIdGet(params: PublicService.ApiStationsByStationIdLogByQsoIdGetParams): Observable<null> {
    return this.ApiStationsByStationIdLogByQsoIdGetResponse(params).pipe(
      __map(_r => _r.body as null)
    );
  }
}

module PublicService {

  /**
   * Parameters for ApiStationsByStationIdLogByQsoIdGet
   */
  export interface ApiStationsByStationIdLogByQsoIdGetParams {
    stationId: number;
    qsoId: number;
  }
}

export { PublicService }
