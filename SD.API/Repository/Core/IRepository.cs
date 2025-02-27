﻿using Microsoft.Azure.Cosmos;
using SD.Shared.Core.Models;
using System.Linq.Expressions;

namespace SD.API.Repository.Core
{
    public interface IRepository
    {
        Task<T?> Get<T>(string id, PartitionKey key, CancellationToken cancellationToken) where T : CosmosDocument;

        Task<List<T>> Query<T>(Expression<Func<T, bool>>? predicate, PartitionKey? key, DocumentType Type, CancellationToken cancellationToken) where T : MainDocument;

        Task<List<T>> Query<T>(QueryDefinition query, CancellationToken cancellationToken) where T : MainDocument;

        Task<T> Upsert<T>(T item, CancellationToken cancellationToken) where T : CosmosDocument;

        Task<T> PatchItem<T>(string id, PartitionKey key, List<PatchOperation> operations, CancellationToken cancellationToken) where T : CosmosDocument;

        Task<bool> Delete<T>(T item, CancellationToken cancellationToken) where T : CosmosDocument;
    }
}