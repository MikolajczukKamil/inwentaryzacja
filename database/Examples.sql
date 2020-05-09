
  /* Function - Assets with rooms */

    SELECT
      *,
      getRoomIdWithAsset(assets.id) AS IamInRoomId
    FROM
      asset
    ;
  
  /* 
    getReportsHeaders(user_id INT): {
      id INT
      name VARCHAR
      create_date DATETIME
      owner_id INT
      owner_name VARCHAR
      room_name VARCHAR
      building_name VARCHAR 
    }
  */

  CALL getReportsHeaders(1);
  
  /* 
    getReportHeader(report_id INT): {
      id INT
      name VARCHAR
      create_date DATETIME
      owner_id INT
      owner_name VARCHAR
      room_name VARCHAR
      building_name VARCHAR 
    }
  */

  CALL getReportHeader(1);

  /* 
    getAssetsInReport(report_id INT): {
      asset_id INT
      previous_room INT
      present BOOLEAN
      asset_type INT
      asset_type_name VARCHAR
    }
  */

  CALL getAssetsInReport(1);
  CALL getAssetsInReport(2);

  /* 
    getAssetsInRoom(room_id INT): {
      id INT
      type INT
      asset_type_name VARCHAR
      new_asset BOOLEAN
      moved BOOLEAN
      moved_from_id INT|NULL
      moved_from_name VARCHAR|NULL
    }
  */
  
  CALL getAssetsInRoom(1);
  CALL getAssetsInRoom(2);

  /* 
    getAssetInfo(asset_id INT): {
      id INT
      type INT
      asset_type_name VARCHAR
      room_id INT
      room_name VARCHAR
      building_name VARCHAR
    }
  */

  CALL getAssetInfo(1);
  
  /* 
  getUser(user_id INT): {
    id INT
    login VARCHAR
    hash VARCHAR
  } */

  CALL getUser(1);

  /* 
  getLoginSession(user_token VARCHAR): {
    id INT
    user_id INT
    expiration_date DATETIME
    token VARCHAR
    expired BOOLEAN
  }
  */

  CALL getLoginSession('fake-token');

  /* 
    addLoginSession(
      user_id INT, 
      expiration_date DATETIME, 
      user_token VARCHAR
    ): { id INT } 
  */

  CALL addLoginSession(1, NOW(), 'fake-token-2');
  
  /* deleteLoginSession(user_token VARCHAR): VOID */

  CALL deleteLoginSession('fake-token-2');

  /* 
    addNewReport(
      report_name VARCHAR,
      report_room INT,
      report_owner INT,
      report_positions VARCHAR( JSON( { id INT, previous: INT|NULL, present: BOOLEAN } ) )
    ): { id INT }  
  */

  call addNewReport('nowy', 3, 1, '[{"id":25,"previous":null,"present":1}]');

  /* addRoom(room_name VARCHAR, building_id INT): { id INT } */

  CALL addRoom('3/1', 1);

  /* 
    getRooms(building_id INT): {
      id INT
      name VARCHAR
      building_id INT
      building_name VARCHAR
    }
  */

  CALL getRooms(1);

  /* getBuildings(): { id INT, name VARCHAR } */

  CALL getBuildings();
