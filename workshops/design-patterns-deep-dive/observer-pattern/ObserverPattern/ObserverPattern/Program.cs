
using ObserverPattern.Concrete;

// Observer Pattern
Russia RussiaFromTheNorth = new Russia();
RussiaFromTheNorth.AddObserver(new USA());
RussiaFromTheNorth.Move(100);

// Add a new observer before Russia's move
RussiaFromTheNorth.AddObserver(new NATO());
RussiaFromTheNorth.Move(100);

// Add a new observer before Russia's move
RussiaFromTheNorth.AddObserver(new Turkey());
RussiaFromTheNorth.Move(200);