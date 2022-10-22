namespace ClassLib.Units.Ship
{
    public class ShipDecorator : IShip
    {
        public Ship Ship { get; }

        public ShipDecorator(Ship ship)
        {
            Ship = ship;
        }

        public virtual Skin GetSkin()
        {
            return Ship.GetSkin();
        }
    }
}
