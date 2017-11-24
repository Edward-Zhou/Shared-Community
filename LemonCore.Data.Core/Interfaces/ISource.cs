using System;
using System.Collections.Generic;
using System.Text;

namespace LemonCore.Core.Interfaces
{
    public interface ISource
    {
        Type SourceType { get; }
        Node Next { get; set; }
    }
}
