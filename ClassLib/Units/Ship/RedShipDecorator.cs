namespace ClassLib.Units.Ship
{
    public class RedShipDecorator : ShipDecorator
    {
        public RedShipDecorator(Ship ship) : base(ship)
        {
        }

        public override Skin GetSkin()
        {
            return Skin.Red;
        }
    }
}
