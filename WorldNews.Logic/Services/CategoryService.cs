using System;
using System.Collections.Generic;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Category;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEncryptor encryptor;

        public CategoryService(IUnitOfWork unitOfWork, IEncryptor encryptor)
        {
            this.unitOfWork = unitOfWork;
            this.encryptor = encryptor;
        }

        public ServiceMessage Create(CategoryCreateDTO categoryDTO)
        {
            List<string> errors = new List<string>();
            bool succeeded = Validate(categoryDTO, errors);

            if (succeeded)
            {
                try
                {
                    bool exists = unitOfWork.Categories.Exists(categoryDTO.Name);
                    if (!exists)
                    {
                        CategoryEntity categoryEntity = new CategoryEntity
                        {
                            Name = categoryDTO.Name
                        };

                        unitOfWork.Categories.Add(categoryEntity);
                        unitOfWork.Commit();
                    }
                    else
                    {
                        succeeded = false;
                        errors.Add("Category with such name already exists");
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

        private bool Validate(CategoryCreateDTO categoryDTO, List<string> errors)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(categoryDTO.Name))
            {
                isValid = false;
                errors.Add("Name cannot be empty");
            }

            return isValid;
        }
    }
}
