using System;
using WorldNews.Logic.DTO.Comment;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Contracts.Services
{
    public interface ICommentService : IDisposable
    {
        ServiceMessage Create(CommentCreateDTO commentDTO);

        ServiceMessage Ban(CommentBanDTO commentDTO);
    }
}
