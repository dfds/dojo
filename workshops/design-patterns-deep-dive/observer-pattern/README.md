
Design Patterns - Observer Pattern
======================================

## Getting started
This tutorial will help you understand observer pattern with a real world example. Ensure that your machine has the tools.

### Prerequisites
* [Visual Studio](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2022)

## Observer Pattern
Oberser pattern is a behavioral pattern. You can use this pattern, when an object want to notify several objects about changes and need to inform them regardless of what the object is. This pattern also let us to add and remove easly an observer.

 There are three keywords to understand this pattern such as **"subject(target)"**, **"observer"** and **"Concrete"**.
* **Subject** is the target object
* **Observer** is the abstract class which might be an interface or an Abstract class that holds the reference of concrete observers.
*  **Concrete** is a class that is derived from an abstract class or is an implementation of an interface.

In this example, i simulated a real-world situation using DFDS ships:  
* The subject(target) is **Tower.cs**
* **Observer.cs** is the abstract class
* **DoverSeaways.cs**, **DelftSeaways.cs** and **DunkerqueSeaways.cs** are the concrete observers.

#### Observer.cs

```c#
//  This is the abstract class which will be inherited by oberservers.
public abstract class Observer
{
    public abstract void Update(bool IsAvailible);
}
```

#### Tower.cs
```c#
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
```

#### DelftSeaways.cs

```c#
internal class DelftSeaways : Observer
{
    public override void Update(int quantity)
    => Console.WriteLine($"DelftSeaways carried vehicles\t The rest: {quantity}");
}
```

#### Program.cs
```c#
Tower tower = new Tower();
tower.AddObserver(new DelftSeaways());
tower.Load(100);

// Add a new observer to carry more ships.
tower.AddObserver(new DoverSeaways());
tower.Load(100);

// Add a new observer to carry more ships.
tower.AddObserver(new DunkerqueSeaways());
tower.Load(200);

```

## Want to help make our training material better?
 * Want to **log an issue** or **request a new kata**? Feel free to visit our [GitHub site](https://github.com/dfds/dojo/issues).