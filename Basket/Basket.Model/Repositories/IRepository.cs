using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Model.Repositories
{
    public interface IRepository : IRepositoryReadOnly
    {
        void Create<TEntity>(TEntity entity, string createdBy = null)
        where TEntity : class;

        void Update<TEntity>(TEntity entity, string modifiedBy = null)
            where TEntity : class;

        void Delete<TEntity>(object id)
            where TEntity : class;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class;

        void Save();

        Task SaveAsync();
    }
}
