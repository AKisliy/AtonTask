using System.Text.RegularExpressions;
using UserService.Core.Interfaces.Infrastructure;

namespace UserService.Infrastructure
{
    public class ValidationHelper: IValidationHelper
    {
        const string loginPattern = "^[0-9A-Za-z]*$";
        const string passwordPattern = "^[0-9A-Za-z]*$";
        const string namePattern = "^[A-Za-zА-Яа-я]*$";

        public bool ValidateLogin(string login)
        {
            Regex regex = new Regex(loginPattern);
            return regex.Match(login).Success;
        } 

        public bool ValidatePassword(string password)
        {
            Regex regex = new Regex(passwordPattern);
            return regex.Match(password).Success;
        }

        public bool ValidateName(string name)
        {
            Regex regex = new Regex(namePattern);
            return regex.Match(name).Success;
        }
    }
}