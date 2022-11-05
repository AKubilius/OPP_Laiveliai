using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Observer
{
    public interface ISubject
    {
        public void Subscribe(Player player);
        public void Unsubscribe(Player player);
        public Task Notify();
    }
}
