using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TenderWin.Models;

namespace TenderWin
{
    public class ApiClient
    {
        public string TenderListUrl { get; set; } = "https://api.market.mosreg.ru/api/Trade/GetTradesForParticipantOrAnonymous";
        private HttpClient Client { get; set; }
        public ApiClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
      
        /// <summary>
        /// Get tender information by id
        /// </summary>
        /// <param name="tenderId"></param>
        /// <returns></returns>
        public async Task<TenderInfo> GetTenderInfo(int tenderId)
        {
            HtmlParser pars = new HtmlParser();
            string tenderDocumentationUrl = "https://api.market.mosreg.ru/api/Trade/" + tenderId + "/GetTradeDocuments";
            var tender = new TenderList() { Page = 1, ItemsPerPage = 10, Id = tenderId };
            var json = JsonConvert.SerializeObject(tender);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(TenderListUrl, data);
            string result = response.Content.ReadAsStringAsync().Result;

            var tenderInfoList = JsonConvert.DeserializeObject<TenderInfoList>(result);
            var currentTender = tenderInfoList.TanderInfo.FirstOrDefault();
            if(currentTender == null)
            {
                return new TenderInfo();
            }
            pars.Parse(currentTender);
            currentTender.Documentation = await GetTenderDocumentation(tenderDocumentationUrl);
            return currentTender;
        }

        /// <summary>
        /// Get all information about tender
        /// </summary>
        /// <param name="tenderInfo"></param>
        public void GetInfo(TenderInfo tenderInfo)
        {
            if(tenderInfo.Id == 0)
            {
                Console.WriteLine("Tender not found");
                return;
            }
            
            Console.WriteLine("Tender information:");
            Console.WriteLine($"Id: " + tenderInfo.Id);
            Console.WriteLine($"Current status: " + tenderInfo.TradeStateName);
            Console.WriteLine($"Customer name: " + tenderInfo.CustomerFullName);
            Console.WriteLine($"Start price: " + tenderInfo.InitialPrice);
            Console.WriteLine($"Publication date: " + tenderInfo.PublicationDate.ToLocalTime());
            Console.WriteLine($"Filling application end date: " + tenderInfo.FillingApplicationEndDate.ToLocalTime());
            if (tenderInfo.DeliveryAddress == "")
            {
                Console.WriteLine($"Delivery address: Отсутствует");
            }
            else Console.WriteLine($"Delivery address: " + tenderInfo.DeliveryAddress);
            Console.WriteLine($"List of lot positions: ");

            if (tenderInfo.Lots != null)
            {
                foreach (var lot in tenderInfo.Lots)
                {
                    Console.WriteLine($"    Lot name: " + lot.LotName);
                    Console.WriteLine($"    Unit: " + lot.Unit);
                    Console.WriteLine($"    Amount: " + lot.Amount);
                    Console.WriteLine($"    Cost per one: " + lot.CostPerOne);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"    Empty");
            }
            
            Console.WriteLine($"Documents list:");
            if (tenderInfo.Documentation != null)
            {
                foreach (var doc in tenderInfo.Documentation)
                {
                    Console.WriteLine($"    Document name: " + doc.FileName);
                    Console.WriteLine($"    URL: " + doc.Url);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"    Empty");
            }           
        }

        private async Task<List<Documentation>> GetTenderDocumentation(string tenderDocumentationUrl)
        {
            try
            {
                var content = await Client.GetStringAsync(tenderDocumentationUrl);
                var result = JsonConvert.DeserializeObject<List<Documentation>>(content);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }           
        }

    }
}
