﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Models.Entities
{
    public class Tag : EntityBase
    {
        public List<ImageTag> ImageTags { get; set; }
    }
}
