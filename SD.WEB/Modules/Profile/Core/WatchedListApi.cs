﻿using Microsoft.Extensions.Caching.Memory;
using SD.Shared.Models.Auth;
using SD.WEB.Shared;

namespace SD.WEB.Modules.Profile.Core
{
    public class WatchedListApi(IHttpClientFactory factory, IMemoryCache memoryCache) : ApiCore<WatchedList>(factory, memoryCache, "WatchedList")
    {
        private struct Endpoint
        {
            public const string Get = "public/watchedlist/get";

            public static string Add(MediaType? type, string TmdbId) => $"watchedlist/add/{type}/{TmdbId}";

            public static string Remove(MediaType? type, string TmdbId) => $"watchedlist/remove/{type}/{TmdbId}";
        }

        public async Task<WatchedList?> Get(bool IsUserAuthenticated, RenderControlCore<WatchedList?>? core, string? id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return await GetAsync($"{Endpoint.Get}?id={id}", core);
            }
            else if (IsUserAuthenticated)
            {
                return await GetAsync(Endpoint.Get, core);
            }
            else
            {
                return new();
            }
        }

        public async Task<WatchedList?> Add(MediaType? mediaType, WatchedList? obj, string? TmdbId, ClientePaddle? paddle)
        {
            ArgumentNullException.ThrowIfNull(mediaType);
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(TmdbId);
            SubscriptionHelper.ValidateWatched(paddle?.ActiveProduct, obj.Items(mediaType).Count + 1);

            return await PostAsync<WatchedList>(Endpoint.Add(mediaType, TmdbId), null, null);
        }

        public async Task<WatchedList?> Remove(MediaType? mediaType, string? TmdbId)
        {
            ArgumentNullException.ThrowIfNull(mediaType);
            ArgumentNullException.ThrowIfNull(TmdbId);

            return await PostAsync<WatchedList>(Endpoint.Remove(mediaType, TmdbId), null, null);
        }
    }
}