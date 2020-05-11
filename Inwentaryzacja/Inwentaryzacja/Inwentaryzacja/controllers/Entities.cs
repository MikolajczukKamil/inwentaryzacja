using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.controllers
{
    public class AssetEntity
    {
        public int id;
        public string name;
        public int assetType;
    }

    public class AssetTypeEntity
    {
        public int id;
        public string letter;
        public string name;
    }

    public class BuildingEntity
    {
        public int id;
        public string name;
    }

    public class ReportEntity
    {
        public int id;
        public string name;
        public int room;
        public DateTime create_date;
        public int owner;
    }

    public class ReportWithAssetEntity
    {
        public int id;
        public string name;
        public int room;
        public DateTime create_date;
        public int owner;
        public ReportAssetsEntity[] assets;

        public class ReportAssetsEntity
        {
            public int report_id;
            public int asset_id;
            public int previous_room;
        }
    }

    public class RoomEntity
    {
        public int id;
        public string name;
        public int building;
    }
}
