using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.controllers
{
    public class AssetInfoEntity
    {
        public int id;
        public AssetTypeEntity type;
        public RoomEntity room;
    }

    public class AssetTypeEntity
    {
        public int id;
        public string letter;
        public string name;
    }

    public class RoomEntity
    {
        public int id;
        public string name;
        public RoomEntity building;
    }

    public class BuildingEntity
    {
        public int id;
        public string name;
    }

    public class ReportHeaderEntity
    {
        public int id;
        public string name;
        public DateTime create_date;
        public int owner_id;
        public string owner_name;
        public string room_name;
        public string building_name;
    }

    public class ReportPositionEntity
    {
        public int id;
        public int type;
        public string asset_type_name;
        public bool new_asset;
        public bool moved;
        public int moved_from_id;
        public string moved_from_name;
        public int previous_room;
        public bool present;
    }
}
