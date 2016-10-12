using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using ZebraApiClient.Exceptions;
using ZebraApiClient.Models;


[assembly: InternalsVisibleTo("ZebraApiClientTests")]
namespace ZebraApiClient
{
    public class ZebraApiClient
    {
        //Huge thanks to Peter Elsayeh at Straboga for making this API public
        //If you want to consume it write to peter(at)happihacking(dot)com and ask 
        //for token and documentation
        private static string _token = "My_Token"; //Replace this with what you get from Peter
        private static string _baseurl = "https://straboga.com/zebra/";
        private static string _nextMove = "next_move";
        private static string _scores = "scores";
        private readonly HttpClient _client;

        public ZebraApiClient(string token)
        {
            _client = new HttpClient();
            if (string.IsNullOrWhiteSpace(_token))
                throw new ZebraApiException("You must provide an API token");
            _token = token;
        }

        public ZebraResult GetZebraResult(string movelist, ZebraSettings settings)
        {
            _client.BaseAddress = new Uri(_baseurl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = BuildUrl(movelist, settings);
            var result = GetZebraResultAsync<ZebraResult>(url);

            return GetAsyncZebraResult(result);
        }

        public ZebraResults GetZebraResultForBoard(string board, ZebraPlayer player, ZebraSettings settings)
        {
            _client.BaseAddress = new Uri(_baseurl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = BuildUrl(board, player, settings);
            var result = GetZebraResultAsync<ZebraResults>(url);
            return GetAsyncZebraResult<ZebraResults>(result);
        }

        private static T GetAsyncZebraResult<T>(Task<T> result)
        {
            result.Wait(3000);
            if (result.IsCompleted)
                return result.Result;
            //if (result.IsCanceled)
            //    return new T() {Status = "canceled"};
            //if (result.IsFaulted)
            //    return new T() { Status = "faulted" };
            return (T)Activator.CreateInstance(typeof(T));
        }


        private async Task<T> GetZebraResultAsync<T>(string url)
        {
            
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }
            return (T)Activator.CreateInstance(typeof(T), new object []
            {
                "error"
            });
        }

        internal static string BuildUrl(string movelist, ZebraSettings settings, string token = null)
        {
            //https://straboga.com/zebra/next_move?token=TOKEN&randomness=0.9&moves=c4e3f6e6f5c5&bsd=25&wsd=25&bed=10&wed=10&bwld=10&wwld=10

            UriBuilder builder = new UriBuilder(_baseurl + _nextMove);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["token"] = token ?? _token;
            parameters["moves"] = movelist;
            AddParametersForSettings(settings, parameters);
            builder.Query = parameters.ToString();
            return builder.ToString();
        }

        internal static string BuildUrl(string board, ZebraPlayer player, ZebraSettings settings, string token = null)
        {
            //https://straboga.com/zebra/scores?token=TOKEN&randomness=0.9&board=XO---XXX-OOO-OOO-OOOOOO---OOXO---OOXOOO-OOXOOOOOXXXXX--XXXXXXX--&player=0&bsd=25&wsd=25&bed=10&wed=10&bwld=10&wwld=10
            
            UriBuilder builder = new UriBuilder(_baseurl + _scores);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["token"] = token ?? _token;
            parameters["board"] = board;
            parameters["player"] = ((int)player).ToString();
            AddParametersForSettings(settings, parameters);
            builder.Query = parameters.ToString();
            return builder.ToString();
        }
        private static void AddParametersForSettings(ZebraSettings settings, NameValueCollection parameters)
        {
            parameters["randomness"] = settings.Randomness.ToString("0.0", CultureInfo.InvariantCulture);
            parameters["bsd"] = settings.SearchDepth.ToString("0");
            parameters["wsd"] = settings.SearchDepth.ToString("0");
            parameters["bed"] = settings.ExactDepth.ToString("0");
            parameters["wed"] = settings.ExactDepth.ToString("0");
            parameters["bwld"] = settings.WinLoseDrawDepth.ToString("0");
            parameters["wwld"] = settings.WinLoseDrawDepth.ToString("0");
        }
    }
}
