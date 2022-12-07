using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Strategy.Template
{
    public class TorpedoTemplate : TemplateAbstractClass
    {
        public override void SetBulletParams()
        {
            _bulletColor = Color.Black;
            _bulletHeight = 10;
            _bulletWidth = 20;
            _bulletSpeed = 50;
            _bulletDamage = 10;
            _bulletType = "torpedo";
        }
    }
}
