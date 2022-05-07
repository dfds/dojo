using ObserverPattern.Abstract;

namespace ObserverPattern.Concrete
{
    internal class NATO : Observer
    {
        public override void Update(double distance)
        => Console.WriteLine($"NATO will deliver equipment to Ukraine\tDistance: {distance}");

    }
}
