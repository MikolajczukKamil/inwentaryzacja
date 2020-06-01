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
    class AssetsRegionTests
    {
        private APIController apiController;
        [SetUp]
        public async Task Setup()
        {
            apiController = new APIController();
            await apiController.LoginUser("user1", "111");
        }

        [TestCase(1, 1, 'c', "komputer")]
        [TestCase(5, 5, 's', "stół")]
        [TestCase(83, 3, 'm', "monitor")]
        public async Task GetAssetInfoTest_CheckAssetType(int id, int typeId, char typeLetter, string typeName)
        {
            AssetInfoEntity assetInfo = await apiController.getAssetInfo(id);
            AssetTypeEntity assetType = new AssetTypeEntity { id = typeId, letter = typeLetter, name = typeName };
            Assert.AreEqual(assetType, assetInfo.type);
        }

        [TestCase(91)]
        [TestCase(101)]
        public async Task GetAssetInfoTest_NotExist(int id)
        {
            AssetInfoEntity assetInfo = await apiController.getAssetInfo(id);
            Assert.AreEqual(null, assetInfo);
            //Expected error message
        }

        [TestCase(1, 1, "3/6", 1, "b 34")]
        [TestCase(19, 3, "3/19", 1, "b 34")]
        [TestCase(5, 0, null, 0, null)]
        public async Task GetAssetInfoTest_CheckRoom(int id, int roomId, string roomName, int buildingId, string buildingName)
        {
            AssetInfoEntity assetInfoEntity = await apiController.getAssetInfo(id);
            BuildingEntity buildingEntity = new BuildingEntity { id = buildingId, name = buildingName };
            RoomEntity roomEntity = new RoomEntity { id = roomId, name = roomName, building = buildingEntity };
            Assert.AreEqual(roomEntity, assetInfoEntity.room);
        }

        [TestCase(25, 0, null, 0, null)]
        [TestCase(83, 0, null, 0, null)]
        public async Task GetAssetInfoTest_NotInAnyReport(int id, int roomId, string roomName, int buildingId, string buildingName)
        {
            AssetInfoEntity assetInfoEntity = await apiController.getAssetInfo(id);
            BuildingEntity buildingEntity = new BuildingEntity { id = buildingId, name = buildingName };
            RoomEntity roomEntity = new RoomEntity { id = roomId, name = roomName, building = buildingEntity };
            Assert.AreEqual(roomEntity, assetInfoEntity.room);
        }
        [Test]
        public async Task CreateAssetTest_CorrectAssetTypeData()
        {
            AssetPrototype assetPrototype = new AssetPrototype(new AssetType(3, "monitor", 'm'));
            Assert.AreEqual(true, await apiController.CreateAsset(assetPrototype));
        }
        [Test]
        public async Task CreateAssetTest_WrongAssetTypeName()
        {
            AssetPrototype assetPrototype = new AssetPrototype(new AssetType(3, "TV", 'm'));
            Assert.AreEqual(false, await apiController.CreateAsset(assetPrototype));
        }
        [Test]
        public async Task CreateAssetTest_WrongAssetTypeId()
        {
            AssetPrototype assetPrototype = new AssetPrototype(new AssetType(11, "TV", 't'));
            Assert.AreEqual(false, await apiController.CreateAsset(assetPrototype));
        }
    }
}