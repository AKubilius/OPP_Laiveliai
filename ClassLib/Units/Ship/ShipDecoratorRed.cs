namespace ClassLib.Units.Ship
{
    public class ShipDecoratorRed : ShipDecorator
    {
        public ShipDecoratorRed(Ship ship) : base(ship)
        {
        }

        public override Skin GetSkin()
        {
            return Skin.Red;
        }
    }
}
