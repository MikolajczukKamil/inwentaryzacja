using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.controllers
{
    public class AssetEntity
    {
        public int id;
        public AssetTypeEntity type;
    }

    public class AssetInfoEntity : AssetEntity
    {
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
        public AssetEntity asset;
        public AssetInfoEntity previus;
        public bool present;
    }
}
