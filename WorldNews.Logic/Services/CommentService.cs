using System;
using System.Collections.Generic;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Comment;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEncryptor encryptor;

        public CommentService(IUnitOfWork unitOfWork, IEncryptor encryptor)
        {
            this.unitOfWork = unitOfWork;
            this.encryptor = encryptor;
        }

        public ServiceMessage Create(CommentCreateDTO commentDTO)
        {
            int articleId;
            string decryptedArticleId = encryptor.Decrypt(commentDTO.ArticleId);

            List<string> errors = new List<string>();
            bool succeeded = true;

            if (Int32.TryParse(decryptedArticleId, out articleId))
            {
                if (succeeded = Validate(commentDTO, errors))
                {
                    try
                    {
                        ArticleEntity articleEntity = unitOfWork.Articles.Get(articleId);
                        if (articleEntity != null)
                        {
                            ApplicationUser applicationUser = unitOfWork.ApplicationUsers.GetByLogin(commentDTO.AuthorLogin);
                            if (applicationUser != null)
                            {
                                CommentEntity commentEntity = new CommentEntity
                                {
                                    Article = articleEntity,
                                    Author = applicationUser,
                                    DateCreated = DateTime.Now,
                                    Text = commentDTO.Text
                                };

                                unitOfWork.Comments.Add(commentEntity);
                                unitOfWork.Commit();
                            }
                            else
                            {
                                succeeded = false;
                                errors.Add("User was not found");
                            }
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

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        private bool Validate(CommentCreateDTO commentDTO, ICollection<string> errors)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(commentDTO.Text))
            {
                isValid = false;
                errors.Add("Text cannot be empty");
            }

            if (String.IsNullOrEmpty(commentDTO.ArticleId))
            {
                isValid = false;
                errors.Add("Article has to be specified");
            }

            if (String.IsNullOrEmpty(commentDTO.AuthorLogin))
            {
                isValid = false;
                errors.Add("Author has to be specified");
            }

            return isValid;
        }
    }
}
