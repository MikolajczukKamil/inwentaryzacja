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
    class ScannigRegionTests
    {
        private APIController apiController;
        [SetUp]
        public async Task Setup()
        {
            apiController = new APIController();
            await apiController.LoginUser("user1", "111");
        }
        [Test]
        public async Task getAssetsInRoomTest_CorrectId2Ass()
        {
            AssetEntity[] assetEntityArr = await apiController.getAssetsInRoom(1);

            AssetEntity[] expected = new AssetEntity[2];
            expected[0] = new AssetEntity { id = 1, type = new AssetTypeEntity { id = 1, letter = 'c', name = "komputer" } };
            expected[1] = new AssetEntity { id = 3, type = new AssetTypeEntity { id = 3, letter = 'm', name = "monitor" } };

            Assert.AreEqual(expected, assetEntityArr);
        }
        [Test]
        public async Task getAssetsInRoomTest_CorrectId4Ass()
        {
            AssetEntity[] assetEntityArr = await apiController.getAssetsInRoom(2);

            AssetEntity[] expected = new AssetEntity[4];
            expected[0] = new AssetEntity { id = 9, type = new AssetTypeEntity { id = 3, letter = 'm', name = "monitor" } };
            expected[1] = new AssetEntity { id = 10, type = new AssetTypeEntity { id = 4, letter = 'p', name = "projektor" } };
            expected[2] = new AssetEntity { id = 11, type = new AssetTypeEntity { id = 5, letter = 's', name = "stół" } };
            expected[3] = new AssetEntity { id = 12, type = new AssetTypeEntity { id = 6, letter = 't', name = "tablica" } };

            Assert.AreEqual(expected, assetEntityArr);
        }
        [TestCase(500)]
        [TestCase(123)]
        [TestCase(-1)]
        public async Task getAssetsInRoomTest_NonexistentId(int room_id)
        {
            AssetEntity[] assetEntityArr = await apiController.getAssetsInRoom(room_id);
            Assert.AreEqual(null, assetEntityArr);
        }
        [Test]
        public async Task getAssetsInRoomTest_CorrectIdNoAss()
        {
            AssetEntity[] assetEntityArr = await apiController.getAssetsInRoom(7);

            AssetEntity[] expected = new AssetEntity[0];

            Assert.AreEqual(expected, assetEntityArr);
        }

        [Test]
        public async Task GetScansTest()
        {
            var mockRoom = new RoomEntity { id = 1, name = "3/6", building = new BuildingEntity { id = 1, name = "b 34" } };
            var mockOwner = new UserEntity { id = 1, login = "user1" };
            var expected = new ScanEntity[] { new ScanEntity { id = 1, room = mockRoom, owner = mockOwner, create_date = new DateTime(2020, 6, 4, 9, 12, 9) } };
            Assert.AreEqual(expected, await apiController.getScans());
        }

        [TestCase(2, true)]
        [TestCase(32, false)]
        public async Task AddScanTest(int roomID, bool status)
        {
            var scanPrototype = new ScanPrototype(roomID);
            bool created = await apiController.addScan(scanPrototype) > 0;
            Assert.AreEqual(status, created);
        }

        [Test]
        public async Task AddScanTest_RoomNotExists() => Assert.AreEqual(-1, await apiController.addScan(new ScanPrototype(32)));

        [TestCase(4, true)]
        [TestCase(134, false)]
        public async Task DeleteScanTest(int scanID, bool status) => Assert.AreEqual(status, await apiController.deleteScan(scanID));

        [TestCase(5, 1, 1, 6, 0, true)]
        [TestCase(5, 1, 1, 11, 0, true)]
        [TestCase(18, 1, 1, 6, 0, false)]
        [TestCase(6, 1, 1, 83, 0, false)]
        [TestCase(6, 1, 1, 7, 7, false)]
        public async Task UpdateScanTest(int scanID, int assetID1, int state1, int assetID2, int state2, bool status)
        {
            ScanPositionPropotype[] positionPropotypes = new ScanPositionPropotype[] {
                new ScanPositionPropotype(assetID1, state1),
                new ScanPositionPropotype(assetID2, state2) };
            var scanPositionPrototype = new ScanUpdatePropotype(scanID, positionPropotypes);
            Assert.AreEqual(status, await apiController.updateScan(scanPositionPrototype));
        }
        [Test]
        public async Task UpdateScanTest_EmptyPositionsArray() => Assert.AreEqual(false, await apiController.updateScan(new ScanUpdatePropotype(5, new ScanPositionPropotype[0])));
    }
}
