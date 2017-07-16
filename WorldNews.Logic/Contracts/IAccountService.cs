using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldNews.Logic.DTO.Registration;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts
{
    public interface IAccountService : IDisposable
    {
        ServiceMessage InitializeRoles();

        ServiceMessage RegisterUser(UserRegisterDTO userDTO);
    }
}
