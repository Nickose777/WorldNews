using System;
using System.Collections.Generic;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Article;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEncryptor encryptor;

        public ArticleService(IUnitOfWork unitOfWork, IEncryptor encryptor)
        {
            this.unitOfWork = unitOfWork;
            this.encryptor = encryptor;
        }

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

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        private bool Validate(ArticleCreateDTO articleDTO, List<string> errors)
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
    }
}
