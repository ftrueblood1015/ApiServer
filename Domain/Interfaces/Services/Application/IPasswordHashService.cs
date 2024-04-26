using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.Application
{
    public interface IPasswordHashService
    {
        string CreateHash(string plainTextPassword);
        bool VerifyPassword(string plainTextPassword, string hashedPassword);
    }
}
