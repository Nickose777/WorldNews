using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Registration;
using WorldNews.Logic.Identity;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager userManager;
        private readonly RoleManager roleManager;
        private readonly SignInManager signInManager;

        public AccountService(IUnitOfWork unitOfWork, IAuthenticationManager authenticationManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = new UserManager(new UserStore(unitOfWork.Context));
            this.roleManager = new RoleManager(new RoleStore(unitOfWork.Context));
            this.signInManager = new SignInManager(userManager, authenticationManager);
        }

        public ServiceMessage InitializeRoles()
        {
            List<string> errors = new List<string>();
            bool succeeded = true;

            try
            {
                succeeded = InitializeRoles(errors, Roles.AdminRole, Roles.ModeratorRole, Roles.UserRole);

                if (succeeded)
                {
                    succeeded = InitializeAdmin(Roles.AdminRole, errors);
                }
            }
            catch (Exception ex)
            {
                succeeded = false;
                ExceptionMessageBuilder.FillErrors(ex, errors);
            }

            return new ServiceMessage
            {
                Succeeded = succeeded,
                Errors = errors
            };
        }

        public ServiceMessage RegisterUser(UserRegisterDTO userDTO)
        {
            List<string> errors = new List<string>();
            bool succeeded = Validate(userDTO, errors);

            if (succeeded)
            {
                try
                {
                    succeeded = Register(userDTO, errors);
                    if (succeeded)
                    {
                        UserEntity userEntity = new UserEntity
                        {
                            Id = userManager.FindByName(userDTO.Login).Id
                        };

                        unitOfWork.Users.Add(userEntity);
                        userManager.AddToRole(userEntity.Id, Roles.UserRole);
                    }
                }
                catch (Exception ex)
                {
                    succeeded = false;
                    ExceptionMessageBuilder.FillErrors(ex, errors);
                }
            }

            return new ServiceMessage
            {
                Succeeded = succeeded,
                Errors = errors
            };
        }

        public ServiceMessage RegisterModerator(ModeratorRegisterDTO moderatorDTO)
        {
            List<string> errors = new List<string>();
            bool succeeded = Validate(moderatorDTO, errors);

            if (succeeded)
            {
                try
                {
                    succeeded = Register(moderatorDTO, errors);
                    if (succeeded)
                    {
                        ModeratorEntity moderatorEntity = new ModeratorEntity
                        {
                            Id = userManager.FindByName(moderatorDTO.Login).Id,
                            PhotoLink = moderatorDTO.PhotoLink
                        };

                        unitOfWork.Moderators.Add(moderatorEntity);
                        userManager.AddToRole(moderatorEntity.Id, Roles.ModeratorRole);
                    }
                }
                catch (Exception ex)
                {
                    succeeded = false;
                    ExceptionMessageBuilder.FillErrors(ex, errors);
                }
            }

            return new ServiceMessage
            {
                Succeeded = succeeded,
                Errors = errors
            };
        }

        public ServiceMessage LogIn(string login, string password)
        {
            List<string> errors = new List<string>();
            bool succeeded = true;

            SignInStatus status = signInManager.PasswordSignIn(login, password, true, false);
            switch (status)
            {
                case SignInStatus.Failure:
                    succeeded = false;
                    errors.Add("Invalid login or password");
                    break;
                case SignInStatus.LockedOut:
                    succeeded = false;
                    errors.Add("Your account is blocked");
                    break;
                case SignInStatus.RequiresVerification:
                    succeeded = false;
                    errors.Add("Your account required verification");
                    break;
                case SignInStatus.Success:
                    break;
                default:
                    break;
            }

            return new ServiceMessage
            {
                Succeeded = succeeded,
                Errors = errors
            };
        }

        public void LogOff()
        {
            signInManager.AuthenticationManager.SignOut();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        private bool Register(RegisterBaseDTO registerBaseDTO, List<string> errors)
        {
            bool succeeded = ValidateCredentials(registerBaseDTO, errors);
            if (succeeded)
            {
                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = registerBaseDTO.Login,
                    Email = registerBaseDTO.Email,
                    FirstName = registerBaseDTO.FirstName,
                    LastName = registerBaseDTO.LastName
                };
                string password = registerBaseDTO.Password;
                IdentityResult identityResult = userManager.Create(applicationUser, password);

                if (!identityResult.Succeeded)
                {
                    errors.AddRange(identityResult.Errors);
                    succeeded = false;
                }
            }

            return succeeded;
        }

        #region Initialization
        private bool InitializeRoles(List<string> errors, params string[] roles)
        {
            bool succeeded = true;

            foreach (string role in roles)
            {
                if (!roleManager.RoleExists(role))
                {
                    IdentityResult identityResult = roleManager.Create(new ApplicationRole { Name = role });
                    if (!identityResult.Succeeded)
                    {
                        succeeded = false;
                        errors.AddRange(identityResult.Errors);
                    }
                }
            }

            return succeeded;
        }

        private bool InitializeAdmin(string role, List<string> errors)
        {
            bool succeeded = true;

            const string adminLogin = "admin";
            const string password = "Nick2397";

            ApplicationUser admin = userManager.FindByName(adminLogin);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminLogin,
                    FirstName = "John",
                    LastName = "Black",
                    Email = "admin@gmail.com"
                };

                IdentityResult identityResult = userManager.Create(admin, password);
                if (identityResult.Succeeded)
                {
                    userManager.SetLockoutEnabled(admin.Id, false);
                }
                else
                {
                    succeeded = false;
                    errors.AddRange(identityResult.Errors);
                }
            }

            if (succeeded && !unitOfWork.Profiles.IsInRole(admin.Id, role))
            {
                IdentityResult identityResult = userManager.AddToRole(admin.Id, role);
                if (!identityResult.Succeeded)
                {
                    succeeded = false;
                    errors.AddRange(identityResult.Errors);
                }
            }

            return succeeded;
        }
        #endregion

        #region Validation
        private bool Validate(UserRegisterDTO userDTO, ICollection<string> errors)
        {
            bool isValid = ValidateRegisterBase(userDTO, errors);

            return isValid;
        }

        private bool Validate(ModeratorRegisterDTO moderatorDTO, ICollection<string> errors)
        {
            bool isValid = ValidateRegisterBase(moderatorDTO, errors);

            if (String.IsNullOrEmpty(moderatorDTO.PhotoLink))
            {
                isValid = false;
                errors.Add("Photo link name cannot be empty");
            }

            return isValid;
        }

        private bool ValidateRegisterBase(RegisterBaseDTO registerBaseDTO, ICollection<string> errors)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(registerBaseDTO.Login))
            {
                isValid = false;
                errors.Add("Login cannot be empty");
            }

            if (String.IsNullOrEmpty(registerBaseDTO.Email))
            {
                isValid = false;
                errors.Add("E-Mail cannot be empty");
            }

            if (String.IsNullOrEmpty(registerBaseDTO.Password))
            {
                isValid = false;
                errors.Add("Password cannot be empty");
            }

            if (String.IsNullOrEmpty(registerBaseDTO.FirstName))
            {
                isValid = false;
                errors.Add("First name cannot be empty");
            }

            if (String.IsNullOrEmpty(registerBaseDTO.LastName))
            {
                isValid = false;
                errors.Add("Last name cannot be empty");
            }

            return isValid;
        }

        private bool ValidateCredentials(RegisterBaseDTO registerBaseDTO, ICollection<string> errors)
        {
            bool isValid = true;

            if (unitOfWork.Users.EmailExists(registerBaseDTO.Email))
            {
                isValid = false;
                errors.Add("Such E-Mail is already registered");
            }

            if (unitOfWork.Users.LoginExists(registerBaseDTO.Login))
            {
                isValid = false;
                errors.Add("Such login is already taken");
            }

            return isValid;
        }
        #endregion
    }
}
