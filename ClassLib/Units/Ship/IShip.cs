namespace ClassLib.Units.Ship
{
    public interface IShip
    {
        public Skin GetSkin();
    }

    public enum Skin
    {
        White,
        Red,
        Blue,
        Yellow
    }
}
