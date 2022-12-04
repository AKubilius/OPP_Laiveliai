using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Strategy.Template
{
    public class MachinegunTemplate : TemplateAbstractClass
    {
        public override void SetBulletParams()
        {
            _bulletColor = Color.Wheat;
            _bulletHeight = 5;
            _bulletWidth = 5;
            _bulletSpeed = 20;
            _bulletType = "machinegun";
        }
    }
}
