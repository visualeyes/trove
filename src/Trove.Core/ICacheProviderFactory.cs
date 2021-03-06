﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trove.Core {
    public interface ICacheProviderFactory {
        ICacheProvider<V> GetCacheProvider<V>(string name) where V : class;
    }
}
