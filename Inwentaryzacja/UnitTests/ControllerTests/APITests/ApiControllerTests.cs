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
        [Test]
        public async Task GetAssetInfoTest()
        {
            AssetInfoEntity assetInfoEntity = await apiController.getAssetInfo(1);
            Assert.AreEqual(1,assetInfoEntity.room.id);
        }
        [Test]
        public async Task CreateAssetTest()
        {
            AssetPrototype assetPrototype = new AssetPrototype(new AssetType(3,"monitor",'m'));
            Assert.AreEqual(true, await apiController.CreateAsset(assetPrototype));
        }
    }
}