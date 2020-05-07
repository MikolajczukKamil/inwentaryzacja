
  /* Assets with rooms */

    SELECT
      *,
      getRoomIdWithAsset(assets.id) AS IamInRoomId
    FROM
      asset
    ;
  
  /* getReportsHeaders */

  CALL getReportsHeaders();

  /* getAssetsInReport(n) */

  CALL getAssetsInReport(1);
  CALL getAssetsInReport(2);

  /* getAssetsInRoom(n) */
  
  CALL getAssetsInRoom(1);
  CALL getAssetsInRoom(2);
  