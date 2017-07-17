using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts;
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
                    succeeded = ValidateCredentials(userDTO.Login, userDTO.Email, errors);
                    if (succeeded)
                    {
                        ApplicationUser applicationUser = new ApplicationUser
                        {
                            UserName = userDTO.Login,
                            Email = userDTO.Email,
                            FirstName = userDTO.FirstName,
                            LastName = userDTO.LastName
                        };
                        string password = userDTO.Password;
                        IdentityResult identityResult = userManager.Create(applicationUser, password);

                        if (identityResult.Succeeded)
                        {
                            UserEntity userEntity = new UserEntity
                            {
                                Id = applicationUser.Id
                            };

                            unitOfWork.Users.Add(userEntity);
                            userManager.AddToRole(applicationUser.Id, Roles.UserRole);
                        }
                        else
                        {
                            errors.AddRange(identityResult.Errors);
                            succeeded = false;
                        }
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
            SignInStatus status = signInManager.PasswordSignIn(login, password, true, false);

            return new ServiceMessage
            {
                Succeeded = status == SignInStatus.Success,
                Errors = status != SignInStatus.Success ? new List<string>() { "Invalid login or password" } : null
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
                if (!identityResult.Succeeded)
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

            if (String.IsNullOrEmpty(userDTO.FirstName))
            {
                isValid = false;
                errors.Add("First name cannot be empty");
            }

            if (String.IsNullOrEmpty(userDTO.LastName))
            {
                isValid = false;
                errors.Add("Last name cannot be empty");
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

            return isValid;
        }

        private bool ValidateCredentials(string login, string email, ICollection<string> errors)
        {
            bool isValid = true;

            if (unitOfWork.Users.EmailExists(email))
            {
                isValid = false;
                errors.Add("Such E-Mail is already registered");
            }

            if (unitOfWork.Users.LoginExists(login))
            {
                isValid = false;
                errors.Add("Such login is already taken");
            }

            return isValid;
        }
        #endregion
    }
}
