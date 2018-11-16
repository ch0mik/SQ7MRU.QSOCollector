/* tslint:disable */
import { Qso } from './qso';
export interface Station {
  stationId?: number;
  name?: string;
  callsign?: string;
  qth?: string;
  locator?: string;
  operator?: string;
  hamID?: string;
  log?: Array<Qso>;
}
