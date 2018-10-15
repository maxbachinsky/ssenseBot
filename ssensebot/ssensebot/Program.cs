using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using HtmlAgilityPack;
using System.Diagnostics;
using System.IO;

namespace ssensebot
{
    class Program
    {

        async static void PostRequest()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            IEnumerable<KeyValuePair<string, string>> loginInfo = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("email", "maxbachinsky99@gmail.com"),
                new KeyValuePair<string, string>("password","dodge123" )
            };

            HttpContent login = new FormUrlEncodedContent(loginInfo);

           

            using (HttpClient client = new HttpClient())
            {


                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
                HttpResponseMessage response = await client.PostAsync("https://www.ssense.com/en-ca/account/login", login);
                //HttpContent content = response.Content;
                Console.WriteLine("logged on");
                Console.WriteLine(timer.Elapsed);

                response = await client.PostAsync("https://www.ssense.com/en-ca/api/shopping-bag/182085M22001301", null);
                Console.WriteLine("added to cart");
                Console.WriteLine(timer.Elapsed);
                //HttpContent content = response.Content

                response = await client.GetAsync("https://www.ssense.com/en-ca/checkout");

            
                var response2 = await client.GetAsync("https://cc.hostedpci.com/iSynSApp/showPxyPage!ccFrame.action?pgmode1=prod&locationName=checkout1&sid=529081&pluginMode=jq1&fullParentHost=https://www.ssense.com");
                HttpContent content2 = response2.Content;
                string responseString2 = await content2.ReadAsStringAsync();
                System.IO.File.WriteAllText(@"C:\Users\maxba\source\repos\ssensebot\JS.txt", responseString2);
                StreamReader objstream = new StreamReader(@"C:\\Users\maxba\source\repos\ssensebot\JS.txt");
                string[] lines = objstream.ReadToEnd().Split(new char[] { '\n' });
                var captchaIDTemp = lines[246].Trim();
                var captchaResponseTemp = lines[247].Trim();
                var sidTemp = lines[330].Trim();

                string sid = "";
                string captchaID = "";
                string captchaResponse = "";

                for (int i= 22; i<29; i++)
                {
                    captchaID = captchaID + captchaIDTemp[i];
                }

                for (int i = 24; i < 30; i++)
                {
                    captchaResponse = captchaResponse + captchaResponseTemp[i];
                }

                for (int i = 195; i < 201; i++)
                {
                    sid = sid + sidTemp[i];
                }

           



                Console.WriteLine("pulled checkout page");
                Console.WriteLine(timer.Elapsed);
                HttpContent content = response.Content;
                        string responseString = await content.ReadAsStringAsync();
                        var htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(responseString);
                        

                        var CSRFTokenId = htmlDoc.DocumentNode.SelectSingleNode("//input[@name='CSRFTokenId']");
                        string CSRFTokenIdString = CSRFTokenId.GetAttributeValue("value","nothing");

                        var CSRFTokenValue = htmlDoc.DocumentNode.SelectSingleNode("//input[@name='CSRFTokenValue']");
                        string CSRFValueString = CSRFTokenValue.GetAttributeValue("value", "nothing");

                        var deviceFingerPrint = htmlDoc.DocumentNode.SelectSingleNode("//input[@name='device_fingerprint']");
                        string deviceFingerPrintString = deviceFingerPrint.GetAttributeValue("value", "nothing");

               
                //HttpContent content2 = response2.Content;
                //string responseString2 = await content2.ReadAsStringAsync();
                //var htmlDoc2 = new HtmlDocument();
                //htmlDoc2.LoadHtml(responseString2);

                //Console.WriteLine(responseString2);
                //Console.ReadLine();

                Console.WriteLine(CSRFTokenIdString);
                Console.WriteLine(CSRFValueString);
                Console.WriteLine(deviceFingerPrintString);

                Console.WriteLine("found the values");
                Console.WriteLine(timer.Elapsed);
                IEnumerable<KeyValuePair<string, string>> checkoutInfo = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("CSRFTokenId", CSRFTokenIdString),
                new KeyValuePair<string, string>("CSRFTokenValue", CSRFValueString),
                new KeyValuePair<string, string>("shipping_id", "49187769"),
                new KeyValuePair<string, string>("shipping_isnew", "1"),
                new KeyValuePair<string, string>("device_fingerprint", deviceFingerPrintString),
                new KeyValuePair<string, string>("shipping_firstname", "Maxim"),
                new KeyValuePair<string, string>("shipping_lastname", "Bachinsky"),
                new KeyValuePair<string, string>("shipping_company", ""),
                new KeyValuePair<string, string>("shipping_address", "39 Philips Lake Court"),
                new KeyValuePair<string, string>("shipping_country","CA"),
                new KeyValuePair<string, string>("shipping_state", "ON"),
                new KeyValuePair<string, string>("shipping_postalcode","L4E 0S8"),
                new KeyValuePair<string, string>("shipping_city","Richmond Hill"),
                new KeyValuePair<string, string>("shipping_phone","6472341105"),
                new KeyValuePair<string, string>("shipping_method", "1"),
                new KeyValuePair<string, string>("pccc",""),
                new KeyValuePair<string, string>("paymentMethod","creditcard"),
                new KeyValuePair<string, string>("creditcardHolderName","maxim bachinsky"),
                new KeyValuePair<string, string>("creditcardNumber", "4263700000008886"),
                new KeyValuePair<string, string>("creditcardCVV", "200"),
                new KeyValuePair<string, string>("creditCardMonth", "02"),
                new KeyValuePair<string, string>("creditCardYear", "2022"),
                new KeyValuePair<string, string>("billing_id", "49187769"),
                new KeyValuePair<string, string>("billing_isnew", "1"),
                new KeyValuePair<string, string>("billing_firstname","Maxim"),
                new KeyValuePair<string, string>("billing_lastname", "Bachinsky"),
                new KeyValuePair<string, string>("billing_company",""),
                new KeyValuePair<string, string>("billing_address", "39 Philips Lake Court"),
                new KeyValuePair<string, string>("billing_country", "CA"),
                new KeyValuePair<string, string>("billing_state", "ON"),
                new KeyValuePair<string, string>("billing_postalcode", "L4E 0S8"),
                new KeyValuePair<string, string>("billing_city" ,"Richmond Hill"),
                new KeyValuePair<string, string>("billing_phone", "6472341105")
            };

