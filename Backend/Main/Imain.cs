using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Dialogflow.V2;

namespace Backend.Main
{
    public interface Imain
    {
        Task<List<string>> CheckIntent(string userInput, string LanguageCode = "en-US");
        Task<List<string>> GetPriceList();
    }
}