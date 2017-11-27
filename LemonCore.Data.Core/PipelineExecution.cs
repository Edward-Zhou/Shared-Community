using LemonCore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LemonCore.Core
{
    public class Execution : IExecute
    {
        private Func<IDictionary<string, object>, Task<bool>> _block;

        public Execution(Func<IDictionary<string, object>, Task<bool>> block)
        {
            this._block = block;
        }

        public bool Run(IDictionary<string, object> namedParameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RunAsync(IDictionary<string, object> namedParameters)
        {
            return this._block(namedParameters);
        }
    }
}
