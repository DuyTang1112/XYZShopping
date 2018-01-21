﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYZShoppingMVCApp_DuyTang.Cache
{
    public interface IWebCache
    {
        void Remove(string key);
        void Store (string key, object obj);
        T Retrieve<T>(string key);
    }
}
