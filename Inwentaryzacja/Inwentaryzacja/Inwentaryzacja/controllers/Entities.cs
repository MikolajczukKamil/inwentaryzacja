using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.controllers
{
    public class AssetInfoEntity
    {
        public int id { get; set; }
        public AssetTypeEntity type;
        public RoomEntity room;
    }
    public class AssetTypeEntity
    {
        public int id { get; set; }
        public string letter { get; set; }
        public string name { get; set; }
    }
    public class BuildingEntity
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class RoomEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public BuildingEntity building;
    }
    
    public class AssetInRoom
    { 
        public int id { get; set; }
        public int type { get; set; }
        public string asset_type_name { get; set; }
        public bool? new_asset { get; set; }
        public bool? moved { get; set; }
        public int? moved_from_id { get; set; }
        public string moved_from_name { get; set; }
        public int? previous_room { get; set; }
        public bool? present { get; set; }

    }

    public class ReportHeaderEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTimeWithZone create_date { get; set; }
        public int owner_id { get; set; }
        public string owner_name { get; set; }
        public string room_name { get; set; }
        public string building_name { get; set; }

        public class DateTimeWithZone
        {
            public DateTime date { get; set; }
            public int timezone_type { get; set; }
            public string timezone { get; set; }
        }
    }
}
