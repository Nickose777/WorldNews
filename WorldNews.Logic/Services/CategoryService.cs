using System;
using System.Linq;
using System.Collections.Generic;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.Category;
using WorldNews.Logic.Infrastructure;
using System.Linq.Expressions;

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
            bool succeeded = Validate(categoryDTO.Name, errors);

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

        public ServiceMessage Edit(CategoryEditDTO categoryDTO)
        {
            int id;
            string decryptedId = encryptor.Decrypt(categoryDTO.Id);

            List<string> errors = new List<string>();
            bool succeeded = true;

            if (Int32.TryParse(decryptedId, out id))
            {
                if (succeeded = Validate(categoryDTO.Name, errors))
                {
                    try
                    {
                        CategoryEntity categoryEntity = unitOfWork.Categories.Get(id);
                        if (categoryEntity != null)
                        {
                            categoryEntity.Name = categoryDTO.Name;
                            categoryEntity.IsDisabled = categoryDTO.IsDisabled;

                            unitOfWork.Commit();
                        }
                        else
                        {
                            succeeded = false;
                            errors.Add("Such category was not found");
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
                errors.Add("Such category was not found");
            }

            return new ServiceMessage
            {
                Errors = errors,
                Succeeded = succeeded
            };
        }

        public DataServiceMessage<CategoryEditDTO> Get(string categoryId)
        {
            int id;
            string decryptedId = encryptor.Decrypt(categoryId);

            List<string> errors = new List<string>();
            bool succeeded = true;
            CategoryEditDTO data = null;

            if (Int32.TryParse(decryptedId, out id))
            {
                try
                {
                    CategoryEntity categoryEntity = unitOfWork.Categories.Get(id);
                    data = new CategoryEditDTO
                    {
                        Id = categoryId,
                        Name = categoryEntity.Name,
                        IsDisabled = categoryEntity.IsDisabled
                    };
                }
                catch (Exception ex)
                {
                    ExceptionMessageBuilder.FillErrors(ex, errors);
                    succeeded = false;
                }
            }
            else
            {
                succeeded = false;
                errors.Add("Such category was not found");
            }

            return new DataServiceMessage<CategoryEditDTO>
            {
                Errors = errors,
                Succeeded = succeeded,
                Data = data
            };
        }

        public DataServiceMessage<IEnumerable<string>> GetEnabledNames()
        {
            return GetAllNames(categoryEntity => !categoryEntity.IsDisabled);
        }

        public DataServiceMessage<IEnumerable<string>> GetAllNames()
        {
            return GetAllNames(categoryEntity => true);
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

        private DataServiceMessage<IEnumerable<string>> GetAllNames(Expression<Func<CategoryEntity, bool>> expression)
        {
            List<string> errors = new List<string>();
            bool succeeded = true;
            IEnumerable<string> data = null;

            try
            {
                IEnumerable<CategoryEntity> categoryEntities = unitOfWork.Categories.GetAll(expression);
                data = categoryEntities.Select(categoryEntity => categoryEntity.Name)
                    .OrderBy(categoryName => categoryName)
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

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        private bool Validate(string categoryName, ICollection<string> errors)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(categoryName))
            {
                isValid = false;
                errors.Add("Name cannot be empty");
            }

            return isValid;
        }
    }
}
