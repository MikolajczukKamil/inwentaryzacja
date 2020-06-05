using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Moq;
using NUnit.Framework.Internal;
using System.Collections.Specialized;
using Inwentaryzacja.views.view_chooseRoom;
using System.Reflection.Metadata;
using System.Linq;

namespace UnitTests.ControllerTests.APITests
{
    [TestFixture]
    class LocalizationRegionTests
    {
        private APIController apiController;
        [SetUp]
        public async Task Setup()
        {
            apiController = new APIController();
            await apiController.LoginUser("user1", "111");
        }

        [TestCase("test 111", true)]
        [TestCase("test 222", true)]
        [TestCase("b 34", false)]
        [TestCase("b 4", false)]
        [TestCase("", false)]
        public async Task CreateBuildingTest(string name, bool status)
        {
            BuildingPrototype buildingPrototype = new BuildingPrototype(name);
            bool result = await apiController.createBuilding(buildingPrototype) > 0;
            Assert.AreEqual(status, result);
        }

        [TestCase(1, "b 34")]
        [TestCase(2, "b 4")]
        [TestCase(9, "b 32")]
        public async Task GetRoomsTest_PositiveTest(int id, string name)
        {
            bool check = true;
            RoomEntity[] roomEntity = await apiController.getRooms(id);
            BuildingEntity buildingEntity = new BuildingEntity { id = id, name = name };
            foreach (var item in roomEntity)
            {
                if (Equals(item.building, buildingEntity) == false)
                {
                    check = false;
                }
            }
            Assert.IsTrue(check);
        }

        [TestCase(1, "b 31")]
        [TestCase(2, "b 3")]
        public async Task GetRoomsTest_NegativeTest(int id, string name)
        {
            bool check = true;
            RoomEntity[] roomEntity = await apiController.getRooms(id);
            BuildingEntity buildingEntity = new BuildingEntity { id = id, name = name };
            foreach (var item in roomEntity)
            {
                if (Equals(item.building, buildingEntity) == false)
                {
                    check = false;
                }
            }
            Assert.IsFalse(check);
        }

        [TestCase(1, new int[] {1, 2, 3}, new string[] { "3/6", "3/40", "3/19"})]
        [TestCase(3, new int[] { 5 }, new string[] { "1/3" })]
        [TestCase(10, new int[] { }, new string[] { })]
        public async Task GetRoomsTest_ExpectedValues(int id, int[] roomIDs, string[] roomNames)
        {
            bool check = true;
            RoomEntity[] roomEntity = await apiController.getRooms(id);
            if(roomIDs.Length==0|| roomNames.Length==0|| roomEntity.Count() == 0)
            {
                if (roomIDs.Length == roomNames.Length && roomIDs.Length== roomEntity.Count()){}
                else check = false;
            }
            else if (roomIDs.Length != roomNames.Length && roomIDs.Length != roomEntity.Count())
            {
                check = false;
            }
            else
            {
                int i = 0;
                foreach (var item in roomEntity)
                {
                    if (item.id != roomIDs[i] || item.name != roomNames[i])
                    {
                        check = false;
                    }
                    i++;
                }
            }
            Assert.IsTrue(check);
        }

        [TestCase(10)]
        public async Task GetRoomsTest_IsEmptyBuildingTest(int id)
        {
            RoomEntity[] roomEntity = await apiController.getRooms(id);
            Assert.IsEmpty(roomEntity);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetRoomsTest_IsNotEmptyBuildingTest(int id)
        {
            RoomEntity[] roomEntity = await apiController.getRooms(id);
            Assert.IsNotEmpty(roomEntity);
        }


        [TestCase(99)]
        public async Task GetRoomsTest_NonExistentBuilding(int id)
        {
            RoomEntity[] roomEntity = await apiController.getRooms(id);
            Assert.AreEqual(null,roomEntity);
        }


        [TestCase(2, "testRoom12", true)]
        [TestCase(4, "testRoom22", true)]
        [TestCase(1, "3/6", false)]
        [TestCase(2, "1/2", false)]
        [TestCase(99, "testRoom3", false)]
        public async Task CreateRoomTest(int buildingID, string roomName, bool status)
        {
            BuildingEntity buildingEntity = new BuildingEntity { id = buildingID };
            RoomPropotype roomPrototype = new RoomPropotype(roomName, buildingEntity);
            bool result = await apiController.createRoom(roomPrototype) > 0;
            Assert.AreEqual(status, result);
        }

        [Test]
        public async Task CreateRoomTest_NoBuilding()
        {
            BuildingEntity buildingEntity = new BuildingEntity { };
            RoomPropotype roomPrototype = new RoomPropotype("testRoom4", buildingEntity);
            Assert.AreEqual(-1, await apiController.createRoom(roomPrototype));
        }

        [Test]
        public async Task CreateRoomTest_NoRoomName()
        {
            BuildingEntity buildingEntity = new BuildingEntity { id = 2 };
            RoomPropotype roomPrototype = new RoomPropotype("", buildingEntity);
            Assert.AreEqual(-1, await apiController.createRoom(roomPrototype));
        }

        [TestCase(1, "b 34")]
        [TestCase(2, "b 4")]
        [TestCase(6, "b 21")]
        public async Task GetBuildingsTest_BuildingExistsAndMatches(int id, string name)
        {
            BuildingEntity[] buildings = await apiController.getBuildings();
            BuildingEntity building = new BuildingEntity { id = id, name = name };
            foreach (var item in buildings)
            {
                if (item.id==id)
                {
                    Assert.AreEqual(building, item);
                }
            }
        }

        [TestCase(1, "b 35")]
        [TestCase(3, "b 4")]
        [TestCase(6, "b21")]
        public async Task GetBuildingsTest_DoesntExistNorMatches(int id, string name)
        {
            bool check = false;
            BuildingEntity[] buildings = await apiController.getBuildings();
            foreach (var item in buildings)
            {
                if (item.id == id&&item.name==name)
                {
                    check = true;
                }
            }
            Assert.IsFalse(check);
        }
    }
}
