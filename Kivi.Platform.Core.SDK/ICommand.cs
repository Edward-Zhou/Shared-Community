using System;
using System.Collections.Generic;
using System.Text;

namespace Kivi.Platform.Core.SDK
{
    public interface ICommand
    {
        bool Run(string arguments);
    }
}
