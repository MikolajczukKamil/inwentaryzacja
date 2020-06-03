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

        public override int GetHashCode()
        {
            int hashCode = -1056084179;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<AssetTypeEntity>.Default.GetHashCode(type);
            return hashCode;
        }
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
        public override bool Equals(object obj)
        {
            if (id == ((ReportHeaderEntity)obj).id && name == ((ReportHeaderEntity)obj).name && owner.Equals(((ReportHeaderEntity)obj).owner) && room.Equals(((ReportHeaderEntity)obj).room) && create_date.Equals(((ReportHeaderEntity)obj).create_date))
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = -986270442;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + create_date.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<UserEntity>.Default.GetHashCode(owner);
            hashCode = hashCode * -1521134295 + EqualityComparer<RoomEntity>.Default.GetHashCode(room);
            return hashCode;
        }
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

        public override int GetHashCode()
        {
            int hashCode = 198247793;
            hashCode = hashCode * -1521134295 + EqualityComparer<AssetEntity>.Default.GetHashCode(asset);
            hashCode = hashCode * -1521134295 + EqualityComparer<RoomEntity>.Default.GetHashCode(previous_room);
            hashCode = hashCode * -1521134295 + present.GetHashCode();
            return hashCode;
        }
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

        public override int GetHashCode()
        {
            int hashCode = 962899218;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(login);
            return hashCode;
        }
    }
}
