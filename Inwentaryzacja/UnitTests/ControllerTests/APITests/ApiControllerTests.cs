using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Moq;

namespace UnitTests.ApiTests
{
    [TestFixture]
    class ApiControllerTests
    {
        private APIController apiController;
        [SetUp]
        public void Setup()
        {
            apiController = new APIController();
        }

        [TestCase(1, 1, 'c', "komputer")]
        [TestCase(5, 5, 's', "stół")]
        public async Task GetAssetInfoTest_CheckAssetType(int id, int typeId, char typeLetter, string typeName)
        {
            AssetInfoEntity assetInfo = await apiController.getAssetInfo(id);
            AssetTypeEntity assetType = new AssetTypeEntity { id = typeId, letter = typeLetter, name = typeName };
            Assert.AreEqual(assetType, assetInfo.type);
        }

        [TestCase(36)]
        [TestCase(101)]
        public async Task GetAssetInfoTest_NotExist(int id)
        {
            AssetInfoEntity assetInfo = await apiController.getAssetInfo(id);
            Assert.AreEqual(id, assetInfo.id);
        }

        [TestCase(1, 1, "3/6", 1, "b 34")]
        [TestCase(7, 2, "3/40", 1, "b 34")]
        public async Task GetAssetInfoTest_CheckRoom(int id, int roomId, string roomName, int buildingId, string buildingName)
        {
            //check room for assets: 1) with no privious room
            //                       2) with previous room
            //                       3) with room with id=0 (assetId 5,6)
            //                       4) without room
            AssetInfoEntity assetInfoEntity = await apiController.getAssetInfo(id);
            BuildingEntity buildingEntity = new BuildingEntity { id = buildingId, name = buildingName };
            RoomEntity roomEntity = new RoomEntity { id = roomId, name = roomName, building = buildingEntity };
            Assert.AreEqual(roomEntity, assetInfoEntity.room);
        }

        [TestCase(5)]
        [TestCase(6)]
        [TestCase(19)]
        [TestCase(24)]
        public async Task GetAssetInfoTest_AssetWithoutRoom(int id)
        {
            AssetInfoEntity assetInfoEntity = await apiController.getAssetInfo(id);
            Assert.AreEqual(null, assetInfoEntity.room);
        }

        public async Task CreateAssetTest()
        {
            AssetPrototype assetPrototype = new AssetPrototype(new AssetType(3, "monitor", 'm'));
            Assert.AreEqual(true, await apiController.CreateAsset(assetPrototype));
        }
        [Test]
        public async Task GetAssetInfoTest()
        {
            AssetInfoEntity assetInfoEntity = await apiController.getAssetInfo(1);
            Assert.AreEqual(1, assetInfoEntity.room.id);
        }
    }
}