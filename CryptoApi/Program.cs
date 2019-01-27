using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Net.Mail;

namespace CryptoApi
{
    class Program
    {
        
        static public double[] GetTickerInfoBitrex()
        {

            WebRequest request = WebRequest.Create("https://api.bittrex.com/api/v1.1/public/getticker?market=USD-BTC");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            JObject asset = JObject.Parse(responseFromServer);
            var Bid = (double)asset["result"]["Bid"];
            var Ask = (double)asset["result"]["Ask"];
            double[] bidAsk =new double[] { Bid, Ask };
            return bidAsk;
        }

        static public double[] GetTickerInfoPoloniex()
        {
            WebRequest request = WebRequest.Create("https://poloniex.com/public?command=returnTicker");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            var results = JsonConvert.DeserializeObject<dynamic>(responseFromServer);
            string bid = results.USDT_BTC.highestBid;
            string ask = results.USDT_BTC.lowestAsk;
            // change . to , in bid and ask to be able to convert to  double
            bool t = false;
            bool y = false;
            int i = 0;
            int j = 0;
            while (!t || !y)
            {
                i++;
                j++;
                if (bid[i] == '.')
                {
                    t = true;
                }
                if (ask[j] == '.')
                {
                    y = true;
                }

            }
            StringBuilder sbb = new StringBuilder(bid);
            sbb[i] = ',';
            bid = sbb.ToString();
            StringBuilder sba = new StringBuilder(ask);
            sba[j] = ',';
            ask = sba.ToString();

            double[] bidAsk = new double[2] { Convert.ToDouble(bid), Convert.ToDouble(ask) };
            return bidAsk;
        }
        
        static public void compare_bidAsk(double[] bidAskA, double[]bidAskB)
        {
            if(bidAskA[0]>bidAskB[1])
            {
                
            }
            if (bidAskA[1] < bidAskB[0])
            {
                
            }
            Console.Clear();
            Console.WriteLine(bidAskA[0] + "  " + bidAskB[1] + "  \n" + bidAskB[0] + "  " + bidAskA[1] + "  ");
        }

        static void seand()
        {
            // Replace sender@example.com with your "From" address. 
            // This address must be verified with Amazon SES.
            String FROM = "";
            String FROMNAME = "Sender Name";

            // Replace recipient@example.com with a "To" address. If your account 
            // is still in the sandbox, this address must be verified.
            String TO = "";

            // Replace smtp_username with your Amazon SES SMTP user name.
            String SMTP_USERNAME = "";

            // Replace smtp_password with your Amazon SES SMTP user name.
            String SMTP_PASSWORD = "";

            // (Optional) the name of a configuration set to use for this message.
            // If you comment out this line, you also need to remove or comment out
            // the "X-SES-CONFIGURATION-SET" header below.
            //String CONFIGSET = "ConfigSet";

            // If you're using Amazon SES in a region other than USA Ouest (Oregon), 
            // replace email-smtp.us-west-2.amazonaws.com with the Amazon SES SMTP  
            // endpoint in the appropriate AWS Region.
            String HOST = "email-smtp.us-west-2.amazonaws.com";

            // The port you will connect to on the Amazon SES SMTP endpoint. We
            // are choosing port 587 because we will use STARTTLS to encrypt
            // the connection.
            int PORT = 587;

            // The subject line of the email
            String SUBJECT =
                "Amazon SES test (SMTP interface accessed using C#)";

            // The body of the email
            String BODY =
                "<h1>Amazon SES Test</h1>" +
                "<p>This email was sent through the " +
                "<a href='https://aws.amazon.com/ses'>Amazon SES</a> SMTP interface " +
                "using the .NET System.Net.Mail library.</p>";

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add(new MailAddress(TO));
            message.Subject = SUBJECT;
            message.Body = BODY;
            // Comment or delete the next line if you are not using a configuration set
            //message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                // Enable SSL encryption
                client.EnableSsl = true;

                // Try to send the message. Show status in console.
                try
                {
                    Console.WriteLine("Attempting to send email...");
                    client.Send(message);
                    Console.WriteLine("Email sent!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);
                }
            }
        }



        static public double XSMA(int X)
        {
            double SMA = 0;
            //for (int l = 0; l <= X; l++)
            //{
            //    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            //    Int32 unixTimestamp20 = unixTimestamp - 60 * 60 * 24 * l;
            //    string date = ;
                string address = "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=usd&days="+X;
                WebRequest request = WebRequest.Create(address);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                JObject asset = JObject.Parse(responseFromServer);
                var Bid = (double)asset["result"]["Bid"];

            //bool t = false;
            //int j = 0;
            //while (!t)// pour chaque echange on remplace le . par , puis on convertit en double et on ajoute au total
            //{
            //    j++;
            //    if (price[j] == '.')
            //    {
            //        StringBuilder sb = new StringBuilder(price);
            //        sb[j] = ',';
            //        price = sb.ToString();
            //        SMA += Convert.ToDouble(price);
            //        t = true;
            //    }
            //}
            //}
            SMA = SMA / X;

            return SMA;
            
            //int i = 0;
            //int j = 0;
            //double TradedTot = 0;
            //string total;
            //foreach(var item in dynJson)// parcours chaque echange des 20 derniers jours
            //{
            //    total = item.total;
            //    i++;
            //    while (!t)// pour chaque echange on remplace le . par , puis on convertit en double et on ajoute au total
            //    {
            //        j++;
            //        if (total[j] == '.')
            //        {
            //            StringBuilder sb = new StringBuilder(total);
            //            sb[j] = ',';
            //            total = sb.ToString();
            //            TradedTot += Convert.ToDouble(total);
            //            t = true;
            //        }
            //    }

            //}
            //TradedTot = TradedTot / 20; 
           // return TradedTot;
        }

            static void Main(string[] args)
        {
            TimerCallback tmCallback = CheckEffectExpiry;
            Timer timer = new Timer(tmCallback, "test", 1000, 1000);
            Console.WriteLine("Press any key to exit the sample");
            Console.ReadLine();
            //Console.WriteLine(XSMA(20));

            Console.ReadKey();

        }
        static void CheckEffectExpiry(object objectInfo)
        {
            compare_bidAsk(GetTickerInfoBitrex(),GetTickerInfoPoloniex());

        }
    }
}


