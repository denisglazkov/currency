using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Currency
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string[] listBank =
            {
                "ПриватБанк",
                "Ощадбанк",
                "Укрэксимбанк",
                "Проминвестбанк",
                "Укрсоцбанк UniCredit Bank TM",
                "Райффайзен Банк Аваль",
                "Сбербанк",
                "Альфа-Банк",
                "УкрСиббанк",
                "ПУМБ"
            };
            double curSum = 0;
            double curAvg = 0;
            double curAsk = 0;
            double curBid = 0;

            string uri = "http://resources.finance.ua/ru/public/currency-cash.json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Proxy=new WebProxy("192.168.1.4", 3128);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string strFile = Convert.ToString(reader.ReadToEnd());

            List<Bank> initBank = Bank.GetBanks(strFile);
            List<Bank> _listBank = initBank.Where(x => listBank.Contains(x.title)).ToList();

            foreach (Bank bankItem in _listBank)
            {
                if (bankItem.currencies.ContainsKey("USD"))
                {
                    double ask = Convert.ToDouble(bankItem.currencies["USD"].ask.Replace(".", ","));
                    double bid = Convert.ToDouble(bankItem.currencies["USD"].bid.Replace(".", ","));
                    Console.WriteLine("{0}: ask: {1}; bid: {2}", bankItem.title, ask, bid);
                    curSum += ask + bid;
                }
                else
                {
                    Console.WriteLine("{0} - У этого банка нет такой валюты!!!!!!!!!!!!!!!!!!!!!!!!", bankItem.title);
                }
            }

            if (initBank.Count < 11)
            {
                Console.WriteLine("Ярик, еще не все банки дали курс!!!!!!!");
            }

            curAvg = Math.Round(curSum / (_listBank.Count * 2), 2);
            curAsk = Math.Round(curAvg * (1 - 0.0325), 2);
            curBid = Math.Round(curAvg * (1 + 0.0325), 2);

            Console.WriteLine("\nСредний Курс кассы - {0}", curAvg);
            Console.WriteLine("Курс выплат - {0}", curAsk);
            Console.WriteLine("Курс взноса - {0}", curBid);
            Console.ReadKey();
        }
    }
}