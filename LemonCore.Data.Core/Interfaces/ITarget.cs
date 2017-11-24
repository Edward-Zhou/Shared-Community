using System;
using System.Collections.Generic;
using System.Text;

namespace LemonCore.Core.Interfaces
{
    public interface ITarget
    {
        Type TargetType { get; }
        Node Prev { get; set; }
    }
}
