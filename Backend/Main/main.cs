using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Backend.Db;
using Backend.Model;
using Google.Cloud.Dialogflow.V2;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Backend.Main
{
    public class main : Imain
    {
        public string _sessionId = System.DateTime.Now.Ticks.ToString();
        private string _projectId = "foodorderingchatbot-eq9g";
        string APIkey = @"C:\Users\jonat\source\repos\Backend\Backend\APIkey.json";
        private SessionsClient _sessionsClient;
        private SessionName _sessionName;
        private static string prevIntent = "";

        public void SetEnvironmentVariable()
        {
            try
            {
                System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", APIkey);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        //Function to create Dialogflow Client Session
        private async Task CreateSession()
        {
            _sessionsClient = await SessionsClient.CreateAsync();
            _sessionName = new SessionName(_projectId, _sessionId);
        }

        public async Task<List<string>> CheckIntent(string userInput, string LanguageCode = "en-US")
        {
            //Connect to GCP credentials
            SetEnvironmentVariable();

            //Await Client Session to be created
            await CreateSession();

            //Declare Input value for dialogflow API
            QueryInput queryInput = new QueryInput();
            var queryText = new TextInput();
            queryText.Text = userInput;
            queryInput.Text = queryText;
            queryText.LanguageCode = LanguageCode;

            // Make Intent detect request and await response
            DetectIntentResponse response = await _sessionsClient.DetectIntentAsync(_sessionName, queryInput);
            var foodlist = await DbSetup.ReadTable();
            List<string> returntext = new List<string>();

            if (prevIntent == "")
            {
                //if else function to filter response according to detected intent
                //Menu Intent//Return Full Menu
                if (response.QueryResult.Intent.DisplayName == "Menu")
                {
                    //Declare an integer variable for indexing
                    int i = 1;
                    //Add the Fulfillment Text declared in dialogflow cloud
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    //For each "food_data" class inside of foodlist which is the return value from Database
                    foreach (food_data food in foodlist)
                    {
                        //Create a return string 
                        string menu = i + ") " + food.Stall + "\r\n" +
                                        food.ItemName + "\r\n" +
                                        "RM" + food.Price + "\r\n" +
                                        "Delivery: " + food.Delivery + "\r\n\n";
                        //Index increment per foreach loop
                        i = i + 1;
                        //Append created string into "returntext" list
                        returntext.Add(menu);
                    }
                    //if no menu in the list
                    if (returntext.Count == 1)
                    {
                        //append another fulfillment text declared in dialogflow cloud 
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Recommend Intent//Return recommended dishes
                else if (response.QueryResult.Intent.DisplayName == "Recommended")
                {
                    int i = 1;
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    foreach (food_data food in foodlist)
                    {
                        if (food.Recommended == "yes")
                        {
                            string menu = i + ") " + food.Stall + "\r\n" +
                                       food.ItemName + "\r\n" +
                                       "RM" + food.Price + "\r\n" +
                                       "Delivery: " + food.Delivery + "\r\n\n";
                            i = i + 1;
                            returntext.Add(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //"Malay" Intent//Filter menu and return list of food from Malay stall
                else if (response.QueryResult.Intent.DisplayName == "Malay")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Stall == "Malay")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //"Mamak" Intent//Filter menu and return list of food from Mamak stall
                else if (response.QueryResult.Intent.DisplayName == "Mamak")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Stall == "Mamak")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Japanese Intent//Filter menu and return list of food from Japanese stall
                else if (response.QueryResult.Intent.DisplayName == "Korean")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Stall == "Korean")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Korean Intent//Filter menu and return list of food from Korean stall
                else if (response.QueryResult.Intent.DisplayName == "Japanese")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Stall == "Japanese")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Beverage Intent//Filter menu and return list of drinks from Beverage and Mamak stall
                else if (response.QueryResult.Intent.DisplayName == "Beverage")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Type == "drinks" || foodlist[a].Type == "coffee")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Coffee Intent//Filter menu and return list of coffees
                else if (response.QueryResult.Intent.DisplayName == "Coffee")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Type == "coffee")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Juice Intent//Filter menu and return list of juices
                else if (response.QueryResult.Intent.DisplayName == "Juice")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Type == "juice")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Vegan Intent//Filter menu and return list of vegan food
                else if (response.QueryResult.Intent.DisplayName == "Vegan")
                {

                    if (response.QueryResult.FulfillmentMessages[0].Text.Text_[0] == "Can you eat eggs?")
                    {
                        prevIntent = response.QueryResult.Intent.DisplayName;
                        returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    }
                    else
                    {
                        returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                        int i = 1;
                        for (int a = 0; a < foodlist.Count; a++)
                        {
                            if (foodlist[a].Vegeterian == "yes")
                            {
                                string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                                "RM" + foodlist[a].Price + "\r\n" +
                                                "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                                returntext.Add(menu);
                                i = i + 1;
                                Debug.WriteLine(menu);
                            }
                        }
                        if (returntext.Count == 1)
                        {
                            returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                        }
                    }  
                }
                //Spicy Intent//Filter menu and return list of spicy food
                else if (response.QueryResult.Intent.DisplayName == "Spicy")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Spicy == "yes")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Below Intent//Filter menu and return list of food that is below a certain amount
                else if (response.QueryResult.Intent.DisplayName == "Below")
                {
                    if (response.QueryResult.FulfillmentMessages[0].Text.Text_[0] == "What is your budget?")
                    {
                        returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                        prevIntent = "Below";
                    }
                    else
                    {
                        string mynumber = Regex.Replace(response.QueryResult.FulfillmentMessages[0].Text.Text_[0], @"\D", "");
                        int i = 1;
                        if (mynumber != "")
                        {
                            double budget = Double.Parse(mynumber);
                            returntext.Add("Sure! Here is a list of food that is below RM" + mynumber + "\r\n\n");
                            for (int a = 0; a < foodlist.Count; a++)
                            {
                                if (Double.Parse(foodlist[a].Price) < budget)
                                {
                                    string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                                    "RM" + foodlist[a].Price + "\r\n\n";
                                    returntext.Add(menu);
                                    i = i + 1;
                                }
                            }
                            Debug.WriteLine(returntext.Count);
                            if (returntext.Count == 1)
                            {
                                returntext[0] = "I'm sorry. We do not have any item below RM" + mynumber + ".\r\n\n";
                            }
                        }
                    }
                }
                //Spicy Vegeterian//Filter and return spicy vegeterian food
                else if (response.QueryResult.Intent.DisplayName == "Spicy Vegeterian")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Vegeterian == "yes" && foodlist[a].Spicy == "yes")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Spicy Japanese//Filter and return spicy japanese food
                else if (response.QueryResult.Intent.DisplayName == "Spicy Japanese")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Stall == "Japanese" && foodlist[a].Spicy == "yes")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Spicy Japanese//Filter and return spicy japanese food
                else if (response.QueryResult.Intent.DisplayName == "Spicy Malay")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Stall == "Malay" && foodlist[a].Spicy == "yes")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Non Spicy Vegeterian//Filter and return spicy vegeterian food
                else if (response.QueryResult.Intent.DisplayName == "Non Spicy Vegeterian")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Type == "Vegeterian" && foodlist[a].Spicy == "no")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Non Spicy Japanese//Filter and return spicy japanese food
                else if (response.QueryResult.Intent.DisplayName == "Non Spicy Japanese")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Stall == "Japanese" && foodlist[a].Spicy == "no")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Non Spicy Korean//Filter and return spicy korean food
                else if (response.QueryResult.Intent.DisplayName == "Non Spicy Japanese")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Stall == "Japanese" && foodlist[a].Spicy == "no")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n" +
                                            "Delivery: " + foodlist[a].Delivery + "\r\n\n"; ;
                            returntext.Add(menu);
                            i = i + 1;
                            Debug.WriteLine(menu);
                        }
                    }
                    if (returntext.Count == 1)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Delivery Intent//Filter and return food available for delivery
                else if (response.QueryResult.Intent.DisplayName == "Delivery")
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0] + "\r\n\n");
                    int i = 1;
                    for (int a = 0; a < foodlist.Count; a++)
                    {
                        if (foodlist[a].Vegeterian == "yes" && foodlist[a].Spicy == "yes")
                        {
                            string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                            "RM" + foodlist[a].Price + "\r\n\n";
                            returntext.Add(menu);
                            i = i + 1;
                        }
                    }
                    if (returntext.Count < 2)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Order Intent//Return menu item for selection 
                else if (userInput.ToUpper() == "ORDER")
                {
                    foreach (food_data food in foodlist)
                    {
                        string menu = food.ItemName;
                        returntext.Add(menu);
                        Debug.WriteLine(menu);
                    }
                    if (returntext.Count < 2)
                    {
                        returntext[0] = response.QueryResult.FulfillmentMessages[1].Text.Text_[0];
                    }
                }
                //Intents without needs of filtering data//Return Intent's Fulfillment Text
                else
                {
                    returntext.Add(response.QueryResult.FulfillmentMessages[0].Text.Text_[0]);
                }
            }
            // else function for if string "prevIntent is not an empty string"
            else
            {
                //If "prevIntent" is "Vegan"
                if (prevIntent == "Vegan")
                {
                    //Reset "prevIntent"
                    prevIntent = "";
                    int i = 1;
                    //Check the new detected Intent.
                    //If new detected intent is "Yes", then filter and return vegeterian food with eggs.
                    if(response.QueryResult.Intent.DisplayName == "Yes")
                    {
                        returntext.Add("Sure! Here's a list of vegeterian food with eggs.\r\n\n");
                        foreach (food_data food in foodlist)
                        {
                            if (food.Vegeterian == "yes" && food.Egg == "yes")
                            {
                                string menu = i + ") " + food.ItemName + "\r\n" +
                                            "RM" + food.Price + "\r\n\n";
                                returntext.Add(menu);
                                i = i + 1;
                            }
                        }
                    }
                    //If new detected intent is "No", then filter and return vegeterian food with eggs.
                    else if (response.QueryResult.Intent.DisplayName == "No")
                    {
                        returntext.Add("Sure! Here's a list of vegeterian food without eggs.\r\n\n");
                        foreach (food_data food in foodlist)
                        {
                            if (food.Vegeterian == "yes" && food.Egg == "no")
                            {
                                string menu = i + ") " + food.ItemName + "\r\n" +
                                            "RM" + food.Price + "\r\n\n";
                                returntext.Add(menu);
                                i = i + 1;
                            }
                        }
                    }
                }
                //If "prevIntent" is "Below"
                else if (prevIntent == "Below")
                {
                    //Reset "prevIntent"
                    prevIntent = "";
                    //Two scenarios
                    //Scenario 1 : User typed in numeric form such as 1,2,3..
                    //Scenario 2 : Yser typed in word form such as one, two three...
                    //Check the scenarios by filtering the userInput.
                    //Filter out integers found in userInput
                    string mynumber = Regex.Replace(userInput, @"\D", "");
                    int i = 1;
                    //string array for scenario 2 
                    string[] wordnumbers = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
                    //Check scenario
                    //Scenario 1: If there is integer found in userInput, filter out the food list below the given integer and return. 
                    if (mynumber != "")
                    {
                        double budget = Double.Parse(mynumber);
                        returntext.Add("Sure! Here is a list of food that is below RM" + mynumber + "\r\n\n");
                        for (int a = 0; a < foodlist.Count; a++)
                        {
                            if (Double.Parse(foodlist[a].Price) < budget)
                            {
                                string menu = i + ") " + foodlist[a].ItemName + "\r\n" +
                                                "RM" + foodlist[a].Price + "\r\n\n";
                                returntext.Add(menu);
                                i = i + 1;
                            }
                        }
                        Debug.WriteLine(returntext.Count);
                        if (returntext.Count == 1)
                        {
                            returntext[0] = "I'm sorry. We do not have any item below RM" + mynumber + ".\r\n\n";
                        }
                    }
                    //Scenario 2: If there is no integer found in userInput string.
                    else 
                    {
                        //Go through each element in the wordnumbers array
                        for (int a = 0; a < wordnumbers.Length; a++)
                        {
                            //if any of the element in the word array is within userInput string,
                            //Use corresponding element index as budget since the index is the numerical form of each element in the list.
                            if (userInput.Contains(wordnumbers[a]))
                            {
                                returntext.Add("Sure! Here's a list of food below RM" + a + " .\r\n\n");
                                for (int b = 0; b < foodlist.Count; b++)
                                {
                                    if (double.Parse(foodlist[b].Price) < a)
                                    {
                                        string menu = i + ") " + foodlist[b].ItemName + "\r\n" +
                                                        "RM" + foodlist[b].Price + "\r\n\n";
                                        returntext.Add(menu);
                                        i = i + 1;
                                    }
                                }
                            }
                            if (returntext.Count == 1)
                            {
                                returntext[0] = "I'm sorry. We do not have any item below RM" + mynumber + ".\r\n\n";
                            }
                        }
                    }
                }
            }
            Debug.WriteLine(response.QueryResult.Intent.DisplayName);
            //return the returntext list to frontend
            return returntext;
        }

        //GetPriceList function
        public async Task<List<string>> GetPriceList()
        {
            //Read database table
            var foodlist = await DbSetup.ReadTable();
            //Instantiate new list
            List<string> returntext = new List<string>();
            //Filter and return foodprice of each food.
            foreach (food_data food in foodlist)
            {
                returntext.Add(food.Price);
            }
            return returntext;
        }
    }
}
