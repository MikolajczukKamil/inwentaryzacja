using Inwentaryzacja.Models;
using System;
using System.Collections.Generic;

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
        public override bool Equals(object obj)
        {
            if (id == ((AssetTypeEntity)obj).id && letter == ((AssetTypeEntity)obj).letter && name == ((AssetTypeEntity)obj).name)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            int hashCode = 1186222763;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + letter.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            return hashCode;
        }
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
                if(id == ((RoomEntity)obj).id && name == ((RoomEntity)obj).name)
                    return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            int hashCode = -1737075669;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + EqualityComparer<BuildingEntity>.Default.GetHashCode(building);
            return hashCode;
        }
    }

    public class BuildingEntity
    {
        public int id;
        public string name;
        public override bool Equals(object obj)
        {
            if (id == ((BuildingEntity)obj).id &&  name == ((BuildingEntity)obj).name)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = -48284730;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            return hashCode;
        }
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

    public class ScanEntity
    {
        public int id;
        public RoomEntity room;
        public UserEntity owner;
        public DateTime create_date;
    }
}
