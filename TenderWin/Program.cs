using System;
using System.Net.Http;
using System.Threading.Tasks;
using TenderWin.Models;

namespace TenderWin
{
    class Program
    {
        static async Task Main()
        {
            ApiClient client = new ApiClient();
            var _tenderCache = new MemoryCache<TenderInfo>();
            int tenderId;
            
            while (true)
            {                
                Console.WriteLine("Enter tender number:");

                while (!int.TryParse(Console.ReadLine(), out tenderId))
                {
                    Console.WriteLine("Input Error! Try again:");
                }

                try
                {
                    var searchedTender = await _tenderCache.GetOrCreate(tenderId, async () => await client.GetTenderInfo(tenderId));
                    client.GetInfo(searchedTender);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                
            }
            
        }
    }
}
