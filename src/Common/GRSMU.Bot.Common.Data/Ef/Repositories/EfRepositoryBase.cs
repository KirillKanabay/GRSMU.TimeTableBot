using GRSMU.Bot.Common.Data.Ef.Entities;
using Microsoft.EntityFrameworkCore;

namespace GRSMU.Bot.Common.Data.Ef.Repositories;

public abstract class EfRepositoryBase<TEntity> 
    where TEntity : EntityBase

{
    protected DbSet<TEntity> Set { get; }

    protected EfRepositoryBase(DbContext dbContext)
    {
        Set = dbContext.Set<TEntity>();
    }
}