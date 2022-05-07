using ObserverPattern.Abstract;

namespace ObserverPattern.Concrete
{

    /// <summary>
    /// Everyone is watching Russia's next move. This is the target.
    /// </summary>
    internal class Russia
    {
        private double distance = 1000;
        public double Distance { get { return distance; } set { distance = value; } }
        private List<Observer> _observers;
        public Russia()
        {

            _observers = new List<Observer>();
        }

        public void Move(double move) { Distance -= move; Notify(); }

        // Add new observers
        public void AddObserver(Observer observer) => _observers.Add(observer);

        // Remove an existing observer
        public void RemoveObserver(Observer observer) => _observers.Remove(observer);

        // Notify all the observers about the distance between Russia and Ukraine.
        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(Distance);
            }
        }
    }
}

