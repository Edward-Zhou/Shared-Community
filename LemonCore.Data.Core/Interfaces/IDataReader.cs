using System;
using System.Collections.Generic;
using System.Text;

namespace LemonCore.Core.Interfaces
{
    public interface IDataReader<TRecord> : IDataReader
    {
        new TRecord Read();
    }

    public interface IDataReader : System.IDisposable
    {
        object Read();

        bool Next();
    }

}
