using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Article;
using WorldNews.Logic.DTO.Comment;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Services
{
    public class ArticleService : ServiceBase, IArticleService
    {
        public ArticleService(IUnitOfWork unitOfWork, IEncryptor encryptor)
            : base(unitOfWork, encryptor) { }

        public ServiceMessage Create(ArticleCreateDTO articleDTO)
        {
            List<string> errors = new List<string>();
            bool succeeded = Validate(articleDTO, errors);

            if (succeeded)
            {
                try
                {
                    CategoryEntity categoryEntity = unitOfWork.Categories.GetByName(articleDTO.CategoryName);
                    if (categoryEntity != null)
                    {
                        ModeratorEntity moderatorEntity = unitOfWork.Moderators.GetByLogin(articleDTO.AuthorLogin);
                        if (moderatorEntity != null)
                        {
                            ArticleEntity articleEntity = new ArticleEntity
                            {
                                Author = moderatorEntity,
                                Category = categoryEntity,
                                DateCreated = DateTime.Now,
                                DateLastModified = DateTime.Now,
                                Header = articleDTO.Header,
                                PhotoLink = articleDTO.PhotoLink,
                                ShortDescription = articleDTO.ShortDescription,
                                Text = articleDTO.Text
                            };

                            unitOfWork.Articles.Add(articleEntity);
                            unitOfWork.Commit();
                        }
                        else
                        {
                            succeeded = false;
                            errors.Add(String.Format("Moderator with login {0} was not found", articleDTO.AuthorLogin));
                        }
                    }
                    else
                    {
                        succeeded = false;
                        errors.Add(String.Format("Category with name {0} was not found", articleDTO.CategoryName));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionMessageBuilder.FillErrors(ex, errors);
                    succeeded = false;
                }
            }

            return new ServiceMessage
            {
                Errors = errors,
                Succeeded = succeeded
            };
        }

        public ServiceMessage Edit(ArticleEditDTO articleDTO)
        {
            int id;
            string decryptedId = encryptor.Decrypt(articleDTO.Id);

            List<string> errors = new List<string>();
            bool succeeded = true;

            if (Int32.TryParse(decryptedId, out id))
            {
                if (succeeded = Validate(articleDTO, errors))
                {
                    try
                    {
                        CategoryEntity categoryEntity = unitOfWork.Categories.GetByName(articleDTO.CategoryName);
                        if (categoryEntity != null)
                        {
                            ArticleEntity articleEntity = unitOfWork.Articles.Get(id);
                            if (articleEntity != null)
                            {
                                articleEntity.Category = categoryEntity;
                                articleEntity.DateLastModified = DateTime.Now;
                                articleEntity.Header = articleDTO.Header;
                                articleEntity.PhotoLink = articleDTO.PhotoLink;
                                articleEntity.ShortDescription = articleDTO.ShortDescription;
                                articleEntity.Text = articleDTO.Text;

                                unitOfWork.Commit();
                            }
                            else
                            {
                                succeeded = false;
                                errors.Add("Article was not found");
                            }
                        }
                        else
                        {
                            succeeded = false;
                            errors.Add(String.Format("Category with name {0} was not found", articleDTO.CategoryName));
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionMessageBuilder.FillErrors(ex, errors);
                        succeeded = false;
                    }
                }
            }
            else
            {
                succeeded = false;
                errors.Add("Article was not found");
            }

            return new ServiceMessage
            {
                Errors = errors,
                Succeeded = succeeded
            };
        }

        public DataServiceMessage<ArticleEditDTO> Get(string encryptedId)
        {
            string decryptedId = encryptor.Decrypt(encryptedId);
            int id;

            List<string> errors = new List<string>();
            bool succeeded = true;
            ArticleEditDTO data = null;

            if (Int32.TryParse(decryptedId, out id))
            {
                try
                {
                    ArticleEntity articleEntity = unitOfWork.Articles.Get(id);
                    if (articleEntity != null)
                    {
                        data = new ArticleEditDTO
                        {
                            Id = encryptedId,
                            CategoryName = articleEntity.Category.Name,
                            Header = articleEntity.Header,
                            PhotoLink = articleEntity.PhotoLink,
                            Text = articleEntity.Text,
                            ShortDescription = articleEntity.ShortDescription
                        };
                    }
                    else
                    {
                        succeeded = false;
                        errors.Add("Article was not found");
                    }
                }
                catch (Exception ex)
                {
                    succeeded = false;
                    ExceptionMessageBuilder.FillErrors(ex, errors);
                }
            }
            else
            {
                succeeded = false;
                errors.Add("Article was not found");
            }

            return new DataServiceMessage<ArticleEditDTO>
            {
                Errors = errors,
                Succeeded = succeeded,
                Data = data
            };
        }

        public DataServiceMessage<ArticleDetailsDTO> GetDetails(string encryptedId)
        {
            string decryptedId = encryptor.Decrypt(encryptedId);
            int id;

            List<string> errors = new List<string>();
            bool succeeded = true;
            ArticleDetailsDTO data = null;

            if (Int32.TryParse(decryptedId, out id))
            {
                try
                {
                    ArticleEntity articleEntity = unitOfWork.Articles.Get(id);
                    if (articleEntity != null)
                    {
                        data = new ArticleDetailsDTO
                        {
                            Id = encryptedId,
                            CategoryName = articleEntity.Category.Name,
                            DateCreated = articleEntity.DateCreated,
                            Header = articleEntity.Header,
                            PhotoLink = articleEntity.PhotoLink,
                            Text = articleEntity.Text,
                            Comments = articleEntity.Comments.Select(commentEntity =>
                                {
                                    ApplicationUser author = commentEntity.Author;
                                    string fullName = String.Format("{0} {1}", author.FirstName, author.LastName);
                                    if (author.Moderator != null)
                                    {
                                        fullName += " - [Moderator]";
                                    }
                                    else if (author.User == null)
                                    {
                                        fullName += " - [Admin]";
                                    }

                                    string content = commentEntity.DateBanned.HasValue
                                        ? "Reason of ban: " + commentEntity.BanReason.Name
                                        : commentEntity.Text;
                                    CommentListDTO commentDTO = new CommentListDTO
                                    {
                                        Id = encryptor.Encrypt(commentEntity.Id.ToString()),
                                        DateCreated = commentEntity.DateCreated,
                                        Content = content,
                                        AuthorDisplayFullName = fullName,
                                        IsBanned = commentEntity.DateBanned.HasValue
                                    };

                                    return commentDTO;
                                })
                                .OrderByDescending(commentEntity => commentEntity.DateCreated)
                                .ToList()
                        };
                    }
                    else
                    {
                        succeeded = false;
                        errors.Add("Article was not found");
                    }
                }
                catch (Exception ex)
                {
                    succeeded = false;
                    ExceptionMessageBuilder.FillErrors(ex, errors);
                }
            }
            else
            {
                succeeded = false;
                errors.Add("Article was not found");
            }

            return new DataServiceMessage<ArticleDetailsDTO>
            {
                Errors = errors,
                Succeeded = succeeded,
                Data = data
            };
        }

        public DataServiceMessage<IEnumerable<ArticleListDTO>> GetAllByCategory(string categoryName)
        {
            return GetAll(articleEntity => articleEntity.Category.Name == categoryName);
        }

        public DataServiceMessage<IEnumerable<ArticleListDTO>> GetAll()
        {
            return GetAll(articleEntity => true);
        }

        public DataServiceMessage<IEnumerable<ArticleListDTO>> GetAllEnabled()
        {
            return GetAll(articleEntity => articleEntity.Category.IsEnabled);
        }

        public DataServiceMessage<IEnumerable<ArticleAuthorListDTO>> GetAllWithAuthors()
        {
            List<string> errors = new List<string>();
            bool succeeded = true;
            IEnumerable<ArticleAuthorListDTO> data = null;

            try
            {
                IEnumerable<ArticleEntity> articleEntities = unitOfWork.Articles.GetAll();
                data = articleEntities.Select(articleEntity =>
                    new ArticleAuthorListDTO
                    {
                        Id = encryptor.Encrypt(articleEntity.Id.ToString()),
                        DateCreated = articleEntity.DateCreated,
                        DateLastModified = articleEntity.DateLastModified,
                        Header = articleEntity.Header,
                        AuthorFullName = articleEntity.Author.User.FirstName + " " + articleEntity.Author.User.LastName,
                        AuthorLogin = articleEntity.Author.User.UserName,
                        CommentsCount = articleEntity.Comments.Count
                    })
                    .OrderByDescending(article => article.DateCreated)
                    .ToList();
            }
            catch (Exception ex)
            {
                succeeded = false;
                ExceptionMessageBuilder.FillErrors(ex, errors);
            }

            return new DataServiceMessage<IEnumerable<ArticleAuthorListDTO>>
            {
                Errors = errors,
                Succeeded = succeeded,
                Data = data
            };
        }

        private DataServiceMessage<IEnumerable<ArticleListDTO>> GetAll(Expression<Func<ArticleEntity, bool>> expression)
        {
            List<string> errors = new List<string>();
            bool succeeded = true;
            IEnumerable<ArticleListDTO> data = null;

            try
            {
                IEnumerable<ArticleEntity> articleEntities = unitOfWork.Articles.GetAll(expression);
                data = articleEntities.Select(articleEntity =>
                    new ArticleListDTO
                    {
                        Id = encryptor.Encrypt(articleEntity.Id.ToString()),
                        DateCreated = articleEntity.DateCreated,
                        Description = articleEntity.ShortDescription,
                        Header = articleEntity.Header,
                        PhotoLink = articleEntity.PhotoLink
                    })
                    .OrderByDescending(article => article.DateCreated)
                    .ToList();
            }
            catch (Exception ex)
            {
                succeeded = false;
                ExceptionMessageBuilder.FillErrors(ex, errors);
            }

            return new DataServiceMessage<IEnumerable<ArticleListDTO>>
            {
                Errors = errors,
                Succeeded = succeeded,
                Data = data
            };
        }

        private bool Validate(ArticleCreateDTO articleDTO, ICollection<string> errors)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(articleDTO.Header))
            {
                isValid = false;
                errors.Add("Header cannot be empty");
            }

            if (String.IsNullOrEmpty(articleDTO.Text))
            {
                isValid = false;
                errors.Add("Text cannot be empty");
            }

            if (String.IsNullOrEmpty(articleDTO.ShortDescription))
            {
                isValid = false;
                errors.Add("Description cannot be empty");
            }

            if (String.IsNullOrEmpty(articleDTO.PhotoLink))
            {
                isValid = false;
                errors.Add("A photo must be chosen");
            }

            if (String.IsNullOrEmpty(articleDTO.CategoryName))
            {
                isValid = false;
                errors.Add("Category must be selected");
            }

            if (String.IsNullOrEmpty(articleDTO.AuthorLogin))
            {
                isValid = false;
                errors.Add("Author must be selected");
            }

            return isValid;
        }

        private bool Validate(ArticleEditDTO articleDTO, ICollection<string> errors)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(articleDTO.Header))
            {
                isValid = false;
                errors.Add("Header cannot be empty");
            }

            if (String.IsNullOrEmpty(articleDTO.Text))
            {
                isValid = false;
                errors.Add("Text cannot be empty");
            }

            if (String.IsNullOrEmpty(articleDTO.ShortDescription))
            {
                isValid = false;
                errors.Add("Description cannot be empty");
            }

            if (String.IsNullOrEmpty(articleDTO.PhotoLink))
            {
                isValid = false;
                errors.Add("A photo must be chosen");
            }

            if (String.IsNullOrEmpty(articleDTO.CategoryName))
            {
                isValid = false;
                errors.Add("Category must be selected");
            }

            return isValid;
        }
    }
}
