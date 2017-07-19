using System;
using System.Collections.Generic;
using System.Linq;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.Contracts.Services;
using WorldNews.Logic.DTO.BanReason;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Services
{
    public class BanReasonService : IBanReasonService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEncryptor encryptor;

        public BanReasonService(IUnitOfWork unitOfWork, IEncryptor encryptor)
        {
            this.unitOfWork = unitOfWork;
            this.encryptor = encryptor;
        }

        public ServiceMessage Create(BanReasonCreateDTO banReasonDTO)
        {
            List<string> errors = new List<string>();
            bool succeeded = Validate(banReasonDTO.Name, errors);

            if (succeeded)
            {
                try
                {
                    bool exists = unitOfWork.Bans.Exists(banReasonDTO.Name);
                    if (!exists)
                    {
                        BanReasonEntity banReasonEntity = new BanReasonEntity
                        {
                            Name = banReasonDTO.Name,
                            IsEnabled = true
                        };

                        unitOfWork.Bans.Add(banReasonEntity);
                        unitOfWork.Commit();
                    }
                    else
                    {
                        succeeded = false;
                        errors.Add("Such reason alredy exists");
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
                Errors = errors,
                Succeeded = succeeded
            };
        }

        public DataServiceMessage<IEnumerable<BanReasonListDTO>> GetAll()
        {
            List<string> errors = new List<string>();
            bool succeeded = true;
            IEnumerable<BanReasonListDTO> data = null;

            try
            {
                IEnumerable<BanReasonEntity> banReasonEntities = unitOfWork.Bans.GetAll();
                data = banReasonEntities.Select(banReasonEntity =>
                    new BanReasonListDTO
                    {
                        Id = encryptor.Encrypt(banReasonEntity.Id.ToString()),
                        Name = banReasonEntity.Name,
                        IsEnabled = banReasonEntity.IsEnabled
                    })
                    .OrderBy(ban => ban.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                succeeded = false;
                ExceptionMessageBuilder.FillErrors(ex, errors);
            }

            return new DataServiceMessage<IEnumerable<BanReasonListDTO>>
            {
                Data = data,
                Errors = errors,
                Succeeded = succeeded
            };
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        private bool Validate(string name, ICollection<string> errors)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(name))
            {
                isValid = false;
                errors.Add("Name cannot be empty");
            }

            return isValid;
        }
    }
}
