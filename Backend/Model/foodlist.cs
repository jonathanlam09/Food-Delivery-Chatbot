using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Backend.Model
{

    public class food_data
    {
        public string Stall { get; set; }
        public string ItemName { get; set; }
        public string Price { get; set; }
        public string Delivery { get; set; }
        public string Type { get; set; }
        public string Vegeterian { get; set; }
        public string Spicy { get; set; }
        public string Egg { get; set; }
        public string Recommended { get; set; }
    }

    public class price
    {
        public string foodprice { get; set; }
    }
}
