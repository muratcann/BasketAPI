using Basket.Api.Entities;
using Basket.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Rules
{
    public class UserRule : IBusinessRule
    {
        private readonly User _user;
        private RuleType _ruleType;
        public UserRule(User user)
        {
            _user = user;
        }

        public bool IsBroken()
        {
            //return _user != null && _user.Status == 1;
            if (_user == null)
            {
                _ruleType = RuleType.UserNotFound;
                return false;
            }
            else if (_user.Status != 1)
            {
                _ruleType = RuleType.UserStatusNotSuitable;
                return false;
            }
            return true;
        }

        public string Message()
        {
            switch (_ruleType)
            {
                case RuleType.UserNotFound:
                    return "User not found";
                case RuleType.UserStatusNotSuitable:
                    return "User status not available to purchase";
                default:
                    return "";
            }
        }
    }
}
