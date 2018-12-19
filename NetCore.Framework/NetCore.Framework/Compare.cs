using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Framework
{
    public class Compare<T, C> : IEqualityComparer<T>
    {
        private Func<T, C> _getField;

        public Compare(Func<T, C> getfield)
        {
            this._getField = getfield;
        }

        public bool Equals(T x, T y)
        {
            return EqualityComparer<C>.Default.Equals(_getField(x), _getField(y));
        }

        public int GetHashCode(T obj)
        {
            return EqualityComparer<C>.Default.GetHashCode(this._getField(obj));
        }
    }
}