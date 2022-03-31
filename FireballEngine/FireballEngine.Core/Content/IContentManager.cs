using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireballEngine.Core.Content
{
    public interface IContentManager
    {
        Task Register<T>(string label, string source);
    }
}
