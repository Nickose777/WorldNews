using System;
using System.Linq;
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

        public DataServiceMessage<IEnumerable<string>> GetAllNames()
        {
            List<string> errors = new List<string>();
            bool succeeded = true;
            IEnumerable<string> data = null;

            try
            {
                IEnumerable<CategoryEntity> categoryEntities = unitOfWork.Categories.GetAll();
                data = categoryEntities.Select(category => category.Name)
                    .OrderBy(category => category)
                    .ToList();
            }
            catch (Exception ex)
            {
                ExceptionMessageBuilder.FillErrors(ex, errors);
                succeeded = false;
            }

            return new DataServiceMessage<IEnumerable<string>>
            {
                Errors = errors,
                Succeeded = succeeded,
                Data = data
            };
        }

        public DataServiceMessage<IEnumerable<CategoryListDTO>> GetAll()
        {
            List<string> errors = new List<string>();
            bool succeeded = true;
            IEnumerable<CategoryListDTO> data = null;

            try
            {
                IEnumerable<CategoryEntity> categoryEntities = unitOfWork.Categories.GetAll();
                data = categoryEntities.Select(categoryEntity =>
                    new CategoryListDTO
                    {
                        Id = encryptor.Encrypt(categoryEntity.Id.ToString()),
                        Name = categoryEntity.Name,
                        NewsCount = categoryEntity.Articles.Count,
                        IsDisabled = categoryEntity.IsDisabled
                    })
                    .OrderBy(category => category.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                ExceptionMessageBuilder.FillErrors(ex, errors);
                succeeded = false;
            }

            return new DataServiceMessage<IEnumerable<CategoryListDTO>>
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
