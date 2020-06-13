using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Moq;

namespace UnitTests.ControllerTests.APITests
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

        [TestCase(1, 1, 'c', "komputer", 7, "1/5", 5, "b 7")]
        [TestCase(5, 5, 's', "stół", 7, "1/5", 5, "b 7")]
        [TestCase(19, 1, 'c', "komputer", 4, "1/2", 2, "b 4")]
        public async Task GetAssetInfoTest(int id, int typeId, char typeLetter, string typeName, int roomId, string roomName, int buildingId, string buildingName)
        {
            AssetTypeEntity assetTypeEntity = new AssetTypeEntity { id = typeId, letter = typeLetter, name = typeName };
            BuildingEntity buildingEntity = new BuildingEntity { id = buildingId, name = buildingName };
            RoomEntity roomEntity = new RoomEntity { id = roomId, name = roomName, building = buildingEntity };
            AssetInfoEntity expected = new AssetInfoEntity { id = id, type = assetTypeEntity, room = roomEntity };
            Assert.AreEqual(expected, await apiController.getAssetInfo(id));
        }

        [TestCase(70)]
        [TestCase(101)]
        public async Task GetAssetInfoTest_NotExist(int id)
        {
            AssetInfoEntity assetInfo = await apiController.getAssetInfo(id);
            Assert.AreEqual(null, assetInfo);
        }

        [Test]
        public async Task GetAssetInfoTest_AssetNotInAnyReport()
        {
            AssetInfoEntity expected = new AssetInfoEntity
            {
                id = 33,
                type = new AssetTypeEntity { id = 3, letter = 'm', name = "monitor" },
                room = null
            };
            Assert.AreEqual(expected, await apiController.getAssetInfo(33));
        }
    }
}