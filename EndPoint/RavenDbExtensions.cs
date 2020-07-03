using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace EndPoint
{
    static class RavenDbExtensions
    {

        public static async Task EnsureDatabaseExistsAsync(this IDocumentStore store, string database = null, bool createDatabaseIfNotExists = true)
        {
            database ??= store.Database;

            if (string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

            try
            {
                await store.Maintenance.ForDatabase(database).SendAsync(new GetStatisticsOperation()).ConfigureAwait(false);
            }
            catch (DatabaseDoesNotExistException) when (createDatabaseIfNotExists)
            {

                try
                {
                    await store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(database))).ConfigureAwait(false);
                }
                catch (ConcurrencyException)
                {
                }
            }
        }
    }
}
