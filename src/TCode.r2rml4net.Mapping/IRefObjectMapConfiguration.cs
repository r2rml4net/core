﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCode.r2rml4net.Mapping
{
    public interface IRefObjectMapConfiguration
    {
        void AddJoinCondition(string childColumn, string parentColumn);
    }
}
