﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedCommunity.Tasks.Pattern
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
