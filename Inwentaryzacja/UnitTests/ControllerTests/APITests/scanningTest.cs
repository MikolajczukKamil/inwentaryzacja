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
    class scannigTests
    {
        private APIController apiController;
        [SetUp]
        public async Task Setup()
        {
            apiController = new APIController();
            await apiController.LoginUser("user1", "111");
        }
        [Test]
        [TestCase(3)]
        public async Task getAssetsInRoomTest(int room_id)
        {
            AssetInfoEntity assetInfoEntity = await apiController.getAssetInfo(1);
            Assert.AreEqual(1, assetInfoEntity.room.id);
        }
    }
}
