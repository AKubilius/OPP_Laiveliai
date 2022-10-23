namespace ClassLib.Units.Ship
{
    public class ShipDecoratorBlue : ShipDecorator
    {
        public ShipDecoratorBlue(Ship ship) : base(ship)
        {
        }

        public override Skin GetSkin()
        {
            return Skin.Blue;
        }
    }
}
