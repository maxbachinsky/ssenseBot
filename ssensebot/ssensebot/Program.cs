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
            string account_email = "random@gmail.com";
            string account_password = "fakePass";
            string first_name = "name1";
            string last_name = "name2";
            string address = "address";
            string postal_code = "postal code";

            IEnumerable<KeyValuePair<string, string>> loginInfo = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("email", account_email),
                new KeyValuePair<string, string>("password",account_password )
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
                var rand_response1 = await client.GetAsync("https://cc.hostedpci.com/WBSStatic/site60/proxy/js/jquery-1.11.2.min.js");
                var rand_response2 = await client.GetAsync("https://cc.hostedpci.com/WBSStatic/site60/proxy/js/jquery.ba-postmessage.2.0.0.min.js");
                var rand_response3 = await client.GetAsync("https://cc.hostedpci.com/WBSStatic/site60/proxy/js/jsencrypt.min.js");
                HttpContent content2 = response2.Content;
                string responseString2 = await content2.ReadAsStringAsync();
                System.IO.File.WriteAllText(@".\JS.txt", responseString2);
                StreamReader objstream = new StreamReader(@".\JS.txt");
                string[] lines = objstream.ReadToEnd().Split(new char[] { '\n' });
                var captchaIDTemp = lines[246].Trim();
                var captchaResponseTemp = lines[247].Trim();
                var sidTemp = lines[330].Trim();

                string sid = "";
                string captchaID = "";
                string captchaResponse = "";

                captchaID = captchaIDTemp.Substring(22, 29);
                captchaResponse = captchaResponseTemp.Substring(24, 30);
                sid = sidTemp.Substring(195, 201);
                /*for (int i = 22; i < 29; i++)
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
                */




                Console.WriteLine("pulled checkout page");
                Console.WriteLine(timer.Elapsed);
                HttpContent content = response.Content;
                string responseString = await content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseString);


                var CSRFTokenId = htmlDoc.DocumentNode.SelectSingleNode("//input[@name='CSRFTokenId']");
                string CSRFTokenIdString = CSRFTokenId.GetAttributeValue("value", "nothing");

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
                new KeyValuePair<string, string>("shipping_firstname", first_name),
                new KeyValuePair<string, string>("shipping_lastname", last_name),
                new KeyValuePair<string, string>("shipping_company", ""),
                new KeyValuePair<string, string>("shipping_address", address),
                new KeyValuePair<string, string>("shipping_country","CA"),
                new KeyValuePair<string, string>("shipping_state", "ON"),
                new KeyValuePair<string, string>("shipping_postalcode",postal_code),
                new KeyValuePair<string, string>("shipping_city","City"),
                new KeyValuePair<string, string>("shipping_phone","1231231234"),
                new KeyValuePair<string, string>("shipping_method", "1"),
                new KeyValuePair<string, string>("pccc",""),
                new KeyValuePair<string, string>("paymentMethod","creditcard"),
                new KeyValuePair<string, string>("creditcardHolderName","name name"),
                new KeyValuePair<string, string>("creditcardNumber", "1234123412341234"),
                new KeyValuePair<string, string>("creditcardCVV", "200"),
                new KeyValuePair<string, string>("creditCardMonth", "02"),
                new KeyValuePair<string, string>("creditCardYear", "2022"),
                new KeyValuePair<string, string>("billing_id", "49187769"),
                new KeyValuePair<string, string>("billing_isnew", "1"),
                new KeyValuePair<string, string>("billing_firstname",first_name),
                new KeyValuePair<string, string>("billing_lastname", last_name),
                new KeyValuePair<string, string>("billing_company",""),
                new KeyValuePair<string, string>("billing_address", address),
                new KeyValuePair<string, string>("billing_country", "CA"),
                new KeyValuePair<string, string>("billing_state", "ON"),
                new KeyValuePair<string, string>("billing_postalcode", postal_code),
                new KeyValuePair<string, string>("billing_city" ,"City"),
                new KeyValuePair<string, string>("billing_phone", "1231231234")
            };

                IEnumerable<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("CaptchaId", captchaID),
                new KeyValuePair<string, string>("CaptchaResp", captchaResponse),
                new KeyValuePair<string, string>("ccNum", "1234123412341234"),
                new KeyValuePair<string, string>("ccCVV", "820"),
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
                Console.WriteLine(captchaID);
                Console.WriteLine(captchaResponse);

                HttpContent form = new FormUrlEncodedContent(formData);
                HttpContent checkout = new FormUrlEncodedContent(checkoutInfo);
                Console.WriteLine("submitting payment");
                Console.WriteLine(timer.Elapsed);

                var finalHostedResponse = await client.PostAsync("https://cc.hostedpci.com/iSynSApp/appUserMapCC!createMapedCC.action", form);
                var finalCheckoutResponse = await client.PostAsync("http://www.ssense.com/en-ca/checkout", checkout);

                HttpContent hostedContent = finalHostedResponse.Content;
                string hostedString = await hostedContent.ReadAsStringAsync();
                System.IO.File.WriteAllText(@".\hosted.txt", hostedString);

                HttpContent checkoutContent = finalCheckoutResponse.Content;
                string checkoutString = await checkoutContent.ReadAsStringAsync();
                System.IO.File.WriteAllText(@".\checkout.html", checkoutString);


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
