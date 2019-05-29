using HtmlAgilityPack;
using NLog;
using Pspl.Shared.Models;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pspl.Agent.Services
{
    public interface IFetcherService
    {
        Task<IEnumerable<Ad>> Fetch(string url);
    }

    public class FetcherService : IFetcherService
    {
        private static readonly Logger Logger = LogManager.GetLogger("PSPL");

        public async Task<IEnumerable<Ad>> Fetch(string url)
        {
            var result = new List<Ad>();

            try
            {
                var html = await GetHtml(url);

                if (!(html is null))
                {
                    var listing = html.CssSelect(".ul-unsafe");

                    if (!(listing is null))
                    {
                        var ads = listing.CssSelect(".li");

                        foreach (var ad in ads)
                        {
                            var name = ad.CssSelect("p").FirstOrDefault();
                            var address = ad.CssSelect("a").FirstOrDefault();
                            var desc = ad.CssSelect(".more").FirstOrDefault();

                            if (!(name is null) && !(address is null))
                            {
                                var newAd = new Ad
                                {
                                    Name = name.InnerText.Trim().ToLowerInvariant(),
                                    Url = address.InnerText.Trim().ToLowerInvariant()
                                };

                                if (!(desc is null))
                                {
                                    newAd.Description = desc.InnerHtml.Trim();
                                }

                                result.Add(newAd);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return result;
        }

        private static async Task<HtmlNode> GetHtml(string url)
        {
            HtmlNode result = null;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            var htmlstring = await reader.ReadToEndAsync();

                            var doc = new HtmlDocument();

                            doc.LoadHtml(htmlstring);

                            result = doc.DocumentNode;
                        }
                    }
                }
            }

            return result;
        }
    }
}