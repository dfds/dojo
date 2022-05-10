using Demo.Abstract;

namespace Demo.Concrete
{
    /// <summary>
    /// Each ship is observing the tower to get information of the vehicles ready for transfer in the port
    /// </summary>
    internal class Tower
    {
        private int _numberOfVehiclesReadyToShip = 1000;
        public int NumberOfVehiclesReadyToShip { get { return _numberOfVehiclesReadyToShip; } set { _numberOfVehiclesReadyToShip = value; } }
        private List<Observer> _observers;
        public Tower()
        {

            _observers = new List<Observer>();
        }

        public void Load(int quantityOfVehicle) { NumberOfVehiclesReadyToShip -= quantityOfVehicle; Notify(); }

        // Add new observers
        public void AddObserver(Observer observer) => _observers.Add(observer);

        // Remove an existing observer
        public void RemoveObserver(Observer observer) => _observers.Remove(observer);

        // Notify all the observers about the number of the vehicles which is ready to ship.
        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(NumberOfVehiclesReadyToShip);
            }
        }
    }
}
