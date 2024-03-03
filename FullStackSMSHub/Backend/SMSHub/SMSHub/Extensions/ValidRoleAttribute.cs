namespace SMSHub.APIs.Extensions
{
    using System.ComponentModel.DataAnnotations;

    public class ValidRoleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var allowedRoles = new[] { "admin", "user" };
            if (value == null || !allowedRoles.Contains(value.ToString().ToLower()))
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"Invalid role. Valid roles are: admin, user.";
        }
    }

}
