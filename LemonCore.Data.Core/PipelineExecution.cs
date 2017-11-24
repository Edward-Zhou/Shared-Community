using LemonCore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LemonCore.Core
{
    public class Execution : IExecute
    {
        private Func<Task<bool>> _block;

        public Execution(Func<Task<bool>> block)
        {
            _block = block;
        }

        public bool Run(IDictionary<string, object> namedParameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RunAsync()
        {
            return _block();
        }

        public Task<bool> RunAsync(IDictionary<string, object> namedParameters)
        {
            throw new NotImplementedException();
        }
    }
}
