using Demo.Abstract;

namespace Demo.Concrete
{
    internal class DunkerqueSeaways : Observer
    {
        public override void Update(int quantity)
        => Console.WriteLine($"DunkerqueSeaways carried vehicles\t The rest: {quantity}");
    }
}
