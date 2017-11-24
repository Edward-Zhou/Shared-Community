using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LemonCore.Core.Models
{
    public class DataColumn
    {
        public string Name { get; set; }
        public DbType DbType { get; set; }
    }

}
