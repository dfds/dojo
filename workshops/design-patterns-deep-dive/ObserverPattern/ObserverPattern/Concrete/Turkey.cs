using ObserverPattern.Abstract;

namespace ObserverPattern.Concrete
{
    internal class Turkey : Observer
    {
        public override void Update(double distance)
        => Console.WriteLine($"Turkey will deliver TB2 to Ukraine\tDistance: {distance}");

    }
}
