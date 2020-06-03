using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Moq;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.ControllerTests.APITests
{
    [TestFixture]
    class ReportRegionTests
    {
        private APIController apiController;
        [SetUp]
        public async Task Setup()
        {
            apiController = new APIController();
            await apiController.LoginUser("user1", "111");
        }
        //GetReportHeaders Tests
        [Test]
        public async Task GetReportHeaders_Correct()
        {
            ReportHeaderEntity[] reportHeaderEntities = await apiController.getReportHeaders();
            for (int i = 0; i < reportHeaderEntities.Length; i++)
            {
                Assert.AreEqual(reportHeaderEntities[i], await apiController.getReportHeader(reportHeaderEntities[i].id));
            }
        }
        //GetReportHeader Tests
        [Test]
        [TestCase(1, 1, "Raport 1", 1, 1, "2020-05-23 14:14:13", "user1", "3/6", "b 34", 1)]
        [TestCase(4, 4, "Raport 4", 3, 1, "2020-05-26 14:14:13", "user1", "3/19", "b 34", 1)]
        [TestCase(6, 6, "Raport 6", 4, 1, "2020-05-28 14:14:13", "user1", "1/2", "b 4", 2)]
        public async Task GetReportHeader_CorrectID(int id, int staticId, string staticName, int staticRoomId, int staticOwnerId, string staticDate, string staticLogin, string staticRoomName, string staticBuildingName, int staticBuildingId)
        {
            ReportHeaderEntity reportHeaderEntity = await apiController.getReportHeader(id);
            ReportHeaderEntity tempEntity = new ReportHeaderEntity();
            tempEntity.id = staticId;
            tempEntity.name = staticName;
            tempEntity.create_date = DateTime.Parse(staticDate);

            RoomEntity tempRoomEntity = new RoomEntity();
            tempRoomEntity.id = staticRoomId;
            tempRoomEntity.name = staticRoomName;
            BuildingEntity tempBuildingEntity = new BuildingEntity();
            tempBuildingEntity.id = staticBuildingId;
            tempBuildingEntity.name = staticBuildingName;
            tempRoomEntity.building = tempBuildingEntity;
            tempEntity.room = tempRoomEntity;

            UserEntity tempUserEntity = new UserEntity();
            tempUserEntity.id = staticOwnerId;
            tempUserEntity.login = staticLogin;
            tempEntity.owner = tempUserEntity;

            Assert.AreEqual(tempEntity, reportHeaderEntity);
        }

        [Test]
        [TestCase(-1)]
        [TestCase('1')]
        [TestCase(null)]
        public async Task GetReportHeader_NotNumberID(int id)
        {
            ReportHeaderEntity reportHeaderEntity = await apiController.getReportHeader(id);
            Assert.AreEqual(null, reportHeaderEntity);
        }

        [Test]
        [TestCase(701)]
        [TestCase(1103)]
        [TestCase(2137)]
        public async Task GetReportHeader_NotExistID(int id)
        {
            ReportHeaderEntity reportHeaderEntity = await apiController.getReportHeader(id);
            Assert.AreEqual(null, reportHeaderEntity);
        }
        //GetReportHeader Tests
        [Test]
        [TestCase(3, new int[] { 1, 7, 2, 8, 3, 4, 5, 6 }, new int[] { 1, 1, 2, 2, 3, 4, 5, 6 }, new string[] { "komputer", "komputer" , "krzesło", "krzesło","monitor","projektor","stół","tablica" }, new char[] {'c', 'c', 'k', 'k', 'm', 'p', 's', 't' }, new int[] { 1, 2, 1, 2, 1, 1, 1, 1 }, new int[] { 1, 1, 1, 1, 1, 1, 0, 0 }, new string[] { "3/6", "3/40", "3/6", "3/40", "3/6", "3/6", "3/6", "3/40" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }, new string[] { "b 34", "b 34", "b 34", "b 34", "b 34", "b 34", "b 34", "b 34" })]
        public async Task GetReportPositions_CorrectIdRoomWithoutNulls(int ReportId, int[] assetId, int[] assetTypeId, string[] assetName, char[] assetLetter, int[] prevRoom, int[] presentRoom, string[] roomName, int[] buildingId, string[] buildingName)
        {
            ReportPositionEntity[] reportPositionEntities = await apiController.getReportPositions(ReportId);
            for (int i = 0; i < assetId.Length; i++)
            {
                ReportPositionEntity tempReportPositionEntity = new ReportPositionEntity();
                tempReportPositionEntity.present = Convert.ToBoolean(presentRoom[1]);
                AssetEntity tempAssetEntity = new AssetEntity();
                tempAssetEntity.id = assetId[1];
                AssetTypeEntity assetType = new AssetTypeEntity();
                assetType.id = assetTypeId[1];
                assetType.name = assetName[1];
                assetType.letter = assetLetter[1];
                tempAssetEntity.type = assetType;
                tempReportPositionEntity.asset = tempAssetEntity;
                RoomEntity tempRoomEntity = new RoomEntity();
                tempRoomEntity.id = prevRoom[1];
                tempRoomEntity.name = roomName[1];

                BuildingEntity tempBuildingEntity = new BuildingEntity();
                tempBuildingEntity.id = buildingId[1];
                tempBuildingEntity.name = buildingName[1];
                tempRoomEntity.building = tempBuildingEntity;
                tempReportPositionEntity.previous_room = tempRoomEntity;

                Assert.AreEqual(tempReportPositionEntity, reportPositionEntities[1]);
                //Assert.AreEqual(reportPositionEntities[0], tempReportPositionEntity);
            }
        }
        [Test]
        [TestCase(-1)]
        [TestCase('1')]
        [TestCase(null)]
        public async Task GetReportPositions_NotNumberID(int id)
        {
            ReportPositionEntity[] reportPositionEntity = await apiController.getReportPositions(id);
            Assert.AreEqual(null, reportPositionEntity);
        }

        [Test]
        [TestCase(700)]
        [TestCase(103)]
        [TestCase(2137)]
        public async Task GetReportPositions_NotExistID(int id)
        {
            ReportPositionEntity[] reportPositionEntity = await apiController.getReportPositions(id);
            Assert.AreEqual(null, reportPositionEntity);
        }
        //createReport tests
        [Test]
        [TestCase("Report testowy",1,"3/6",1,"b 34", 1,1,"komputer",'k', 4, "1/2", 2, "b 4", true)]
        public async Task createReport_CorrectData_OneAsset(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int assetId, int assetTypeId, string assetTypeName, char assetLetter, int PreviousRoomId, string PreviousRoomName, int PreviousBuildingId, string PreviousBuildingName, bool present)
        {
            Room room = new Room(CurrentRoomId, CurrentRoomName, new Building(CurrentBuildingId, CurrentBuildingName));
            ReportPositionPrototype positionPrototyp = new ReportPositionPrototype(new Asset(assetId, new AssetType(assetTypeId, assetTypeName, assetLetter)), new Room(PreviousRoomId,PreviousRoomName, new Building(PreviousBuildingId, PreviousBuildingName)), present);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp });
            Assert.AreEqual(true, await apiController.createReport(reportPrototype));
        }

        [Test]
        [TestCase("Report testowy - CorrectData TwoAssets", 1, "3/6", 1, "b 34", new int[] { 1,3 }, new int[] { 1,3 }, new string[] { "komputer", "monitor" }, new char[] { 'k','m' },new int[] {1,4},new string[] {"3/6","1/2"},new int[] {1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]

        public async Task createReport_CorrectData_TwoAsset(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, int[] PreviousRoomId, string[] PreviousRoomName, int[] PreviousBuildingId, string[] PreviousBuildingName, bool[] present) 
        { 
            Room room = new Room(CurrentRoomId, CurrentRoomName, new Building(CurrentBuildingId, CurrentBuildingName));
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new Asset(assetId[0], new AssetType(assetTypeId[0], assetTypeName[0], assetLetter[0])), new Room(PreviousRoomId[0], PreviousRoomName[0], new Building(PreviousBuildingId[0], PreviousBuildingName[0])), present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new Asset(assetId[1], new AssetType(assetTypeId[1], assetTypeName[1], assetLetter[1])), new Room(PreviousRoomId[1], PreviousRoomName[1], new Building(PreviousBuildingId[1], PreviousBuildingName[1])), present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1,positionPrototyp2 });
            Assert.AreEqual(true, await apiController.createReport(reportPrototype));
        }

        [Test]
        [TestCase("Report testowy", 1, "3/6", 1, "b 34", new int[] { 1, 3 }, new int[] { }, new string[] { }, new char[] { }, new bool[] { })]
        public async Task createReport_CorrectData_ZeroAsset(string reportName, int roomId, string roomName, int buildingId, string buildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, bool[] present)
        {
            Room room = new Room(roomId, roomName, new Building(buildingId, buildingName));
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] {});
            Assert.AreEqual(false, await apiController.createReport(reportPrototype));
        }

        [Test]
        [TestCase("", 1, "3/6", 1, "b 34", new int[] { 1, 3 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'k', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]

        public async Task createReport_WrongData_EmptyReportName(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, int[] PreviousRoomId, string[] PreviousRoomName, int[] PreviousBuildingId, string[] PreviousBuildingName, bool[] present)
        {
            Room room = new Room(CurrentRoomId, CurrentRoomName, new Building(CurrentBuildingId, CurrentBuildingName));
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new Asset(assetId[0], new AssetType(assetTypeId[0], assetTypeName[0], assetLetter[0])), new Room(PreviousRoomId[0], PreviousRoomName[0], new Building(PreviousBuildingId[0], PreviousBuildingName[0])), present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new Asset(assetId[1], new AssetType(assetTypeId[1], assetTypeName[1], assetLetter[1])), new Room(PreviousRoomId[1], PreviousRoomName[1], new Building(PreviousBuildingId[1], PreviousBuildingName[1])), present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1, positionPrototyp2 });
            Assert.AreEqual(false, await apiController.createReport(reportPrototype));
        }

        [Test]
        [TestCase("Report testowy - WrongData No Room", null, "3/6", 1, "b 34", new int[] { 1, 3 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'k', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]

        public async Task createReport_WrongData_NoCurrentRoom(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, int[] PreviousRoomId, string[] PreviousRoomName, int[] PreviousBuildingId, string[] PreviousBuildingName, bool[] present)
        {
            Room room = new Room(CurrentRoomId, CurrentRoomName, new Building(CurrentBuildingId, CurrentBuildingName));
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new Asset(assetId[0], new AssetType(assetTypeId[0], assetTypeName[0], assetLetter[0])), new Room(PreviousRoomId[0], PreviousRoomName[0], new Building(PreviousBuildingId[0], PreviousBuildingName[0])), present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new Asset(assetId[1], new AssetType(assetTypeId[1], assetTypeName[1], assetLetter[1])), new Room(PreviousRoomId[1], PreviousRoomName[1], new Building(PreviousBuildingId[1], PreviousBuildingName[1])), present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1, positionPrototyp2 });
            Assert.AreEqual(false, await apiController.createReport(reportPrototype));
        }

        [Test]
        [TestCase("Report testowy - WrongData No Exist Room", 2202, "3/6", 1, "b 34", new int[] { 1, 3 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'k', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]

        public async Task createReport_WrongData_NoExistCurrentRoom(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, int[] PreviousRoomId, string[] PreviousRoomName, int[] PreviousBuildingId, string[] PreviousBuildingName, bool[] present)
        {
            Room room = new Room(CurrentRoomId, CurrentRoomName, new Building(CurrentBuildingId, CurrentBuildingName));
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new Asset(assetId[0], new AssetType(assetTypeId[0], assetTypeName[0], assetLetter[0])), new Room(PreviousRoomId[0], PreviousRoomName[0], new Building(PreviousBuildingId[0], PreviousBuildingName[0])), present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new Asset(assetId[1], new AssetType(assetTypeId[1], assetTypeName[1], assetLetter[1])), new Room(PreviousRoomId[1], PreviousRoomName[1], new Building(PreviousBuildingId[1], PreviousBuildingName[1])), present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1, positionPrototyp2 });
            Assert.AreEqual(false, await apiController.createReport(reportPrototype));
        }

        [Test]
        [TestCase("Report testowy - WrongData Wrong Asset ID", 1, "3/6", 1, "b 34", new int[] { 1330, 3 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'k', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]
        [TestCase("Report testowy - WrongData Wrong Asset ID", 1, "3/6", 1, "b 34", new int[] { 1, 32020 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'k', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]
        [TestCase("Report testowy - WrongData Wrong Asset ID", 1, "3/6", 1, "b 34", new int[] { 1330, 3011 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'k', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]

        public async Task createReport_WrongData_WrongAssetId(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, int[] PreviousRoomId, string[] PreviousRoomName, int[] PreviousBuildingId, string[] PreviousBuildingName, bool[] present)
        {
            Room room = new Room(CurrentRoomId, CurrentRoomName, new Building(CurrentBuildingId, CurrentBuildingName));
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new Asset(assetId[0], new AssetType(assetTypeId[0], assetTypeName[0], assetLetter[0])), new Room(PreviousRoomId[0], PreviousRoomName[0], new Building(PreviousBuildingId[0], PreviousBuildingName[0])), present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new Asset(assetId[1], new AssetType(assetTypeId[1], assetTypeName[1], assetLetter[1])), new Room(PreviousRoomId[1], PreviousRoomName[1], new Building(PreviousBuildingId[1], PreviousBuildingName[1])), present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1, positionPrototyp2 });
            Assert.AreEqual(false, await apiController.createReport(reportPrototype));
        }

        [Test]
        [TestCase("Report testowy - WrongData PreviousIsNullPresentIsFalse", 1, "3/6", 1, "b 34", new int[] { 1, 3 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'k', 'm' }, new bool[] { false, false })]

        public async Task createReport_WrongData_PreviousIsNullPresentIsFalse(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, bool[] present)
        {
            Room room = new Room(CurrentRoomId, CurrentRoomName, new Building(CurrentBuildingId, CurrentBuildingName));
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new Asset(assetId[0], new AssetType(assetTypeId[0], assetTypeName[0], assetLetter[0])), null, present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new Asset(assetId[1], new AssetType(assetTypeId[1], assetTypeName[1], assetLetter[1])), null, present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1, positionPrototyp2 });
            Assert.AreEqual(false, await apiController.createReport(reportPrototype));
        }

        [Test]
        [TestCase("Report testowy", 1, "3/6", 1, "b 34", 1, 1, "komputer", 'k', 4, "1/2", 2, "b 4", true)]
        public async Task createReport_WrongData_PreviousIsNullPresentIsTrue(string reportName, int currentRoomId, string currentRoomName, int currentBuildingId, string currentBuildingName, int assetId, int assetTypeId, string assetTypeName, char assetLetter, int previousRoomId, string previousRoomName, int previousBuildingId, string previousBuildingName, bool present)
        {
            Room CurrentRoom = new Room(currentRoomId, currentRoomName, new Building(currentBuildingId, currentBuildingName));
            ReportPositionPrototype positionPrototype = new ReportPositionPrototype(new Asset(assetId, new AssetType(assetTypeId, assetTypeName, assetLetter)), null, present);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, CurrentRoom, new ReportPositionPrototype[] { positionPrototype });
            Assert.AreEqual(true, await apiController.createReport(reportPrototype));
        }
    }
}
