using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.Exceptions;
using EMNDC.Preposicionamiento.Exceptions.BadRequest;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Models.Requests;
using EMNDC.Preposicionamiento.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;


namespace EMNDC.Preposicionamiento.Services
{
    public class AuthService : IAuthService
    {
        private readonly IStringLocalizer<IAuthService> _localizer;
        private readonly IMailKitService _emailService;
        private readonly IActiveDirectoryService _ldapService;
        private readonly UserManager<UserModel> _userManager;
        private readonly PreposicionamientoDbContext _context;
        public AuthService(IConfiguration configuration,
            IStringLocalizer<IAuthService> localizer, UserManager<UserModel> userManager,
            IMailKitService emailService, PreposicionamientoDbContext context, IOptions<JwtSettings> jwtSettings,
            IActiveDirectoryService ldapService)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _ldapService = ldapService ?? throw new ArgumentNullException(nameof(ldapService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        public async Task<UserModel> SignUpAsync(RegisterUserRequests request)
        {
            var user = new UserModel
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow,
                ModifieddAt = DateTime.UtcNow,
            };

            var validateEmail = await _context.Users.Where(r=> r.Email == request.Email).FirstOrDefaultAsync();
            if (validateEmail !=null) throw new EmailAlreadyRegisteredBadRequestException(_localizer);

            var validate_password = await ValidatePasswordAsync(request.Password);

            if (!validate_password.Succeeded)
                throw new PasswordInvalidBadRequesException(_localizer);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new ErrorCreatingUserBadRequesException(_localizer);

            return user;
        }

        private async Task<IdentityResult> ValidatePasswordAsync(string password)
        {
            var user = new UserModel();
            var validators = _userManager.PasswordValidators;

            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(_userManager, user, password);
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            return IdentityResult.Success;
        }

        public async Task<UserModel> LoginAsync(LoginRequests request)
        {
            if (request.Email.Split('@')[1] == "dcn.co.cu")
            {
                var adUser= await _ldapService.AuthenticateAsync(request.Email, request.Password);
                var ldapUser = await _userManager.FindByEmailAsync(adUser.Email);

                var newUser = new UserModel
                {
                    UserName = adUser.Email,
                    Email = adUser.Email,
                    Name = adUser.FirstName,
                    LastName = adUser.LastName,
                    IsUserDomain = true,
                    CreatedAt = DateTime.UtcNow,
                    ModifieddAt = DateTime.UtcNow,
                };

                if (ldapUser == null)
                {
                    var result = await _context.Users.AddAsync(newUser);
                    
                }
                else
                {
                    var result  =  _context.Users.Update(newUser);
                }
                await _context.SaveChangesAsync();
                return newUser;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new BaseUnauthorizedException();

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid) throw new BaseUnauthorizedException();

            return user;
        }

        public async Task<bool> SendResetCodeAsync(SendEmailRequest email)
        {
            var user = await _userManager.FindByEmailAsync(email.Email);

            if (user is null)
            {
                throw new EmailNotFoundException(_localizer);
            }

            DateTime now = DateTime.Now.ToUniversalTime();
            var subtrackTime = now.Subtract(user.DateGeneratedCode).TotalMinutes;

            if (user.Code != null && subtrackTime <= 1)
            {
                throw new ErrorSendNextCodeRequestTimeOutException(_localizer);
            }

            Random rand = new Random();
            int code = rand.Next(100000, 999999);
            string subject = $"Verifications Code .";

            var body = $"<p> Hi {user.Name} {user.LastName}, </p>" +
                           $"<p>This is yours verifications code.</p>" +
                           $"<p>Code: {code}</p>" +
                           $"<p> Regards, </p> <br>" +
                           $"<p>Telescopy Teams</p>";

            EmailRequest emailSent = new()
            {
                Body = body,
                Subject = subject,
                To = email.Email,
            };

            await _emailService.SendEmailResponseAsync(emailSent);

            user.Code = code;
            user.DateGeneratedCode = now.ToUniversalTime();

            await _userManager.UpdateAsync(user);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                throw new EmailNotFoundException(_localizer);
            }

            if (user.Code != int.Parse(request.ResetCode))
            {
                throw new InvalidCodeNotFoundException(_localizer);
            }

            DateTime now = DateTime.Now.ToUniversalTime();
            var subtrackTime = now.Subtract(user.DateGeneratedCode).TotalMinutes;

            if (user.Code != null && subtrackTime > 1)
            {
                throw new CodeExpiredResquestTimeOutException(_localizer);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                throw new ChangePasswordForbidenException(_localizer);
            }

            return true;
        }

        public async Task<UserModel> GetUserDetailsAsync(string userId)
        {
            var userExist = await _userManager.FindByIdAsync(userId);

            if (userExist is null)
            {
                throw new UserNotFoundException(_localizer);
            }

            return userExist;
        }
    }
}