                IEnumerable<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("CaptchaId", captchaID),
                new KeyValuePair<string, string>("CaptchaResp", captchaResponse),
                new KeyValuePair<string, string>("ccNum", "4263700340598886"),
                new KeyValuePair<string, string>("ccCVV", "920"),
                new KeyValuePair<string, string>("sid", sid),
                new KeyValuePair<string, string>("ccNumTokenIdx", "1"),
                new KeyValuePair<string, string>("encryptEnabled", "N"),
                new KeyValuePair<string, string>("cvvValidate",""),
                new KeyValuePair<string, string>("enableTokenDisplay",""),
                new KeyValuePair<string, string>("ccNumTokenIdx",""),
                new KeyValuePair<string, string>("ccNumToken",""),
                new KeyValuePair<string, string>("ccCVVToken",""),
                new KeyValuePair<string, string>("firstName",""),
                new KeyValuePair<string, string>("lastName",""),
                new KeyValuePair<string, string>("expYear",""),
                new KeyValuePair<string, string>("expMonth",""),
                new KeyValuePair<string, string>("requestRef",""),
                new KeyValuePair<string, string>("encryptEnabled",""),
                new KeyValuePair<string, string>("encryptKeyName" ,"") 
                
            };

                HttpContent form = new FormUrlEncodedContent(formData);
                HttpContent checkout = new FormUrlEncodedContent(checkoutInfo);
                Console.WriteLine("submitting payment");
                Console.WriteLine(timer.Elapsed);

                await client.PostAsync("https://cc.hostedpci.com/iSynSApp/appUserMapCC!createMapedCC.action", form);
                await client.PostAsync("http://www.ssense.com/en-ca/checkout", checkout);
                      



                Console.WriteLine("checked out");
                Console.WriteLine(timer.Elapsed);

                timer.Stop();
                Console.WriteLine(timer.Elapsed);

            }
        }
        static void Main(string[] args)
        {



            //response = client.PostAsync("https://www.ssense.com/en-ca/account/login", content);
            //Console.WriteLine(response);
            //Console.WriteLine("account logged in");
            //Console.ReadLine();

            
            PostRequest();
            
 

            Console.ReadLine();
        }
    }
}
