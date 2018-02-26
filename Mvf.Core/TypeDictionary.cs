using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvf.Core
{
    public class TypeDictionary<TValue> : Dictionary<Type, TValue>
    {
        public TValue this[Type type]
        {
            get
            {
                foreach (var key in base.Keys)
                {
                    if (key.IsAssignableFrom(type))
                    {
                        return base[key];
                    }
                }

                return default(TValue);
            }
        }


    }
}
