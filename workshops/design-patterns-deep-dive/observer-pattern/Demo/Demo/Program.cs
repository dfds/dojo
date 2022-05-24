using Demo.Concrete;

// Observer Pattern
Tower tower = new Tower();
tower.AddObserver(new DelftSeaways());
tower.Load(100);

// Add a new observer to carry more ships.
tower.AddObserver(new DoverSeaways());
tower.Load(100);

// Add a new observer to carry more ships.
tower.AddObserver(new DunkerqueSeaways());
tower.Load(200);
