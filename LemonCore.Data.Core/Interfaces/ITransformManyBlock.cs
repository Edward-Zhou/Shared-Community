using System;
using System.Collections.Generic;
using System.Text;

namespace LemonCore.Core.Interfaces
{
    public interface ITransformManyBlock<TSource, TTarget>
    {
        IEnumerable<TTarget> Transform(TSource record);
    }
}
