# SQ7MRU.QSOCollector
Service for Collect and Manage QSOs (ADIF Records)

## Public API

#### GET Stations 
GET /api/stations

#### Get Stations Info
GET /api/station/{id}

#### Get Stations Logs
GET /api/station/{id}/log

#### Get Station Log Item
GET /api/station/{id}/log/{id}

## Restriction API

#### Insert Station Item
POST /restriction/station
return inserted Item or Error

#### Update Station Item
PUT /restriction/station/{id}
return updated Item or Error

#### Delete Station Item
DELETE /restriction/station/{id}
DELETE /restriction/station/{id}/force - recursive delete
return true/false

#### Insert Station's Log Item
POST /restriction/station/{id}/log
return inserted Item or Error

#### Update Station's Log Item
PUT /restriction/station/{id}/log/{id}
return updated Item or Error

#### Delete Station's Log Item
DELETE /restriction/station/{id}/log/{id}
return true/false


