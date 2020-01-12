
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Template.Localization
{
    // https://stackoverflow.com/questions/19961648/how-to-localize-asp-net-identity-username-and-password-error-messages/40801250#40801250
    public sealed class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        /// <summary> 
        /// The <see cref="IStringLocalizer{LocalizedIdentityErrorDescriber}"/>
        /// used to localize the strings
        /// </summary>
        private readonly IStringLocalizer<LocalizedIdentityErrorDescriber> localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedIdentityErrorDescriber"/> class.
        /// </summary>
        /// <param name="localizer">
        /// The <see cref="IStringLocalizer{LocalizedIdentityErrorDescriber}"/>
        /// that we will use to localize the strings
        /// </param>
        public LocalizedIdentityErrorDescriber(IStringLocalizer<LocalizedIdentityErrorDescriber> localizer)
        {
            this.localizer = localizer;
        }

        /// <summary>
        /// Returns the default <see cref="IdentityError" />.
        /// </summary>
        /// <returns>The default <see cref="IdentityError" /></returns>
        public override IdentityError DefaultError()
        {
            return this.GetErrorByCode("DefaultError");
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating a concurrency failure.
        /// </summary>
        /// <returns>An <see cref="IdentityError" /> indicating a concurrency failure.</returns>
        public override IdentityError ConcurrencyFailure()
        {
            return this.GetErrorByCode("ConcurrencyFailure");
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating a password mismatch.
        /// </summary>
        /// <returns>An <see cref="IdentityError" /> indicating a password mismatch.</returns>
        public override IdentityError PasswordMismatch()
        {
            return this.GetErrorByCode("PasswordMismatch");
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating an invalid token.
        /// </summary>
        /// <returns>An <see cref="IdentityError" /> indicating an invalid token.</returns>
        public override IdentityError InvalidToken()
        {
            return this.GetErrorByCode("InvalidToken");
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating an external login is already associated with an account.
        /// </summary>
        /// <returns>An <see cref="IdentityError" /> indicating an external login is already associated with an account.</returns>
        public override IdentityError LoginAlreadyAssociated()
        {
            return this.GetErrorByCode("LoginAlreadyAssociated");
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating the specified user <paramref name="userName" /> is invalid.
        /// </summary>
        /// <param name="userName">The user name that is invalid.</param>
        /// <returns>An <see cref="IdentityError" /> indicating the specified user <paramref name="userName" /> is invalid.</returns>
        public override IdentityError InvalidUserName(string userName)
        {
            return this.FormatErrorByCode("InvalidUserName", (object)userName);
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating the specified <paramref name="email" /> is invalid.
        /// </summary>
        /// <param name="email">The email that is invalid.</param>
        /// <returns>An <see cref="IdentityError" /> indicating the specified <paramref name="email" /> is invalid.</returns>
        public override IdentityError InvalidEmail(string email)
        {
            return this.FormatErrorByCode("InvalidEmail", (object)email);
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating the specified <paramref name="userName" /> already exists.
        /// </summary>
        /// <param name="userName">The user name that already exists.</param>
        /// <returns>An <see cref="IdentityError" /> indicating the specified <paramref name="userName" /> already exists.</returns>
        public override IdentityError DuplicateUserName(string userName)
        {
            return this.FormatErrorByCode("DuplicateUserName", (object)userName);
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating the specified <paramref name="email" /> is already associated with an account.
        /// </summary>
        /// <param name="email">The email that is already associated with an account.</param>
        /// <returns>An <see cref="IdentityError" /> indicating the specified <paramref name="email" /> is already associated with an account.</returns>
        public override IdentityError DuplicateEmail(string email)
        {
            return this.FormatErrorByCode("DuplicateEmail", (object)email);
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating the specified <paramref name="role" /> name is invalid.
        /// </summary>
        /// <param name="role">The invalid role.</param>
        /// <returns>An <see cref="IdentityError" /> indicating the specific role <paramref name="role" /> name is invalid.</returns>
        public override IdentityError InvalidRoleName(string role)
        {
            return this.FormatErrorByCode("InvalidRoleName", (object)role);
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating the specified <paramref name="role" /> name already exists.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="IdentityError" /> indicating the specific role <paramref name="role" /> name already exists.</returns>
        public override IdentityError DuplicateRoleName(string role)
        {
            return this.FormatErrorByCode("DuplicateRoleName", (object)role);
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating a user already has a password.
        /// </summary>
        /// <returns>An <see cref="IdentityError" /> indicating a user already has a password.</returns>
        public override IdentityError UserAlreadyHasPassword()
        {
            return this.GetErrorByCode("UserAlreadyHasPassword");
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating user lockout is not enabled.
        /// </summary>
        /// <returns>An <see cref="IdentityError" /> indicating user lockout is not enabled..</returns>
        public override IdentityError UserLockoutNotEnabled()
        {
            return this.GetErrorByCode("UserLockoutNotEnabled");
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating a user is already in the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="IdentityError" /> indicating a user is already in the specified <paramref name="role" />.</returns>
        public override IdentityError UserAlreadyInRole(string role)
        {
            return this.FormatErrorByCode("UserAlreadyInRole", (object)role);
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating a user is not in the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="IdentityError" /> indicating a user is not in the specified <paramref name="role" />.</returns>
        public override IdentityError UserNotInRole(string role)
        {
            return this.FormatErrorByCode("UserNotInRole", (object)role);
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating a password of the specified <paramref name="length" /> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is not long enough.</param>
        /// <returns>An <see cref="IdentityError" /> indicating a password of the specified <paramref name="length" /> does not meet the minimum length requirements.</returns>
        public override IdentityError PasswordTooShort(int length)
        {
            return this.FormatErrorByCode("PasswordTooShort", (object)length);
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating a password entered does not contain a non-alphanumeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError" /> indicating a password entered does not contain a non-alphanumeric character.</returns>
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return this.GetErrorByCode("PasswordRequiresNonAlphanumeric");
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating a password entered does not contain a numeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError" /> indicating a password entered does not contain a numeric character.</returns>
        public override IdentityError PasswordRequiresDigit()
        {
            return this.GetErrorByCode("PasswordRequiresDigit");
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating a password entered does not contain a lower case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError" /> indicating a password entered does not contain a lower case letter.</returns>
        public override IdentityError PasswordRequiresLower()
        {
            return this.GetErrorByCode("PasswordRequiresLower");
        }

        /// <summary>
        /// Returns an <see cref="IdentityError" /> indicating a password entered does not contain an upper case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="IdentityError" /> indicating a password entered does not contain an upper case letter.</returns>
        public override IdentityError PasswordRequiresUpper()
        {
            return this.GetErrorByCode("PasswordRequiresUpper");
        }

        /// <summary>Returns a localized <see cref="IdentityError"/> for the provided code.</summary>
        /// <param name="code">The error's code.</param>
        /// <returns>A localized <see cref="IdentityError"/>.</returns>
        private IdentityError GetErrorByCode(string code)
        {
            return new IdentityError()
            {
                Code = code,
                Description = this.localizer.GetString(code)
            };
        }

        /// <summary>Formats a localized <see cref="IdentityError"/> for the provided code.</summary>
        /// <param name="code">The error's code.</param>
        /// <param name="parameters">The parameters to format the string with.</param>
        /// <returns>A localized <see cref="IdentityError"/>.</returns>
        private IdentityError FormatErrorByCode(string code, params object[] parameters)
        {
            return new IdentityError
            {
                Code = code,
                Description = string.Format(this.localizer.GetString(code, parameters))
            };
        }
    }
}
