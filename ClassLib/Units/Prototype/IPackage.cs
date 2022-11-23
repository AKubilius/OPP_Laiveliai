using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Units.Prototype
{
    public interface IPackage
    {
        void print();
        IPackage ShallowCopy();
        IPackage DeepCopy();
       
    }
}
