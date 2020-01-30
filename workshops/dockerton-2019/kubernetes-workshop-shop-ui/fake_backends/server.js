const path = require("path");
const express = require("express");
const app = express();

app.get("/api/customers/:id", (req, res) => {
    res.sendFile(path.join(__dirname, "customer.json"));
});

app.get("/api/orders", (req, res) => {
    res.sendFile(path.join(__dirname, "orders.json"));
});

app.get("/api/customers/:id/recommendations", (req, res) => {
    res.sendFile(path.join(__dirname, "recommendations.json"));
});

app.listen(8080, () => {
    console.log("Fake backend is now listening...");
});