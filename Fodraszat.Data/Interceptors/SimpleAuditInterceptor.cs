using System;
using System.Threading;
using System.Threading.Tasks;
using Fodraszat.Data.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static Microsoft.EntityFrameworkCore.EntityState;

namespace Fodraszat.Data.Interceptors
{
    public class SimpleAuditInterceptor : SaveChangesInterceptor
    {
        private SimpleAuditInterceptor() { }

        public static SimpleAuditInterceptor Instance { get; } = new();

        private static void SetLetrehozasModositasDatumok(DbContextEventData eventData)
        {
            var now = DateTime.Now;
            foreach (var entityEntry in eventData.Context.ChangeTracker.Entries<EntityBase>())
            {
                if (entityEntry.State == Added)
                    entityEntry.Entity.LetrehozasDatum = now;
                if (entityEntry.State is Added or Modified)
                    entityEntry.Entity.ModositasDatum = now;
            }
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            SetLetrehozasModositasDatumok(eventData);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            SetLetrehozasModositasDatumok(eventData);
            return result;
        }
    }
}