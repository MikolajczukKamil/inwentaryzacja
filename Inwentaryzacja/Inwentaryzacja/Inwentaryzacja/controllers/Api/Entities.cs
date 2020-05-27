using Inwentaryzacja.Models;
using System;

namespace Inwentaryzacja.Controllers.Api
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
        public char letter;
        public string name;
    }

    public class RoomEntity
    {
        public int id;
        public string name;
        public BuildingEntity building;
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
        public UserEntity owner;
        public RoomEntity room;
    }

    public class ReportPositionEntity
    {
        public AssetEntity asset;
        public RoomEntity previous_room;
        public bool present;
    }

    public class UserEntity
    {
        public int id;
        public string login;
    }
}
