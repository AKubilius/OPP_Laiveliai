using ClassLib.Units.Ship;

namespace Client.Models
{
    public class RedShip : ShipDecorator
    {
        public RedShip(Ship ship) : base(ship)
        {
        }

        public override Skin GetSkin()
        {
            return Skin.Red;
        }
    }
}
