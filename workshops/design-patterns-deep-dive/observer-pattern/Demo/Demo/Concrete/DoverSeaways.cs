using Demo.Abstract;

namespace Demo.Concrete
{
    internal class DoverSeaways : Observer
    {
        public override void Update(int quantity)
        => Console.WriteLine($"DoverSeaways carried vehicles\t The rest: {quantity}");
    }
}
