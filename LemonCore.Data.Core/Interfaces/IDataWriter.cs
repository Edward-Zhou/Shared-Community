using System;
using System.Collections.Generic;
using System.Text;

namespace LemonCore.Core.Interfaces
{
    public interface IDataWriter<TRecord> : System.IDisposable
    {
        void Write(TRecord record);
        void Write(IEnumerable<TRecord> records);
    }
}
