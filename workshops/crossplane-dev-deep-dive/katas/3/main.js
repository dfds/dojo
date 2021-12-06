const { Client } = require("pg");

const client = new Client({
  user: 'admin',
  host: 'localhost', // test against a AWS postgres
  database: 'postgres',
  password: 'postgres',
  port: 5432,
})
  client.connect()
  client.query('SELECT NOW()', (err, res) => {
    console.log(err, res)
    client.end()
  })