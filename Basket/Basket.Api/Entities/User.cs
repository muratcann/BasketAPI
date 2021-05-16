using Basket.Api.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Entities
{
    public class User : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte Status { get; set; }

        public static User CreateUser(User user)
        {
            CheckRule(new UserRule(user));

            return user;
        }
    }
}
