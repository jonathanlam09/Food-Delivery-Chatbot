using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Backend.Model;
using MySqlConnector;

namespace Backend.Db
{
    public class DbSetup
    {
        public static async Task<List<food_data>> ReadTable()
        {
            //Initialize MySQL connection
            var connection = new MySqlConnection("server = localhost; user = root; password = xxxxxxxx ; database = nott-a-code");
            await connection.OpenAsync();
            //Check connection state 
            if (connection.State == System.Data.ConnectionState.Open)
            {
                Debug.WriteLine("Connected to Database");
            }
            else
            {
                Debug.WriteLine("Not Connected to Database");
            }
            //Initialize MySQL commands
            var cmd = new MySqlCommand();
            //Initialize SQL code to query data from table
            cmd.CommandText = @"SELECT * FROM food_data";
            cmd.Connection = connection;
            var reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();
            List<food_data> foodlist = new List<food_data>();
            //Get the first row of data using .HasRows 
            //Data are set to model "food_data"
            if (reader.HasRows)
            {
                food_data data = new food_data();
                data.Stall = reader["Stall"].ToString();
                data.ItemName = reader["ItemName"].ToString();
                data.Price = reader["Price"].ToString();
                data.Delivery = reader["Delivery"].ToString();
                data.Type = reader["Type"].ToString();
                data.Vegeterian = reader["Vegeterian"].ToString();
                data.Spicy = reader["Spicy"].ToString();
                data.Egg = reader["Egg"].ToString();
                data.Recommended = reader["Recommended"].ToString();
                foodlist.Add(data);
            }
            //Get the rest of the data row using .Red() and while loop.
            while (reader.Read())
            {
                food_data data = new food_data();
                data.Stall = reader["Stall"].ToString();
                data.ItemName = reader["ItemName"].ToString();
                data.Price = reader["Price"].ToString();
                data.Delivery = reader["Delivery"].ToString();
                data.Type = reader["Type"].ToString();
                data.Vegeterian = reader["Vegeterian"].ToString();
                data.Spicy = reader["Spicy"].ToString();
                data.Egg = reader["Egg"].ToString();
                data.Recommended = reader["Recommended"].ToString();
                foodlist.Add(data);
            }
            //return foodlist 
            return foodlist;
        }  
    }
}
