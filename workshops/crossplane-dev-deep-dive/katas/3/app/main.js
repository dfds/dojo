var pg = require("pg");
var fs = require('fs')
require('dotenv').config()

var PG_HOST = process.env.PG_HOST || ""
var PG_PORT = process.env.PG_PORT || ""
var PG_DB = process.env.PG_DB || ""
var PG_USER = process.env.PG_USER || ""
var PG_PASSWORD = process.env.PG_PASSWORD || ""

var client = new pg.Client({
  host: PG_HOST,
  port: PG_PORT,
  database: PG_DB,  
  user: PG_USER,
  password: PG_PASSWORD,
  
  ssl: {
    ca: fs.readFileSync('./eu-west-1-bundle.pem').toString()
  },
});

client.connect();

client.query('SELECT NOW()', (err, res) => {
  console.log(err, res)
  client.end()
})