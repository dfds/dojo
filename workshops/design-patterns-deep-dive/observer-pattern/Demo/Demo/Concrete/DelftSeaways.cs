using Demo.Abstract;

namespace Demo.Concrete
{
    internal class DelftSeaways : Observer
    {
        public override void Update(int quantity)
        => Console.WriteLine($"DelftSeaways carried vehicles\t The rest: {quantity}");
    }
}
