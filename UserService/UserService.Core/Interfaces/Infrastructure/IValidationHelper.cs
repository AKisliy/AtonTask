using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Core.Interfaces.Infrastructure
{
    public interface IValidationHelper
    {
        bool ValidateLogin(string login);

        bool ValidatePassword(string password);

        bool ValidateName(string name);
    }
}