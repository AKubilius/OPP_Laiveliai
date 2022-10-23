namespace ClassLib.Units.Ship
{
    public abstract class ShipDecorator : IShip
    {
        public Ship Ship { get; }

        public ShipDecorator(Ship ship)
        {
            Ship = ship;
        }

        public abstract Skin GetSkin();
    }
}
