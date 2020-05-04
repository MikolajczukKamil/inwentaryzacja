using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.controllers
{
    class AssetEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int assetType { get; set; }
    }
    class AssetTypeEntity
    {
        public int id { get; set; }
        public string letter { get; set; }
        public string name { get; set; }
    }
    class BuildingEntity
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    class ReportEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int room { get; set; }
        public DateTime create_date { get; set; }
        public int owner { get; set; }
    }
    class ReportWithAssetEntity
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
    class RoomEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int building { get; set; }
    }
}
