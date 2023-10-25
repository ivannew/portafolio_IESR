using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using portafolio_IESR.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using portafolio_IESR.Models;

namespace Portafolio_IESR.Controllers
{
    public class VideoController : Controller
    {
        private string key = "AIzaSyDNexATyzkbRRkF3CCoAxf8EJuyy5eum2Q";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(string searchTerm)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = key,
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = searchTerm;
            searchListRequest.MaxResults = 50;

            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<VideoModel> listaVideos = new List<VideoModel>();
            foreach (var searchResult in searchListResponse.Items)
            {
                listaVideos.Add(new VideoModel()
                {
                    VideoId = searchResult.Id.VideoId,
                    Title = searchResult.Snippet.Title,
                    ImageUrl = searchResult.Snippet.Thumbnails.Medium.Url
                });
            }

            return View("Index", listaVideos);
        }
    }
}
