using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.controllers
{
    public class AssetEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int assetType { get; set; }
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
    public class ReportEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int room { get; set; }
        public DateTime create_date { get; set; }
        public int owner { get; set; }
    }
    public class ReportWithAssetEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int room { get; set; }
        public DateTime create_date { get; set; }
        public int owner { get; set; }
        public List<ReportAssetsEntity> assets;

        public class ReportAssetsEntity
        {
            public int report_id { get; set; }
            public int asset_id { get; set; }
            public int previous_room { get; set; }
        }
    }
    public class RoomEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int building { get; set; }
    }
    public class ReportAssetsEntity
    {
        public int count { get; set; }
        public List<ReportAssetEntity> report_assets;

        public class ReportAssetEntity
        {
            public int report_id { get; set; }
            public int asset_id { get; set; }
            public int previous_room { get; set; }
        }
    }
}
