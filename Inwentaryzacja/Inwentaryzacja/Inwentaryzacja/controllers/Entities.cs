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
        public string name { get; set; }
        public int room { get; set; }
        public List<ReportAssetsEntity> assets;

        public ReportWithAssetEntity(string name, int room, int[] assets)
        {
            this.name = name;
            this.room = room;
            this.assets = new List<ReportAssetsEntity>();
            foreach (var item in assets)
            {
                this.assets.Add(new ReportAssetsEntity(item));
            }
        }
        public class ReportAssetsEntity
        {
            public ReportAssetsEntity(int asset_id)
            {
                this.asset_id = asset_id;
            }

            public int asset_id { get; set; }
        }
    }
    class RoomEntity
    {
        public int id { get; set; }
        public string name { get; set; }
        public int building { get; set; }
    }
}
