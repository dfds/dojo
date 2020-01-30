# Shop UI

## Development

### Fake backend for development
A fake backend that implements all three service contracts and returns stubbed data can be started by navigating to the `fake_backends` folder in the repository root. Then the following command will start the fake backend server:

```shell
$ npm start
```

This will serve up a backend on `http://localhost:8080` that supports the routes defined in the api contracts.

The data returned is defined in `json` files located in the `fake_backends` folder. The following table describes the relationship between services and `json` files:

| API | `json` file with stubbed data |
|-----|-------------------------------|
| CRM service | `customer.json` |
| Order service | `orders.json` |
| Recommendation service | `recommendations.json` |


### Frontend development
The backend that serves the frontend can be started by navigating to the `src` folder in the repository root. Then the following command will start the backend:

```shell
$ npm start
```

The website can be reached on `http://localhost:57010/` in a browser.