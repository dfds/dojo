using ObserverPattern.Abstract;

namespace ObserverPattern.Concrete
{
    internal class USA : Observer
    {
        public override void Update(double distance)
        => Console.WriteLine($"USA will impose sanctions \tDistance: {distance}");

    }
}
