﻿using System;
using System.Collections.Generic;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Comment;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Services
{
    public class CommentService : ServiceBase, ICommentService
    {
        public CommentService(IUnitOfWork unitOfWork, IEncryptor encryptor)
            : base(unitOfWork, encryptor) { }

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

        public ServiceMessage Ban(CommentBanDTO commentDTO)
        {
            int commentId;
            string decryptedCommentId = encryptor.Decrypt(commentDTO.Id);

            int banReasonId;
            string decryptedBanReasonId = encryptor.Decrypt(commentDTO.BanReasonId);

            List<string> errors = new List<string>();
            bool succeeded = Validate(commentDTO, errors);

            if (!Int32.TryParse(decryptedCommentId, out commentId))
            {
                succeeded = false;
                errors.Add("Comment was not found");
            }

            if (!Int32.TryParse(decryptedBanReasonId, out banReasonId))
            {
                succeeded = false;
                errors.Add("Ban reason was not found");
            }

            if (succeeded && (succeeded = Validate(commentDTO, errors)))
            {
                try
                {
                    ModeratorEntity moderatorEntity = unitOfWork.Moderators.GetByLogin(commentDTO.ModeratorLogin);
                    if (moderatorEntity == null)
                    {
                        succeeded = false;
                        errors.Add("Moderator with such login was not found");
                    }

                    BanReasonEntity banReasonEntity = unitOfWork.Bans.Get(banReasonId);
                    if (moderatorEntity == null)
                    {
                        succeeded = false;
                        errors.Add("Moderator with such login was not found");
                    }

                    CommentEntity commentEntity = unitOfWork.Comments.Get(commentId);
                    if (commentEntity == null)
                    {
                        succeeded = false;
                        errors.Add("Comment was not found");
                    }

                    if (succeeded)
                    {
                        commentEntity.BanReason = banReasonEntity;
                        commentEntity.DateBanned = DateTime.Now;
                        commentEntity.ModeratorWhoBanned = moderatorEntity;

                        unitOfWork.Commit();
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

        private bool Validate(CommentBanDTO commentDTO, ICollection<string> errors)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(commentDTO.ModeratorLogin))
            {
                isValid = false;
                errors.Add("Moderator must be specified");
            }

            return isValid;
        }
    }
}
