namespace ClassLib.Units
{
    public abstract class Unit : IDisposable
    {
        public PictureBox Image { get; set; }
        public Label Label { get; set; }

        public virtual void Dispose()
        {
            Image.Dispose();
            Label.Dispose();
        }
    }
}
