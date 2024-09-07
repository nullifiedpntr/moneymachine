using HtmlAgilityPack;
using System;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Swift
{
    class Program
    {
        static int minimum;
        static int maximum;
        static bool lock_region;
        static bool estimate_price;
        static string regionLock;
        static string lockedPayment;
        static bool lock_payment;

        static string morejunk = "mechmarket: search results - flair:buyingjump to contentmy subredditsedit subscriptionspopular-all-random-users | AskReddit-pics-funny-movies-gaming-worldnews-news-todayilearned-nottheonion-explainlikeimfive-mildlyinteresting-DIY-videos-OldSchoolCool-europe-television-TwoXChromosomes-tifu-Music-books-LifeProTips-dataisbeautiful-aww-science-space-Showerthoughts-askscience-Jokes-IAmA-Futurology-sports-UpliftingNews-food-nosleep-creepy-history-gifs-InternetIsBeautiful-GetMotivated-gadgets-announcements-WritingPrompts-philosophy-Documentaries-Romania-EarthPorn-photoshopbattles-listentothis-blogmore »reddit.com mechmarket: search results - flair:buyingWant to join? Log in or sign up in seconds.|EnglishSubmit a swapGet an ad-free experience with special benefits, and directly support Reddit.get reddit premiummechmarketjoinleave294,026 readers271 users here nowSort by flair:\r\n\r\nSelling\r\nBuying\r\n Trading\r\n GB\r\nStore\r\nService\r\n\r\nInterest Check Artisan\r\nBulk\r\nSold\r\nPurchased\r\nMeta\r\n\r\ndivider\r\n\r\nSort by origin:\r\n\r\n[United States]\r\n[Canada]\r\n[Australia]\r\n[European Union]\r\n[United Kingdom]\r\n\r\ndivider\r\n\r\nREAD THE RULES\r\n\r\nPrice check thread\r\n\r\nAdd your Heatware\r\n\r\nConfirm your Trades\r\n\r\nCheck the Scammer Lists\r\n\r\nVendor Application\r\n\r\nProblem with a Trade?\r\n\r\ndivider\r\n\r\nIf you have been scammed please PM the moderators so we can take appropriate action and report the user to BAD KARMA so people will know. DO NOT make a [META] post before messaging the moderators.\r\n\r\nDo not message moderators individually with issues regarding the subreddit. We do not guarantee response if you message this way, if you really need something attended to, send us a modmail through the following link: Modmail or through the \"message the moderators button\" down below. They both do the same.\r\n\r\nThere is no /r/Mechmarket Discord, Slack or otherwise. Transactions made outside of www.reddit.com/r/mechmarket/ may not offer you the same protections and will be at your own risk.\r\n\r\nAny violation of the rules will result in your post being immediately removed. No exceptions. Repeated violations will result in a ban (temporary or permanent, at the discretion of the moderators.)\r\n\r\ndivider\r\n\r\nOther Trade Subreddits\r\n\r\n/r/hardwareswap\r\n\r\n/r/phoneswap\r\n\r\n/r/hardwareswapaustralia\r\n\r\n/r/HardwareSwapUK\r\n\r\n/r/HardwareSwapEU\r\n\r\n/r/AVexchange\r\n\r\n/r/CanadianHardwareSwap\r\n\r\n/r/SteamGameSwap\r\n\r\n/r/MouseMarket";
        static string junk = "a community for 11 yearsMODERATORSmessage the modssearchlimit my search to r/mechmarketuse the following search parameters to narrow your results:subreddit:subredditfind submissions in \"subreddit\"author:usernamefind submissions by \"username\"site:example.comfind submissions from \"example.com\"url:textsearch for \"text\" in urlselftext:textsearch for \"text\" in self post contentsself:yes (or self:no)include (or exclude) self postsnsfw:yes (or nsfw:no)include (or exclude) results marked as NSFWe.g. subreddit:aww site:imgur.com dogsee the search faq for details.advanced search: by author, subreddit...postssorted by: newrelevancehottopcommentslinks from: all timepast hourpast 24 hourspast weekpast monthpast yearBuying";
        static void Main(string[] args)
        {
            Console.WriteLine("money machine maker developed by moneymaker2024");
            Console.WriteLine("reading configuration..");
            ReadConfig();
            Thread.Sleep(1000);
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string raw = GetTextFromWebsite("https://old.reddit.com/r/mechmarket/search?q=flair%3Abuying&restrict_sr=on&sort=new&t=all").Replace(junk, string.Empty).Replace(morejunk, string.Empty);
            string[] offers = raw.Trim().Split("morelessBuying");
            foreach (string offer in offers)
            {
                string buyer = offer.Split("ago by")[1].Split("to r/mechmarket")[0].Trim();
                if (offer.Contains("[H]"))
                {
                    string offerParsed = offer.Split("point")[0];
                    
                    Console.WriteLine("---");
                    //Console.WriteLine("Found offer: " + offerParsed);
                    string region = offerParsed.Split("[H]")[0];
                    if (lock_region)
                    {
                        if (!region.Contains(regionLock))
                        {
                            continue;
                        }
                    }
                    string[] payment = offerParsed.Split("[W]");
                    string finalPayment = payment[0].Split("[H]")[1];
                    if (lock_payment)
                    {
                        if (!finalPayment.Contains(lockedPayment)) { continue; }
                    }
                    int price = 0;
                    if (estimate_price)
                    {
                        try
                        {
                            Console.WriteLine("loading product");
                            price = int.Parse(FindPrice(payment[1].Trim()));
                        } catch { Console.WriteLine("failed to estimate price"); }
                    }
                    
                    if (estimate_price)
                        if (price > maximum | price < minimum) { continue; }
                    
                    Console.WriteLine("Region: " + region.Trim());
                    Console.WriteLine("Buyer: " + buyer);
                    Console.WriteLine("Payment method(s): " + finalPayment.Trim());
                    if (payment[1].Contains(",") | payment[1].Contains("or") | payment[1].Contains("and"))
                    {
                        if (payment[1].Contains(","))
                        {
                            List<string> products = new List<string>();
                            string[] _temp;
                            _temp = payment[1].Trim().TrimEnd(digits).Split(",");
                            foreach (string product in _temp)
                            {
                                products.Add(product);
                            }
                            Console.WriteLine("Products: " + payment[1].Trim().TrimEnd(digits));
                        }
                        if (payment[1].Contains("or"))
                        {
                            List<string> products = new List<string>();
                            string[] _temp;
                            _temp = payment[1].Trim().TrimEnd(digits).Split("or");
                            foreach (string product in _temp)
                            {
                                products.Add(product);
                            }
                            Console.WriteLine("Products: " + payment[1].Trim().TrimEnd(digits));
                        }
                        if (payment[1].Contains("and"))
                        {
                            List<string> products = new List<string>();
                            string[] _temp;
                            _temp = payment[1].Trim().TrimEnd(digits).Split("and");
                            foreach (string product in _temp)
                            {
                                products.Add(product);
                            }
                            Console.WriteLine("Products: " + payment[1].Trim().TrimEnd(digits));
                        }

                    } else
                    {
                        Console.WriteLine("Product: " + payment[1].Trim().TrimEnd(digits));
                        if (estimate_price)
                            Console.WriteLine("Estimated Price: $" + price);
                    }
                    Console.WriteLine("---");
                }

            }
        }

        static void ReadConfig()
        {
            string config = Assembly.GetExecutingAssembly().Location.Replace("money machine.dll", string.Empty) + "config.txt";
            string[] raw = File.ReadAllLines(config);
            foreach (string line in raw)
            {
                if (line.Contains("minimum"))
                { 
                    minimum = int.Parse(line.Replace("minimum: ", string.Empty).Trim());
                }
                if (line.Contains("maximum"))
                {
                    maximum = int.Parse(line.Replace("maximum: ", string.Empty).Trim());
                }
                if (line.Contains("estimate_price"))
                {
                    estimate_price = bool.Parse(line.Replace("estimate_price: ", string.Empty).Trim());
                }
                if (line.Contains("region"))
                {
                    regionLock = line.Replace("region: ", string.Empty).Trim();
                }
                if (line.Contains("lock:"))
                {
                    lock_region = bool.Parse(line.Replace("lock: ", string.Empty).Trim());
                }
                if (line.Contains("payments:"))
                {
                    lockedPayment = line.Replace("payments: ", string.Empty).Trim();
                }
                if (line.Contains("lock_payment:"))
                {
                    lock_payment = bool.Parse(line.Replace("lock_payment: ", string.Empty).Trim());
                }
            }
        }

        static string FindPrice(string search)
        {
            string price = "";
            string raw = GetTextFromWebsite($"https://www.google.com/search?q={search + " price"}&oq={search + " price"}&gs_lcrp=EgZjaHJvbWUyBggAEEUYOdIBBzMxM2owajeoAgCwAgA&sourceid=chrome&ie=UTF-8");
            int dollarIndex = raw.IndexOf('$');

            if (dollarIndex != -1)
            {
                string afterDollar = raw.Substring(dollarIndex + 1);

                int endIndex = afterDollar.IndexOf(' ');

                price = (endIndex != -1) ? afterDollar.Substring(0, endIndex) : afterDollar;

            }

            return price;
        }

        static string GetTextFromWebsite(string url)
        {
            WebRequest client = WebRequest.Create(url);
            
                ((HttpWebRequest)client).UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:10.0.2) Gecko/20100101 Firefox/10.0.2";

                client.Method = "GET";
                WebResponse resp = client.GetResponse();
                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseText = reader.ReadToEnd();

                string htmlContent = responseText.Trim();

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(htmlContent);

                foreach (var script in document.DocumentNode.SelectNodes("//script|//style") ?? new HtmlNodeCollection(null))
                {
                    script.Remove();
                }

                string textContent = document.DocumentNode.InnerText;

                textContent = HtmlEntity.DeEntitize(textContent); 
                return textContent.Trim();
            }
        

    }
}