using GifExplorer.DTOs;
using GifExplorer.Models;
using GifExplorer.Repository.iRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GifExplorer.Repository
{
    public class GifRepository : IGifRepository
    {
        private ApplicationDbContext _db;
        private string _apikey;
        private string _APIBaseAddress = "https://api.giphy.com/v1/gifs/";

        public GifRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _apikey = configuration["apikey"];
        }

        private string apikey { get { return _apikey; } }

        private void UpdateCache(string ReqURL, string json)
        {
            GifRequestHistory reqHist = _db.GifRequestHistory.FirstOrDefault(r => r.ReqURL == ReqURL);
            if (reqHist == null)
            {
                _db.GifRequestHistory.Add(new GifRequestHistory()
                {
                    ReqURL = ReqURL,
                    ResJSON = json,
                    UpdateDate = DateTime.Now
                });
            }
            else
            {
                reqHist.ResJSON = json;
                reqHist.UpdateDate = DateTime.Now;
            }
            _db.SaveChanges();
        }

        private List<Gif> GetTrendingFromAPI()
        {
            List<Gif> list = new List<Gif>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_APIBaseAddress);
                //HTTP GET
                var responseTask = client.GetAsync($"trending?api_key={apikey}&limit=25&rating=g");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<TrendingAPIDto>();
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    UpdateCache("trending", readTask.Result);

                    dynamic trendingObj = JsonConvert.DeserializeObject(readTask.Result);

                    foreach (var gif in trendingObj.data)
                    {
                        list.Add(new Gif
                        {
                            Title = gif.title,
                            URL = gif.images.downsized.url
                        });
                    }

                }
            }

            return list;
        }

        private List<Gif> SearchFromAPI(string term)
        {
            List<Gif> list = new List<Gif>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_APIBaseAddress);
                //HTTP GET
                var responseTask = client.GetAsync($"search?api_key={apikey}&q={term}&limit=25&offset=0&rating=g&lang=en");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<TrendingAPIDto>();
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    UpdateCache($"search?q={term}", readTask.Result);

                    dynamic trendingObj = JsonConvert.DeserializeObject(readTask.Result);

                    foreach (var gif in trendingObj.data)
                    {
                        list.Add(new Gif
                        {
                            Title = gif.title,
                            URL = gif.images.downsized.url
                        });
                    }

                }
            }

            return list;
        }

        public async Task<List<Gif>> GetTrending()
        {
            List<Gif> list;
            GifRequestHistory reqHist = await _db.GifRequestHistory.FirstOrDefaultAsync(r => r.ReqURL == "trending");
            if (reqHist == null || reqHist.UpdateDate.AddDays(1) < DateTime.Now)
            {
                list = GetTrendingFromAPI();
            } else
            {
                dynamic trendingObj = JsonConvert.DeserializeObject(reqHist.ResJSON);
                list = new List<Gif>();
                foreach (var gif in trendingObj.data)
                {
                    list.Add(new Gif
                    {
                        Title = gif.title,
                        URL = gif.images.downsized.url
                    });
                }
            }

            return list;
        }

        public async Task<List<Gif>> Search(string term)
        {
            List<Gif> list;
            GifRequestHistory reqHist = await _db.GifRequestHistory.FirstOrDefaultAsync(r => r.ReqURL == $"search?q={term}");
            if (reqHist == null || reqHist.UpdateDate.AddDays(1) < DateTime.Now)
            {
                list = SearchFromAPI(term);
            }
            else
            {
                dynamic trendingObj = JsonConvert.DeserializeObject(reqHist.ResJSON);
                list = new List<Gif>();
                foreach (var gif in trendingObj.data)
                {
                    list.Add(new Gif
                    {
                        Title = gif.title,
                        URL = gif.images.downsized.url
                    });
                }
            }

            return list;
        }
    }
}
