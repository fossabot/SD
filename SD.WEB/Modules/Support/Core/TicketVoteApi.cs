﻿using Microsoft.Extensions.Caching.Memory;
using SD.Shared.Models.Support;

namespace SD.WEB.Modules.Support.Core
{
    public class TicketVoteApi(IHttpClientFactory http, IMemoryCache memoryCache) : ApiCore<TicketVoteModel>(http, memoryCache, "TicketVoteModel")
    {
        private struct Endpoint
        {
            public const string GetMyVotes = "Ticket/GetMyVotes";
            public const string Vote = "Ticket/Vote";
        }

        public async Task<HashSet<TicketVoteModel>> GetMyVotes(bool IsUserAuthenticated)
        {
            if (IsUserAuthenticated)
            {
                return await GetListAsync(Endpoint.GetMyVotes, false);
            }
            else
            {
                return [];
            }
        }

        public async Task<TicketVoteModel?> Vote(TicketVoteModel obj)
        {
            return await PostAsync(Endpoint.Vote, obj);
        }
    }
}