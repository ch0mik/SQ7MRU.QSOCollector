# SQ7MRU.QSOCollector
Service for Collect and Manage QSOs (ADIF Records)

## Public API

#### GET Stations 
GET /api/stations

#### Get Station Info
GET /api/stations/{stationId}

#### Get Stations Logs
GET /api/stations/{stationId}/log

#### Get Station Log Item
GET /api/stations/{stationId}/log/{qsoId}

## Restriction API

#### Insert Station's Item
POST /restriction/station
return inserted Item or Error

#### Update Station's Item
PUT /restricted/stations/{stationId}
return updated Item or Error

#### Delete Station's Item
DELETE /restricted/stations/{stationId}
DELETE /restricted/stations/{stationId}/force - recursive delete
return true/false

#### Insert Station's Log Item
POST /restricted/stations/{stationId}/insert/qso
return inserted Item or Error

#### Insert Station's Log Item from Adif Record
POST /restricted/stations/{stationId}/insert/adif/{minutesAccept}
return inserted Item or Error
optional parameter :  int minutesAccept, default = 10 
                      checks duplicates into Station's Log

#### Update Station's Log Item
PUT /restricted/stations/{stationId}/log/{qsoId}
return updated Item or Error

#### Delete Station's Log Item
DELETE /restriction/station/{id}/log/{id}
return true/false


