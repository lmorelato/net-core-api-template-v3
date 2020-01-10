namespace Template.Shared
{
    public static class Constants
    {
        public static class Api
        {
            public static class Actions
            {
                public const string 
                    ConfirmEmail = "confirm-email",
                    PasswordReset = "password-reset";
            }
        }

        public static class Roles
        {
            public const string
                Any = "any",
                Admin = "admin",
                User = "user",
                Tenant = "tenant";
        }

        public static class Database
        {
            public const int
                DefaultVarcharMaxLength = 128,
                IdentityVarcharMaxLength = 256;
        }

        public static class ClaimTypes
        {
            public const string
                Id = "id",
                Role = "role";
        }

        public static class Mailjet
        {
            public const string
                Email = "Email",
                Name = "Name",
                From = "From",
                To = "To",
                Subject = "Subject",
                TemplateID = "TemplateID",
                TemplateLanguage = "TemplateLanguage",
                TextPart = "TextPart",
                HTMLPart = "HTMLPart",
                Variables = "Variables";
            
            public static class Keys
            {
                public const string 
                    ConfirmationEmailLink = "confirmation_link", 
                    NewPassword = "new_password";
            }
        }
    }
}