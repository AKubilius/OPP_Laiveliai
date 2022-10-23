namespace ClassLib.Units.Ship
{
    public class ShipDecoratorWhite : ShipDecorator
    {
        public ShipDecoratorWhite(Ship ship) : base(ship)
        {
        }

        public override Skin GetSkin()
        {
            return Skin.White;
        }
    }
}
