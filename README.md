# SQ7MRU.QSOCollector
Service for Collect and Manage QSOs (ADIF Records)

## Public API
<br/>
#### GET Stations <br/>
GET /api/stations<br/>
<br/>
#### Get Station Info <br/>
GET /api/stations/{stationId}<br/>
<br/>
#### Get Stations Logs <br/>
GET /api/stations/{stationId}/log<br/>
<br/>
#### Get Station Log Item <br/>
GET /api/stations/{stationId}/log/{qsoId}<br/>
<br/>
## Restriction API<br/>
<br/>
#### Insert Station's Item <br/>
POST /restriction/station<br/>
return inserted Item or Error<br/>
<br/>
#### Update Station's Item <br/>
PUT /restricted/stations/{stationId}<br/>
return updated Item or Error<br/>
<br/>
#### Delete Station's Item <br/>
DELETE /restricted/stations/{stationId}<br/>
DELETE /restricted/stations/{stationId}/force - recursive delete<br/>
return true/false<br/>
<br/>
#### Insert Station's Log Item<br/>
POST /restricted/stations/{stationId}/insert/qso<br/>
return inserted Item or Error<br/>
<br/>
#### Insert Station's Log Item from Adif Record<br/>
POST /restricted/stations/{stationId}/insert/adif/{minutesAccept}<br/>
return inserted Item or Error<br/>
optional parameter : int minutesAccept, default = 10 (checks duplicates into Station's Log) <br/>
<br/>                      
#### Update Station's Log Item<br/>
PUT /restricted/stations/{stationId}/log/{qsoId}<br/>
return updated Item or Error<br/>
<br/>
#### Delete Station's Log Item<br/>
DELETE /restriction/station/{id}/log/{id}<br/>
return true/false<br/>


