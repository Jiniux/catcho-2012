using System.ComponentModel.DataAnnotations.Schema;
using osuserver2012.MySql;

await (new Server("127.0.0.1", 13381, new Database("localhost", "u", "p", "d"))).Start();