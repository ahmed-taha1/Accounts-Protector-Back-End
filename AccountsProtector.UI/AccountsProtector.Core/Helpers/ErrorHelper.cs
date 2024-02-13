using AccountsProtector.AccountsProtector.Core.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AccountsProtector.AccountsProtector.Core.Helpers
{
    public class ErrorHelper
    {
        public static object ModelStateErrorHandler(ModelStateDictionary model)
        {
            List<string> errors = new List<string>();
            foreach (var modelState in model.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            return new DtoErrorsResponse() {Errors = errors};
        }

        public static object IdentityResultErrorHandler(IdentityResult identityResult)
        {
            List<string> errors = new List<string>();
            foreach (var error in identityResult.Errors)
            {
                string errorDescription = error.Description;
                if (errorDescription.Contains("Username"))
                {
                    errorDescription = errorDescription.Replace("Username", "Email");
                }
                errors.Add(errorDescription);
            }
            return new DtoErrorsResponse() { Errors = errors };
        }
    }
}
