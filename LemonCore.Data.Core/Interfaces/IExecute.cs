using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LemonCore.Core.Interfaces
{
    public interface IExecute
    {
        Task<bool> RunAsync(IDictionary<string, object> namedParameters);

        bool Run(IDictionary<string, object> namedParameters);
    }
}
