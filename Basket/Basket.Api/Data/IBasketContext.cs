using Basket.Api.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Data
{
    public interface IBasketContext
    {
        IMongoCollection<BasketItem> BasketItems { get; }
    }
}
