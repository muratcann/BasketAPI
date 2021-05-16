using Basket.Api.Entities;
using Basket.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Rules
{
    public class ProductAndBasketItemRule : IBusinessRule
    {
        private readonly Product _product;
        private readonly BasketItemModel _basketItem;
        private RuleType _ruleType;
        public ProductAndBasketItemRule(Product product, BasketItemModel basketItem)
        {
            _product = product;
            _basketItem = basketItem;
        }

        public bool IsBroken()
        {
            /*
            return _product != null
                && _product.Status == 1
                && _product.Stock >= _basketItem.Quantity
                && _product.OrderMinimumQuantity <= _basketItem.Quantity
                && _product.OrderMaximumQuantity >= _basketItem.Quantity;
            */
            if (_product == null)
            {
                _ruleType = RuleType.ProductNotFound;
                return false;
            }
            else if (_product.Status != 1)
            {
                _ruleType = RuleType.ProductStatusNotSuitable;
                return false;
            }
            else if (_product.Stock < _basketItem.Quantity)
            {
                _ruleType = RuleType.NoProductStock;
                return false;
            }
            else if (_product.OrderMinimumQuantity > _basketItem.Quantity)
            {
                _ruleType = RuleType.OrderMinimumQuantityNotReached;
                return false;
            }
            else if (_product.OrderMaximumQuantity < _basketItem.Quantity)
            {
                _ruleType = RuleType.OrderMaximumQuantityExceeded;
                return false;
            }
            return true;
        }

        public string Message()
        {
            switch (_ruleType)
            {
                case RuleType.ProductNotFound:
                    return "Product not found";
                case RuleType.ProductStatusNotSuitable:
                    return "Product status not available to sale";
                case RuleType.NoProductStock:
                    return "There is no product stock";
                case RuleType.OrderMinimumQuantityNotReached:
                    return "Order minimum quantity not reached";
                case RuleType.OrderMaximumQuantityExceeded:
                    return "Order maximum quantity exceeded";
                default:
                    return "";
            }
        }
    }
}
