using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication1.Models
{
    public class Thing
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Thing (string name, int id = 0)
        {
            Id = id;
            Name = name;
        }

        public static List<Thing> GetAll()
        {
            List<Thing> allThings = new List<Thing> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM things;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int thingId = rdr.GetInt32(0);
                string thingName = rdr.GetString(1);
                Thing newThing = new Thing(thingName, thingId);
                allThings.Add(newThing);
            }
            conn.Close();
            if(conn!= null)
            {
                conn.Dispose();
            }
            return allThings;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO `things` (`name`) VALUES (@ThingName);";
            cmd.Parameters.AddWithValue("@ThingName", Name);
            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
