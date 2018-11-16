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
import { AdifRow } from '../models/adif-row';
import { CheckDupRequest } from '../models/check-dup-request';
@Injectable({
  providedIn: 'root',
})
class RestrictedService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @param station undefined
   */
  RestrictedStationsPostResponse(station?: Station): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = station;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/restricted/stations`,
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
   * @param station undefined
   */
  RestrictedStationsPost(station?: Station): Observable<null> {
    return this.RestrictedStationsPostResponse(station).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param params The `RestrictedService.RestrictedStationsByStationIdPutParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `station`:
   */
  RestrictedStationsByStationIdPutResponse(params: RestrictedService.RestrictedStationsByStationIdPutParams): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    __body = params.station;
    let req = new HttpRequest<any>(
      'PUT',
      this.rootUrl + `/restricted/stations/${params.stationId}`,
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
   * @param params The `RestrictedService.RestrictedStationsByStationIdPutParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `station`:
   */
  RestrictedStationsByStationIdPut(params: RestrictedService.RestrictedStationsByStationIdPutParams): Observable<null> {
    return this.RestrictedStationsByStationIdPutResponse(params).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param stationId undefined
   */
  RestrictedStationsByStationIdDeleteResponse(stationId: number): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    let req = new HttpRequest<any>(
      'DELETE',
      this.rootUrl + `/restricted/stations/${stationId}`,
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
  RestrictedStationsByStationIdDelete(stationId: number): Observable<null> {
    return this.RestrictedStationsByStationIdDeleteResponse(stationId).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param stationId undefined
   */
  RestrictedStationsByStationIdForceDeleteResponse(stationId: number): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    let req = new HttpRequest<any>(
      'DELETE',
      this.rootUrl + `/restricted/stations/${stationId}/force`,
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
  RestrictedStationsByStationIdForceDelete(stationId: number): Observable<null> {
    return this.RestrictedStationsByStationIdForceDeleteResponse(stationId).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param params The `RestrictedService.RestrictedStationsByStationIdInsertQsoPostParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `qso`:
   */
  RestrictedStationsByStationIdInsertQsoPostResponse(params: RestrictedService.RestrictedStationsByStationIdInsertQsoPostParams): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    __body = params.qso;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/restricted/stations/${params.stationId}/insert/qso`,
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
   * @param params The `RestrictedService.RestrictedStationsByStationIdInsertQsoPostParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `qso`:
   */
  RestrictedStationsByStationIdInsertQsoPost(params: RestrictedService.RestrictedStationsByStationIdInsertQsoPostParams): Observable<null> {
    return this.RestrictedStationsByStationIdInsertQsoPostResponse(params).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param params The `RestrictedService.RestrictedStationsByStationIdInsertAdifByMinutesAcceptPostParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `minutesAccept`:
   *
   * - `adifRow`:
   */
  RestrictedStationsByStationIdInsertAdifByMinutesAcceptPostResponse(params: RestrictedService.RestrictedStationsByStationIdInsertAdifByMinutesAcceptPostParams): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;


    __body = params.adifRow;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/restricted/stations/${params.stationId}/insert/adif/${params.minutesAccept}`,
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
   * @param params The `RestrictedService.RestrictedStationsByStationIdInsertAdifByMinutesAcceptPostParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `minutesAccept`:
   *
   * - `adifRow`:
   */
  RestrictedStationsByStationIdInsertAdifByMinutesAcceptPost(params: RestrictedService.RestrictedStationsByStationIdInsertAdifByMinutesAcceptPostParams): Observable<null> {
    return this.RestrictedStationsByStationIdInsertAdifByMinutesAcceptPostResponse(params).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param params The `RestrictedService.RestrictedStationsByStationIdLogByQsoIdPutParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `qsoId`:
   *
   * - `qso`:
   */
  RestrictedStationsByStationIdLogByQsoIdPutResponse(params: RestrictedService.RestrictedStationsByStationIdLogByQsoIdPutParams): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;


    __body = params.qso;
    let req = new HttpRequest<any>(
      'PUT',
      this.rootUrl + `/restricted/stations/${params.stationId}/log/${params.qsoId}`,
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
   * @param params The `RestrictedService.RestrictedStationsByStationIdLogByQsoIdPutParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `qsoId`:
   *
   * - `qso`:
   */
  RestrictedStationsByStationIdLogByQsoIdPut(params: RestrictedService.RestrictedStationsByStationIdLogByQsoIdPutParams): Observable<null> {
    return this.RestrictedStationsByStationIdLogByQsoIdPutResponse(params).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param params The `RestrictedService.StationsByStationIdLogByQsoIdDeleteParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `qsoId`:
   */
  StationsByStationIdLogByQsoIdDeleteResponse(params: RestrictedService.StationsByStationIdLogByQsoIdDeleteParams): Observable<StrictHttpResponse<null>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;


    let req = new HttpRequest<any>(
      'DELETE',
      this.rootUrl + `/stations/${params.stationId}/log/${params.qsoId}`,
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
   * @param params The `RestrictedService.StationsByStationIdLogByQsoIdDeleteParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `qsoId`:
   */
  StationsByStationIdLogByQsoIdDelete(params: RestrictedService.StationsByStationIdLogByQsoIdDeleteParams): Observable<null> {
    return this.StationsByStationIdLogByQsoIdDeleteResponse(params).pipe(
      __map(_r => _r.body as null)
    );
  }

  /**
   * @param params The `RestrictedService.RestrictedStationsByStationIdCheckDupPostParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `checkDupRequest`:
   *
   * @return Success
   */
  RestrictedStationsByStationIdCheckDupPostResponse(params: RestrictedService.RestrictedStationsByStationIdCheckDupPostParams): Observable<StrictHttpResponse<boolean>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    __body = params.checkDupRequest;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/restricted/stations/${params.stationId}/check_dup`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'text'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return (_r as HttpResponse<any>).clone({ body: (_r as HttpResponse<any>).body === 'true' }) as StrictHttpResponse<boolean>
      })
    );
  }
  /**
   * @param params The `RestrictedService.RestrictedStationsByStationIdCheckDupPostParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `checkDupRequest`:
   *
   * @return Success
   */
  RestrictedStationsByStationIdCheckDupPost(params: RestrictedService.RestrictedStationsByStationIdCheckDupPostParams): Observable<boolean> {
    return this.RestrictedStationsByStationIdCheckDupPostResponse(params).pipe(
      __map(_r => _r.body as boolean)
    );
  }

  /**
   * @param params The `RestrictedService.RestrictedStationsByStationIdGetRecordPostParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `callSign`:
   *
   * @return Success
   */
  RestrictedStationsByStationIdGetRecordPostResponse(params: RestrictedService.RestrictedStationsByStationIdGetRecordPostParams): Observable<StrictHttpResponse<AdifRow>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;

    __body = params.callSign;
    let req = new HttpRequest<any>(
      'POST',
      this.rootUrl + `/restricted/stations/${params.stationId}/get_record`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      __filter(_r => _r instanceof HttpResponse),
      __map((_r) => {
        return _r as StrictHttpResponse<AdifRow>;
      })
    );
  }
  /**
   * @param params The `RestrictedService.RestrictedStationsByStationIdGetRecordPostParams` containing the following parameters:
   *
   * - `stationId`:
   *
   * - `callSign`:
   *
   * @return Success
   */
  RestrictedStationsByStationIdGetRecordPost(params: RestrictedService.RestrictedStationsByStationIdGetRecordPostParams): Observable<AdifRow> {
    return this.RestrictedStationsByStationIdGetRecordPostResponse(params).pipe(
      __map(_r => _r.body as AdifRow)
    );
  }
}

module RestrictedService {

  /**
   * Parameters for RestrictedStationsByStationIdPut
   */
  export interface RestrictedStationsByStationIdPutParams {
    stationId: number;
    station?: Station;
  }

  /**
   * Parameters for RestrictedStationsByStationIdInsertQsoPost
   */
  export interface RestrictedStationsByStationIdInsertQsoPostParams {
    stationId: number;
    qso?: Qso;
  }

  /**
   * Parameters for RestrictedStationsByStationIdInsertAdifByMinutesAcceptPost
   */
  export interface RestrictedStationsByStationIdInsertAdifByMinutesAcceptPostParams {
    stationId: number;
    minutesAccept: number;
    adifRow?: AdifRow;
  }

  /**
   * Parameters for RestrictedStationsByStationIdLogByQsoIdPut
   */
  export interface RestrictedStationsByStationIdLogByQsoIdPutParams {
    stationId: number;
    qsoId: number;
    qso?: Qso;
  }

  /**
   * Parameters for StationsByStationIdLogByQsoIdDelete
   */
  export interface StationsByStationIdLogByQsoIdDeleteParams {
    stationId: number;
    qsoId: number;
  }

  /**
   * Parameters for RestrictedStationsByStationIdCheckDupPost
   */
  export interface RestrictedStationsByStationIdCheckDupPostParams {
    stationId: number;
    checkDupRequest?: CheckDupRequest;
  }

  /**
   * Parameters for RestrictedStationsByStationIdGetRecordPost
   */
  export interface RestrictedStationsByStationIdGetRecordPostParams {
    stationId: number;
    callSign?: string;
  }
}

export { RestrictedService }
