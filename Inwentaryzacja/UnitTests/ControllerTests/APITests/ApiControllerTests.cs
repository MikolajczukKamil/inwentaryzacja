using Inwentaryzacja.Controllers.Api;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        [Test]
        public async Task getAssetInfoTest()
        {
            AssetInfoEntity assetInfoEntity = await apiController.getAssetInfo(1);
            Assert.AreEqual(1,assetInfoEntity.room.id);
        }
    }
}