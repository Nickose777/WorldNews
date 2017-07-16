using System;
using System.Collections.Generic;

namespace WorldNews.Logic.Infrastructure
{
    static class ExceptionMessageBuilder
    {
        public static void FillErrors(Exception ex, ICollection<string> errors)
        {
            do
            {
                errors.Add(ex.Message);
                ex = ex.InnerException;
            } while (ex != null);
        }
    }
}
