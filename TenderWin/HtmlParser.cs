using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TenderWin.Models;

namespace TenderWin
{
    public class HtmlParser
    {
        private HtmlDocument _doc = new HtmlDocument();
        public HtmlWeb web = new HtmlWeb();

        public bool Parse(TenderInfo tender)
        {
            string url = $"http://market.mosreg.ru/Trade/ViewTrade/{tender.Id}";
            try
            {
                _doc = web.Load(url);
                HtmlNodeCollection objectWithAddress = _doc.DocumentNode.SelectNodes("//div[contains(@class,'informationAboutCustomer__informationPurchase')]/child::div//p");
                tender.DeliveryAddress = objectWithAddress[5].InnerText;
                tender.Lots = new List<LotInfo>();
                HtmlNodeCollection objects = _doc.DocumentNode.SelectNodes("//div[contains(@class,'informationAboutCustomer__resultBlock objectPurchase')]/child::div//div//div//p");

                for (int i = 0; i < objects.Count; i++)
                {
                    LotInfo lotInfo = new LotInfo();
                    lotInfo.LotName = objects[i].LastChild.InnerText.Replace("\n", "").Replace("\r", "");
                    lotInfo.Unit = objects[i + 3].LastChild.InnerText.Replace(" ", "").Replace("\n", "").Replace("\r", "");
                    var amount = objects[i + 4].LastChild.InnerText.Replace(" ", "").Replace("\n", "").Replace("\r", "");
                    lotInfo.Amount = decimal.Parse(amount);
                    var costString = objects[i + 5].LastChild.InnerText.Replace(" ", "").Replace("\n", "").Replace("\r", "");
                    lotInfo.CostPerOne = decimal.Parse(costString);
                    tender.Lots.Add(lotInfo);
                    i = i + 6;
                }
                return true;
            }
            catch (WebException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return false;
            }                
        }
      
        
    }
}
