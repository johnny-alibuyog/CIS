﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Store.Domain.Entities;
using ServiceStack;

namespace CIS.Store.Services.Warrants
{
    public class PullWarrantResponse
    {
        public virtual Warrant[] Warrants { get; set; }
    }
}