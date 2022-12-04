using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Strategy.Template
{
    public class LaserTemplate : TemplateAbstractClass
    {
        public override void SetBulletParams()
        {
            _bulletColor = Color.GreenYellow;
            _bulletHeight = 5;
            _bulletWidth = 100;
            _bulletSpeed = 5;
            _bulletType = "laser";
        }
    }
}
