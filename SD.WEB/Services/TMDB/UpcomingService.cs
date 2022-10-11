﻿using SD.Shared.Core;
using SD.Shared.Helper;
using SD.Shared.Modal;
using SD.Shared.Modal.Enum;
using SD.Shared.Modal.Tmdb;
using SD.WEB.Core;

namespace SD.WEB.Services.TMDB
{
    public class UpcomingService : IMediaListService
    {
        public async Task PopulateListMedia(HttpClient http, IStorageService storage, Settings settings,
            HashSet<MediaDetail> list_media, MediaType type, int qtd = 9, Dictionary<string, string>? ExtraParameters = null)
        {
            var page = 0;

            var parameter = new Dictionary<string, string>()
            {
                { "api_key", TmdbOptions.ApiKey },
                { "region", settings.Region.ToString() },
                { "language", settings.Language.GetName(false) },
                { "page", page.ToString() }
            };

            if (type == MediaType.movie)
            {
                while (list_media.Count < qtd)
                {
                    page++;
                    parameter["page"] = page.ToString();
                    var result = await http.Get<MovieUpcoming>(TmdbOptions.BaseUri + "movie/upcoming".ConfigureParameters(parameter), storage.Session);

                    foreach (var item in result.results)
                    {
                        //if (string.IsNullOrEmpty(item.poster_path)) continue;

                        list_media.Add(new MediaDetail
                        {
                            tmdb_id = item.id.ToString(),
                            title = item.title,
                            plot = string.IsNullOrEmpty(item.overview) ? "No plot found" : item.overview,
                            release_date = item.release_date.GetDate(),
                            poster_path_small = string.IsNullOrEmpty(item.poster_path) ? null : TmdbOptions.SmallPosterPath + item.poster_path,
                            poster_path_large = string.IsNullOrEmpty(item.poster_path) ? null : TmdbOptions.LargePosterPath + item.poster_path,
                            rating = item.vote_count > 10 ? item.vote_average : 0,
                            MediaType = MediaType.movie
                        });
                    }

                    if (result.total_results < qtd) break; //if there is less result than requested
                    if (page >= result.total_pages) break; //passed the last page
                    if (page > 10) break; //if it exceeds 10 calls, something is wrong
                }
            }
            else if (type == MediaType.tv)
            {
                throw new NotImplementedException();
            }
        }
    }
}