using System;
using System.Collections.Generic;
using System.Linq;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Comment;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Services
{
    public class ModeratorService : ServiceBase, IModeratorService
    {
        public ModeratorService(IUnitOfWork unitOfWork, IEncryptor encryptor)
            : base(unitOfWork, encryptor) { }

        public ServiceMessage Edit(ModeratorEditDTO moderatorDTO)
        {
            List<string> errors = new List<string>();
            bool succeeded = Validate(moderatorDTO, errors);

            try
            {
                ModeratorEntity moderatorEntity = unitOfWork.Moderators.GetByLogin(moderatorDTO.Login);
                if (moderatorEntity != null)
                {
                    ApplicationUser applicationUser = moderatorEntity.User;
                    applicationUser.FirstName = moderatorDTO.FirstName;
                    applicationUser.LastName = moderatorDTO.LastName;

                    unitOfWork.Commit();
                }
                else
                {
                    succeeded = false;
                    errors.Add("Moderator with such login was not found");
                }
            }
            catch (Exception ex)
            {
                ExceptionMessageBuilder.FillErrors(ex, errors);
                succeeded = false;
            }

            return new ServiceMessage
            {
                Succeeded = succeeded,
                Errors = errors
            };
        }

        public DataServiceMessage<ModeratorEditDTO> Get(string login)
        {
            List<string> errors = new List<string>();
            bool succeeded = true;
            ModeratorEditDTO data = null;

            try
            {
                ModeratorEntity moderatorEntity = unitOfWork.Moderators.GetByLogin(login);
                if (moderatorEntity != null)
                {
                    data = new ModeratorEditDTO
                    {
                        Email = moderatorEntity.User.Email,
                        FirstName = moderatorEntity.User.FirstName,
                        LastName = moderatorEntity.User.LastName,
                        Login = login,
                        PhotoLink = moderatorEntity.PhotoLink
                    };
                }
                else
                {
                    succeeded = false;
                    errors.Add("Moderator with such login was not found");
                }
            }
            catch (Exception ex)
            {
                ExceptionMessageBuilder.FillErrors(ex, errors);
                succeeded = false;
            }

            return new DataServiceMessage<ModeratorEditDTO>
            {
                Succeeded = succeeded,
                Errors = errors,
                Data = data
            };
        }

        public DataServiceMessage<ModeratorDetailsDTO> GetDetails(string login)
        {
            List<string> errors = new List<string>();
            bool succeeded = true;
            ModeratorDetailsDTO data = null;

            try
            {
                ModeratorEntity moderatorEntity = unitOfWork.Moderators.GetByLogin(login);
                if (moderatorEntity != null)
                {
                    data = new ModeratorDetailsDTO
                    {
                        Email = moderatorEntity.User.Email,
                        FirstName = moderatorEntity.User.FirstName,
                        LastName = moderatorEntity.User.LastName,
                        Login = login,
                        PhotoLink = moderatorEntity.PhotoLink,
                        BannedComments = moderatorEntity.BannedComments.Select(commentEntity =>
                            new CommentBanDetailsDTO
                            {
                                Id = encryptor.Encrypt(commentEntity.Id.ToString()),
                                Text = commentEntity.Text,
                                DateBanned = commentEntity.DateBanned.Value,
                                BanReason = commentEntity.BanReason.Name,
                                AuthorFullName = commentEntity.Author.FirstName + " " + commentEntity.Author.LastName
                            })
                            .OrderByDescending(comment => comment.DateBanned)
                            .ToList()
                    };
                }
                else
                {
                    succeeded = false;
                    errors.Add("Moderator with such login was not found");
                }
            }
            catch (Exception ex)
            {
                ExceptionMessageBuilder.FillErrors(ex, errors);
                succeeded = false;
            }

            return new DataServiceMessage<ModeratorDetailsDTO>
            {
                Succeeded = succeeded,
                Errors = errors,
                Data = data
            };
        }

        public DataServiceMessage<IEnumerable<ModeratorListDTO>> GetAll()
        {
            List<string> errors = new List<string>();
            bool succeeded = true;
            IEnumerable<ModeratorListDTO> data = null;

            try
            {
                IEnumerable<ModeratorEntity> moderatorEntities = unitOfWork.Moderators.GetAll();
                data = moderatorEntities.Select(moderatorEntity =>
                    new ModeratorListDTO
                    {
                        FirstName = moderatorEntity.User.FirstName,
                        LastName = moderatorEntity.User.LastName,
                        Email = moderatorEntity.User.Email,
                        Login = moderatorEntity.User.UserName,
                        PhotoLink = moderatorEntity.PhotoLink
                    })
                    .OrderBy(moderator => moderator.FirstName)
                    .ThenBy(moderator => moderator.LastName)
                    .ToList();
            }
            catch (Exception ex)
            {
                ExceptionMessageBuilder.FillErrors(ex, errors);
                succeeded = false;
            }

            return new DataServiceMessage<IEnumerable<ModeratorListDTO>>
            {
                Succeeded = succeeded,
                Errors = errors,
                Data = data
            };
        }

        private bool Validate(ModeratorEditDTO moderatorDTO, ICollection<string> errors)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(moderatorDTO.FirstName))
            {
                isValid = false;
                errors.Add("First name cannot be empty");
            }

            if (String.IsNullOrEmpty(moderatorDTO.LastName))
            {
                isValid = false;
                errors.Add("Last name cannot be empty");
            }

            if (String.IsNullOrEmpty(moderatorDTO.Email))
            {
                isValid = false;
                errors.Add("Email is required");
            }

            if (String.IsNullOrEmpty(moderatorDTO.Login))
            {
                isValid = false;
                errors.Add("Login is required");
            }

            if (String.IsNullOrEmpty(moderatorDTO.PhotoLink))
            {
                isValid = false;
                errors.Add("Photo is required");
            }

            return isValid;
        }
    }
}
