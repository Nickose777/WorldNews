﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public DataServiceMessage<ArticleDetailsDTO> Get(string encryptedId)
        {
            string decryptedId = encryptor.Decrypt(encryptedId);
            int id;

            List<string> errors = new List<string>();
            bool succeeded = Int32.TryParse(decryptedId, out id);
            ArticleDetailsDTO data = null;

            if (succeeded)
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
                            Text = articleEntity.Text
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
            return GetAll(true, categoryName);
        }

        public DataServiceMessage<IEnumerable<ArticleListDTO>> GetAll()
        {
            return GetAll(false, null);
        }

        private DataServiceMessage<IEnumerable<ArticleListDTO>> GetAll(bool filter, string categoryName)
        {
            List<string> errors = new List<string>();
            bool succeeded = true;
            IEnumerable<ArticleListDTO> data = null;

            try
            {
                IEnumerable<ArticleEntity> articleEntities = filter ?
                    unitOfWork.Articles.GetAll(categoryEntity => categoryEntity.Category.Name == categoryName) :
                    unitOfWork.Articles.GetAll();
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
