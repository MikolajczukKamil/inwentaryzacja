using Inwentaryzacja.Models;
using System;
using System.Collections.Generic;

namespace Inwentaryzacja.Controllers.Api
{
    public class AssetEntity
    {
        public int id;
        public AssetTypeEntity type;
        public override bool Equals(object obj)
        {
            if (id == ((AssetEntity)obj).id && type.Equals(((AssetEntity)obj).type))
                return true;
            return false;
        }

        public override int GetHashCode() => id.GetHashCode();
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
        public override bool Equals(object obj)
        {
            if (id == ((AssetTypeEntity)obj).id && letter == ((AssetTypeEntity)obj).letter && name == ((AssetTypeEntity)obj).name)
                return true;
            return false;
        }
        public override int GetHashCode() => id.GetHashCode();
    }

    public class RoomEntity
    {
        public int id;
        public string name;
        public BuildingEntity building;
        public override bool Equals(object obj)
        {
            if (building.Equals(((RoomEntity)obj).building))
            {
                if (id == ((RoomEntity)obj).id && name == ((RoomEntity)obj).name)
                    return true;
            }
            return false;
        }
        public override int GetHashCode() => id.GetHashCode();
    }

    public class BuildingEntity
    {
        public int id;
        public string name;
        public override bool Equals(object obj)
        {
            if (id == ((BuildingEntity)obj).id && name == ((BuildingEntity)obj).name)
                return true;
            return false;
        }
        public override int GetHashCode() => id.GetHashCode();
    }

    public class ReportHeaderEntity
    {
        public int id;
        public string name;
        public DateTime create_date;
        public UserEntity owner;
        public RoomEntity room;
        public override bool Equals(object obj)
        {
            if (id == ((ReportHeaderEntity)obj).id && name == ((ReportHeaderEntity)obj).name && owner.Equals(((ReportHeaderEntity)obj).owner) && room.Equals(((ReportHeaderEntity)obj).room) && create_date.Equals(((ReportHeaderEntity)obj).create_date))
                return true;
            return false;
        }
        public override int GetHashCode() => id.GetHashCode();
    }

    public class ReportPositionEntity
    {
        public AssetEntity asset;
        public RoomEntity previous_room;
        public bool present;
        public override bool Equals(object obj)
        {
            if (asset.Equals(((ReportPositionEntity)obj).asset) && previous_room.Equals(((ReportPositionEntity)obj).previous_room) && present == ((ReportPositionEntity)obj).present)
                return true;
            return false;
        }
        public override int GetHashCode() => asset.GetHashCode() * asset.GetHashCode();
    }

    public class UserEntity
    {
        public int id;
        public string login;
        public override bool Equals(object obj)
        {
            if (id == ((UserEntity)obj).id && login == ((UserEntity)obj).login)
                return true;
            return false;
        }
        public override int GetHashCode() => id.GetHashCode();

    }

    public class AnswerEntity
    {
        public int id;
        public string message;
    }

    public class ScanEntity
    {
        public int id;
        public RoomEntity room;
        public UserEntity owner;
        public DateTime create_date;
        public override bool Equals(object obj)
        {
            if (id == ((ScanEntity)obj).id && room.Equals(((ScanEntity)obj).room) && owner.Equals(((ScanEntity)obj).owner) && create_date.Equals(((ScanEntity)obj).create_date))
                return true;
            return false;
        }

        public override int GetHashCode() => id.GetHashCode();
    }
}
