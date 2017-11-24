using System;
using System.Collections.Generic;
using System.Text;

namespace LemonCore.Core.Interfaces
{
    public interface ITransformBlock<ISource, ITarget>
    {
        ITarget Transform(ISource record);
    }
}
