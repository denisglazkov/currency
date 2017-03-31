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
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);

            request.UseDefaultCredentials = true;
            WebProxy proxy = new WebProxy("192.168.1.4", 3128);
            Console.WriteLine("Enter your login:");
            string login = Console.ReadLine();
            Console.WriteLine("Enter your pass:");
            string pass = ReadPassword();
            proxy.Credentials = new NetworkCredential(login, pass);
            request.Proxy = proxy;

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
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

            if (_listBank.Count < 11)
            {
                Console.WriteLine("\n Ярик, еще не все банки дали курс!!!!!!!");
            }

            else
            {
                curAvg = curSum / (_listBank.Count * 2);
                curAsk = curAvg * (1 - 0.0325);
                curBid = curAvg * (1 + 0.0325);

                Console.WriteLine("\nСредний Курс кассы - {0}", curAvg);
                Console.WriteLine("Курс выплат - {0}", curAsk);
                Console.WriteLine("Курс взноса - {0}", curBid);
            }

//            curAvg = Math.Round(curSum / (_listBank.Count * 2), 2);
//            curAsk = Math.Round(curAvg * (1 - 0.0325), 2);
//            curBid = Math.Round(curAvg * (1 + 0.0325), 2);


            Console.ReadKey();
        }

        public static string ReadPassword()
            {
                string password = "";
                ConsoleKeyInfo info = Console.ReadKey(true);
                while (info.Key != ConsoleKey.Enter)


{
                    if (info.Key != ConsoleKey.Backspace)
{
                        Console.Write("*");
                        password += info.KeyChar;
}
                    else if (info.Key == ConsoleKey.Backspace)
{
                        if (!string.IsNullOrEmpty(password))
{
                            // remove one character from the list of password characters
                            password = password.Substring(0, password.Length - 1);
                            // get the location of the cursor
                            int pos = Console.CursorLeft;
                            // move the cursor to the left by one character
                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                            // replace it with space
                            Console.Write(" ");
                            // move the cursor to the left by one character again
                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
}
}
                    info = Console.ReadKey(true);
}
                // add a new line because user pressed enter at the end of their password
                Console.WriteLine();
                return password;
            }
    }
}