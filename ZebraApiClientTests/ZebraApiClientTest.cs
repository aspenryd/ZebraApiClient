using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZebraApiClient.Models;

namespace ZebraApiClientTests
{
    [TestClass]
    public class ZebraApiClientTest
    {
        //Huge thanks to Peter Elsayeh at Straboga for making this API public
        //If you want to consume it write to peter(at)happihacking(dot)com and ask 
        //for token and documentation

        private const string FakeToken = "fake_token";
        private const string test_token = "My_Token"; //Replace this with what you get from Peter
            

        [TestMethod]
        public void BuildUrl__ForMoves_withSettings_ReturnsCorrectString()
        {
            var expectedResult =
                "https://straboga.com:443/zebra/next_move?token=fake_token&moves=c4e3f6e6f5c5&randomness=0.9&bsd=25&wsd=25&bed=10&wed=10&bwld=10&wwld=10";
            var movelist = "c4e3f6e6f5c5";
            var settings = new ZebraSettings
            {
                Randomness = 0.9,
                SearchDepth = 25,
                ExactDepth = 10,
                WinLoseDrawDepth = 10
            };
            
            var url = ZebraApiClient.ZebraApiClient.BuildUrl(movelist, settings, FakeToken);
            Assert.AreEqual(expectedResult, url);
        }

        [TestMethod]
        public void BuildUrl_ForMoves_withDefaultSettings_ReturnsCorrectString()
        {
            var expectedResult =
                "https://straboga.com:443/zebra/next_move?token=fake_token&moves=c4e3f6&randomness=0.1&bsd=16&wsd=16&bed=14&wed=14&bwld=20&wwld=20";
            var movelist = "c4e3f6";
            var settings = new ZebraSettings();            
            var url = ZebraApiClient.ZebraApiClient.BuildUrl(movelist, settings, FakeToken);
            Assert.AreEqual(expectedResult, url);
        }
      
        [TestMethod]
        public void GetZebraResult_ForMoveswithDefaultSettings_ReturnsCorrectString()
        {
            var settings = new ZebraSettings();

            var client = new ZebraApiClient.ZebraApiClient(test_token);
            var movelist = "c4e3f4";
            var result = client.GetZebraResult(movelist, settings);
            Assert.IsNotNull(result);
            Assert.AreEqual("ok", result.Status);
            Assert.AreEqual("c5", result.Move);
        }

        [TestMethod]
        public void BuildUrl_ForBoard_withSettings_ReturnsCorrectString()
        {
            var expectedResult =
                "https://straboga.com:443/zebra/scores?token=fake_token&board=XO---XXX-OOO-OOO-OOOOOO---OOXO---OOXOOO-OOXOOOOOXXXXX--XXXXXXX--&player=0&randomness=0.1&bsd=16&wsd=16&bed=14&wed=14&bwld=20&wwld=20";
            var board = "XO---XXX-OOO-OOO-OOOOOO---OOXO---OOXOOO-OOXOOOOOXXXXX--XXXXXXX--";
            var player = ZebraPlayer.Black;
            var settings = new ZebraSettings();
            var url = ZebraApiClient.ZebraApiClient.BuildUrl(board, player, settings, FakeToken);
            Assert.AreEqual(expectedResult, url);
        }


        [TestMethod]
        public void GetZebraResult_ForBoardwithDefaultSettings_ReturnsCorrectString()
        {
            var settings = new ZebraSettings();

            var client = new ZebraApiClient.ZebraApiClient(test_token);
            var board = "XO---XXX-OOO-OOO-OOOOOO---OOXO---OOXOOO-OOXOOOOOXXXXX--XXXXXXX--";
            var player = ZebraPlayer.Black;
            var result = client.GetZebraResultForBoard(board, player, settings);
            
            Assert.IsNotNull(result);
            Assert.AreEqual("ok", result.Status);
            Assert.IsNotNull(result.Result);
            Assert.IsTrue(result.Result.Any());
            var zebraScore = result.Result.First();
            Assert.AreEqual("H5",zebraScore.Move);
            Assert.AreEqual(4, zebraScore.White);
            Assert.AreEqual(60, zebraScore.Black);
        }


    }
}
