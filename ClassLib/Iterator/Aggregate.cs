using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Iterator
{
    abstract class Aggregate : IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
    }
}
