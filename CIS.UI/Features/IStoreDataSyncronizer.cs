﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS.UI.Features
{
    public interface IStoreDataSyncronizer
    {
        bool WorkInProgress { get; }
        void SyncronizeData();
    }
}
