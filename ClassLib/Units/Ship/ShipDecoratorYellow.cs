namespace ClassLib.Units.Ship
{
    public class ShipDecoratorYellow : ShipDecorator
    {
        public ShipDecoratorYellow(Ship ship) : base(ship)
        {
        }

        public override Skin GetSkin()
        {
            return Skin.Yellow;
        }
    }
}
