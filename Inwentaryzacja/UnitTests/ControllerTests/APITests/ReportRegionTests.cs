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
        //Tutaj się zmieniają daty w raportach
        [Test]
        [TestCase(1, 1, "Raport 1", 1, 1, "2020-06-03 16:54:55", "user1", "3/6", "b 34", 1)]
        [TestCase(4, 4, "Raport 4", 3, 1, "2020-06-06 16:54:55", "user1", "3/19", "b 34", 1)]
        [TestCase(6, 6, "Raport 6", 4, 1, "2020-06-08 16:54:55", "user1", "1/2", "b 4", 2)]
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
        [TestCase(3, new int[] { 1, 7, 2, 8, 3, 4, 5, 6 }, new int[] { 1, 1, 2, 2, 3, 4, 5, 6 }, new string[] { "komputer", "komputer", "krzesło", "krzesło", "monitor", "projektor", "stół", "tablica" }, new char[] { 'c', 'c', 'k', 'k', 'm', 'p', 's', 't' }, new int[] { 1, 2, 1, 2, 1, 1, 1, 1 }, new int[] { 1, 1, 1, 1, 1, 1, 0, 0 }, new string[] { "3/6", "3/40", "3/6", "3/40", "3/6", "3/6", "3/6", "3/6" }, new int[] { 1, 1, 1, 1, 1, 1, 1, 1 }, new string[] { "b 34", "b 34", "b 34", "b 34", "b 34", "b 34", "b 34", "b 34" })]
        public async Task GetReportPositions_CorrectIdRoomWithoutNulls(int ReportId, int[] assetId, int[] assetTypeId, string[] assetName, char[] assetLetter, int[] prevRoom, int[] presentRoom, string[] roomName, int[] buildingId, string[] buildingName)
        {
            ReportPositionEntity[] reportPositionEntities = await apiController.getReportPositions(ReportId);
            for (int i = 0; i < assetId.Length; i++)
            {
                ReportPositionEntity tempReportPositionEntity = new ReportPositionEntity();
                tempReportPositionEntity.present = Convert.ToBoolean(presentRoom[i]);
                AssetEntity tempAssetEntity = new AssetEntity();
                tempAssetEntity.id = assetId[i];
                AssetTypeEntity assetType = new AssetTypeEntity();
                assetType.id = assetTypeId[i];
                assetType.name = assetName[i];
                assetType.letter = assetLetter[i];
                tempAssetEntity.type = assetType;
                tempReportPositionEntity.asset = tempAssetEntity;
                RoomEntity tempRoomEntity = new RoomEntity();
                tempRoomEntity.id = prevRoom[i];
                tempRoomEntity.name = roomName[i];

                BuildingEntity tempBuildingEntity = new BuildingEntity();
                tempBuildingEntity.id = buildingId[i];
                tempBuildingEntity.name = buildingName[i];
                tempRoomEntity.building = tempBuildingEntity;
                tempReportPositionEntity.previous_room = tempRoomEntity;

                Assert.AreEqual(tempReportPositionEntity, reportPositionEntities[i]);
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
        [TestCase("Report testowy", 1, "3/6", 1, "b 34", 1, 1, "komputer", 'c', 4, "1/2", 2, "b 4", true)]
        [TestCase("Report test", 5, "1/3", 3, "b 5", 13, 1, "komputer", 'c', 1, "3/6", 1, "b 34", true)]
        public async Task createReport_CorrectData_OneAsset(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int assetId, int assetTypeId, string assetTypeName, char assetLetter, int PreviousRoomId, string PreviousRoomName, int PreviousBuildingId, string PreviousBuildingName, bool present)
        {
            RoomEntity room = new RoomEntity { id = CurrentRoomId, name = CurrentRoomName, building = new BuildingEntity { id = CurrentBuildingId, name = CurrentBuildingName } };
            ReportPositionPrototype positionPrototyp = new ReportPositionPrototype(new AssetEntity { id = assetId, type = new AssetTypeEntity { id=assetTypeId, name=assetTypeName, letter=assetLetter } }, new RoomEntity { id = PreviousRoomId, name = PreviousRoomName, building = new BuildingEntity { id = PreviousBuildingId, name = PreviousBuildingName } }, present);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp });
            int resultInt = await apiController.createReport(reportPrototype);
            bool resultBool = false;
            if (resultInt > 0) { resultBool = true; }
            Assert.AreEqual(true, resultBool);
        }

        [Test]
        [TestCase("Report testowy - CorrectData TwoAssets", 1, "3/6", 1, "b 34", new int[] { 1, 3 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'c', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]

        public async Task CreateReport_CorrectData_TwoAsset(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, int[] PreviousRoomId, string[] PreviousRoomName, int[] PreviousBuildingId, string[] PreviousBuildingName, bool[] present)
        {
            RoomEntity room = new RoomEntity { id = CurrentRoomId, name = CurrentRoomName, building = new BuildingEntity { id = CurrentBuildingId, name = CurrentBuildingName } };
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new AssetEntity { id = assetId[0], type = new AssetTypeEntity { id = assetTypeId[0], name = assetTypeName[0], letter = assetLetter[0] } }, new RoomEntity {id=PreviousRoomId[0], name=PreviousRoomName[0], building=new BuildingEntity{id=PreviousBuildingId[0], name=PreviousBuildingName[0]}}, present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new AssetEntity { id = assetId[1], type = new AssetTypeEntity { id = assetTypeId[1], name = assetTypeName[1], letter = assetLetter[1] } }, new RoomEntity { id = PreviousRoomId[1], name = PreviousRoomName[1], building = new BuildingEntity { id = PreviousBuildingId[1], name = PreviousBuildingName[1] } }, present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1,positionPrototyp2 });
            int resultInt = await apiController.createReport(reportPrototype);
            bool resultBool = false;
            if (resultInt > 0) { resultBool = true; }
            Assert.AreEqual(true, resultBool);
        }

        [Test]
        [TestCase("Report testowy", 1, "3/6", 1, "b 34")]
        public async Task CreateReport_WrongData_ZeroAsset(string reportName, int roomId, string roomName, int buildingId, string buildingName)
        {
            RoomEntity room = new RoomEntity { id = roomId, name = roomName, building = new BuildingEntity { id = buildingId, name = buildingName } };
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] {});
            int resultInt = await apiController.createReport(reportPrototype);
            bool resultBool = false;
            if (resultInt > 0) { resultBool = true; }
            Assert.AreEqual(false, resultBool);
        }

        [Test]
        [TestCase("", 1, "3/6", 1, "b 34", new int[] { 1, 3 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'c', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]

        public async Task CreateReport_WrongData_EmptyReportName(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, int[] PreviousRoomId, string[] PreviousRoomName, int[] PreviousBuildingId, string[] PreviousBuildingName, bool[] present)
        {
            RoomEntity room = new RoomEntity { id = CurrentRoomId, name = CurrentRoomName, building = new BuildingEntity { id = CurrentBuildingId, name = CurrentBuildingName } };
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new AssetEntity { id = assetId[0], type = new AssetTypeEntity { id = assetTypeId[0], name = assetTypeName[0], letter = assetLetter[0] } }, new RoomEntity { id = PreviousRoomId[0], name = PreviousRoomName[0], building = new BuildingEntity { id = PreviousBuildingId[0], name = PreviousBuildingName[0] } }, present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new AssetEntity { id = assetId[1], type = new AssetTypeEntity { id = assetTypeId[1], name = assetTypeName[1], letter = assetLetter[1] } }, new RoomEntity { id = PreviousRoomId[1], name = PreviousRoomName[1], building = new BuildingEntity { id = PreviousBuildingId[1], name = PreviousBuildingName[1] } }, present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1, positionPrototyp2 });
            int resultInt = await apiController.createReport(reportPrototype);
            bool resultBool = false;
            if (resultInt > 0) { resultBool = true; }
            Assert.AreEqual(false, resultBool);
        }

        [Test]
        [TestCase("Report testowy - WrongData No Room", null, "3/6", 1, "b 34", new int[] { 1, 3 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'c', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]

        public async Task CreateReport_WrongData_NoCurrentRoom(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, int[] PreviousRoomId, string[] PreviousRoomName, int[] PreviousBuildingId, string[] PreviousBuildingName, bool[] present)
        {
            RoomEntity room = new RoomEntity { id = CurrentRoomId, name = CurrentRoomName, building = new BuildingEntity { id = CurrentBuildingId, name = CurrentBuildingName } };
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new AssetEntity { id = assetId[0], type = new AssetTypeEntity { id = assetTypeId[0], name = assetTypeName[0], letter = assetLetter[0] } }, new RoomEntity { id = PreviousRoomId[0], name = PreviousRoomName[0], building = new BuildingEntity { id = PreviousBuildingId[0], name = PreviousBuildingName[0] } }, present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new AssetEntity { id = assetId[1], type = new AssetTypeEntity { id = assetTypeId[1], name = assetTypeName[1], letter = assetLetter[1] } }, new RoomEntity { id = PreviousRoomId[1], name = PreviousRoomName[1], building = new BuildingEntity { id = PreviousBuildingId[1], name = PreviousBuildingName[1] } }, present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1, positionPrototyp2 });
            int resultInt = await apiController.createReport(reportPrototype);
            bool resultBool = false;
            if (resultInt > 0) { resultBool = true; }
            Assert.AreEqual(false, resultBool);
        }

        [Test]
        [TestCase("Report testowy - WrongData No Exist Room", 2202, "3/6", 1, "b 34", new int[] { 1, 3 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'c', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]

        public async Task CreateReport_WrongData_NoExistCurrentRoom(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, int[] PreviousRoomId, string[] PreviousRoomName, int[] PreviousBuildingId, string[] PreviousBuildingName, bool[] present)
        {
            RoomEntity room = new RoomEntity { id = CurrentRoomId, name = CurrentRoomName, building = new BuildingEntity { id = CurrentBuildingId, name = CurrentBuildingName } };
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new AssetEntity { id = assetId[0], type = new AssetTypeEntity { id = assetTypeId[0], name = assetTypeName[0], letter = assetLetter[0] } }, new RoomEntity { id = PreviousRoomId[0], name = PreviousRoomName[0], building = new BuildingEntity { id = PreviousBuildingId[0], name = PreviousBuildingName[0] } }, present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new AssetEntity { id = assetId[1], type = new AssetTypeEntity { id = assetTypeId[1], name = assetTypeName[1], letter = assetLetter[1] } }, new RoomEntity { id = PreviousRoomId[1], name = PreviousRoomName[1], building = new BuildingEntity { id = PreviousBuildingId[1], name = PreviousBuildingName[1] } }, present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1, positionPrototyp2 });
            int resultInt = await apiController.createReport(reportPrototype);
            bool resultBool = false;
            if (resultInt > 0) { resultBool = true; }
            Assert.AreEqual(false, resultBool);
        }

        [Test]
        [TestCase("Report testowy - WrongData Wrong Asset ID", 1, "3/6", 1, "b 34", new int[] { 1330, 3 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'c', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]
        [TestCase("Report testowy - WrongData Wrong Asset ID", 1, "3/6", 1, "b 34", new int[] { 1, 32020 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'c', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]
        [TestCase("Report testowy - WrongData Wrong Asset ID", 1, "3/6", 1, "b 34", new int[] { 1330, 3011 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'c', 'm' }, new int[] { 1, 4 }, new string[] { "3/6", "1/2" }, new int[] { 1, 2 },
    new string[] { "b 34", "b 4" }, new bool[] { true, true })]

        public async Task CreateReport_WrongData_WrongAssetId(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, int[] PreviousRoomId, string[] PreviousRoomName, int[] PreviousBuildingId, string[] PreviousBuildingName, bool[] present)
        {
            RoomEntity room = new RoomEntity { id = CurrentRoomId, name = CurrentRoomName, building = new BuildingEntity { id = CurrentBuildingId, name = CurrentBuildingName } };
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new AssetEntity { id = assetId[0], type = new AssetTypeEntity { id = assetTypeId[0], name = assetTypeName[0], letter = assetLetter[0] } }, new RoomEntity { id = PreviousRoomId[0], name = PreviousRoomName[0], building = new BuildingEntity { id = PreviousBuildingId[0], name = PreviousBuildingName[0] } }, present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new AssetEntity { id = assetId[1], type = new AssetTypeEntity { id = assetTypeId[1], name = assetTypeName[1], letter = assetLetter[1] } }, new RoomEntity { id = PreviousRoomId[1], name = PreviousRoomName[1], building = new BuildingEntity { id = PreviousBuildingId[1], name = PreviousBuildingName[1] } }, present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1, positionPrototyp2 });
            int resultInt = await apiController.createReport(reportPrototype);
            bool resultBool = false;
            if (resultInt > 0) { resultBool = true; }
            Assert.AreEqual(false, resultBool);
        }

        [Test]
        [TestCase("Report testowy - WrongData PreviousIsNullPresentIsFalse", 1, "3/6", 1, "b 34", new int[] { 1, 3 }, new int[] { 1, 3 }, new string[] { "komputer", "monitor" }, new char[] { 'c', 'm' }, new bool[] { false, false })]

        public async Task CreateReport_WrongData_PreviousIsNullPresentIsFalse(string reportName, int CurrentRoomId, string CurrentRoomName, int CurrentBuildingId, string CurrentBuildingName, int[] assetId, int[] assetTypeId, string[] assetTypeName, char[] assetLetter, bool[] present)
        {
            RoomEntity room = new RoomEntity { id = CurrentRoomId, name = CurrentRoomName, building = new BuildingEntity { id = CurrentBuildingId, name = CurrentBuildingName } };
            ReportPositionPrototype positionPrototyp1 = new ReportPositionPrototype(new AssetEntity { id = assetId[0], type = new AssetTypeEntity { id = assetTypeId[0], name = assetTypeName[0], letter = assetLetter[0] } }, null, present[0]);
            ReportPositionPrototype positionPrototyp2 = new ReportPositionPrototype(new AssetEntity { id = assetId[1], type = new AssetTypeEntity { id = assetTypeId[1], name = assetTypeName[1], letter = assetLetter[1] } }, null, present[1]);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, room, new ReportPositionPrototype[] { positionPrototyp1, positionPrototyp2 });
            int resultInt = await apiController.createReport(reportPrototype);
            bool resultBool = false;
            if (resultInt > 0) { resultBool = true; }
            Assert.AreEqual(false, resultBool);
        }

        [Test]
        [TestCase("Report testowy", 1, "3/6", 1, "b 34", 1, 1, "komputer", 'c', true)]
        [TestCase("Report testowy", 6, "1/4", 4, "b 6", 8, 2, "krzesło", 'k', true)]
        public async Task CreateReport_CorrectData_PreviousIsNullPresentIsTrue(string reportName, int currentRoomId, string currentRoomName, int currentBuildingId, string currentBuildingName, int assetId, int assetTypeId, string assetTypeName, char assetLetter, bool present)
        {
            RoomEntity CurrentRoom = new RoomEntity { id = currentRoomId, name = currentRoomName, building = new BuildingEntity { id = currentBuildingId, name = currentBuildingName } };
            ReportPositionPrototype positionPrototype = new ReportPositionPrototype(new AssetEntity { id = assetId, type = new AssetTypeEntity { id = assetTypeId, name = assetTypeName, letter = assetLetter } }, null, present);
            ReportPrototype reportPrototype = new ReportPrototype(reportName, CurrentRoom, new ReportPositionPrototype[] { positionPrototype });
            int resultInt = await apiController.createReport(reportPrototype);
            bool resultBool = false;
            if (resultInt > 0) { resultBool = true; }
            Assert.AreEqual(true, resultBool);
        }
    }
}
