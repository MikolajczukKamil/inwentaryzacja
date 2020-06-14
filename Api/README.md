# InwentaryzacjaAPI
API for a mobile app student project. We are currently developing a 
mobile app for Android which will be used for stocktaking of assets 
belonging to our university (in theory). I thought of it as a great
opportunity to put my PHP skills to a test and create an API which
will enable us to query an external server's database for data.

The entire project is written in vanilla PHP.

## Endpoints
All the get endpoints are of type GET.  
All the addNew endpoints are of type POST.
-----------------------------------------------------
### Login
* /login/addLoginSession - creates session for a user that has an
account in the database. Returns a token allowing the user
to make requests listed below.
### Asset
* /asset/getAssetInfo/{id} - returns object of type Asset with
 the specified id along with its AssetType, Room and Building
 in JSON format.
* /asset/addNewAsset - when passed complete data it creates
 an Asset object and persists it in the database.
```json
{
  "type": 4 
}
```
 ### Building
 * /building/getBuildings - returns all objects of type Building in JSON
 format.
 * /building/addNewBuilding - when passed complete data it creates
  a Building object and persists it in the database.
```json
{
  "name": "Nowy budynek"
}
```
 * /building/getRooms/{id} - returns all objects of type 
   Room belonging to the building with the specified id in JSON format.
 ### ReportHeader
  * /report/getReportsHeaders - returns all objects of type ReportHeader
   in JSON format.
  * /report/getReportHeader/{id} - returns object of type ReportHeader
   with the specified id in JSON format.
  * /report/addNewReport - when passed complete data it creates
   a Report object and persists it in the database.
```json
{
  "name": "raport testowy 2416",
  "room": 2,
  "assets":
  [
    {
      "id": 2,
      "previous": 1,
      "present": 1
    },
    {
      "id": 3,
      "previous": 1,
      "present": 1
    }
  ]
}
```
  * /report/getReportPositions/{id} - returns all assets
    of type ReportAsset, that were inside the report with the given
    id, in JSON format
  ### Room
  * /room/addNewRoom - when passed complete data it creates
  a Room object and persists it in the database.
```json
{
  "name": "Pokój specjalny 2",
  "building": 1
}
```
  * /room/getAssetsInRoom/{id} - returns all objects
    of type ReportAsset belonging to room with the specified id 
    in JSON format 
  ### Scan
  * /scan/getScans - returns all objects of type Scan in JSON format
  * /scan/addScan - when passed complete data it creates a Scan object and persists it in the database.
```json
{
  "room_id": 3,
  "owner": 5
}
```
  * /scan/deleteScan/{id} - deletes the Scan with the specified id
  * /scan/updateScan - when passed complete data it adds a new Asset to the specified Scan object in the database.
```json
{
  "id": 3,
  "positions": [
  {
    "asset": 1,
    "state": 2
  },
  {
    "asset": 3,
    "state": 1
  }
  ]
}
```
  * /scan/getScanPositions/{id} - returns all assets from scan
  
  ### UserCreator <- Just for testing!
  * /creator/user_creator - creates a User object with 
 the specified login and password and persists it to database.
 This should only be used for testing purposes as it is not 
 a secured endpoint. No token/privileges verification is performed
 upon executing this script.
