using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Iterator
{
    class CustomCollection<T> : Aggregate
    {
        List<T> _collection = new List<T>();

        public List<T> getItems()
        {
            return _collection;
        }

        public void AddItem(T item)
        {
            this._collection.Add(item);
        }

        public override IEnumerator GetEnumerator()
        {
            return new InOrderIterator<T>(this);
        }
    }
}
