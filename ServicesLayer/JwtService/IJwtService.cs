using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
using ServicesLayer.UserService;

namespace ServicesLayer.JwtService
{
    public interface IJwtService
    {
        public string GenerateToken(User user);
    }
}
