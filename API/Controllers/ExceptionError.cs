using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VocabularyAPI.Controllers
{
    public sealed class ExceptionError
    {
        public string Title { get; }

        public ExceptionError(string title)
        {
            Title = title;
        }
    }
}
