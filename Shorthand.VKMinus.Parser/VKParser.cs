using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Shorthand.VKMinus.Data.Models;

namespace Shorthand.VKMinus.Parser
{
    public static class VKParser
    {
        private static string[] _blacklist = { "http://www.vk.se/1012028/vk-fritt-for-alla-prenumeranter", "http://www.vk.se/1027884/sa-delar-du-vk" };
        public static StartpageData ParseStartpage()
        {
            var wc = new WebClient();
            var html = wc.DownloadString("http://www.vk.se/");
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var data = new StartpageData();

            var links = new List<string>();
            var latestNewsLinks = new List<string>();
            var mainNewsLinks = new List<string>();
            foreach (var link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                if (!IsValidNode(link))
                    continue;

                var attr = link.Attributes["href"].Value;

                if (string.IsNullOrEmpty(attr))
                    continue;

                if (attr.StartsWith("mailto:"))
                    continue;

                if (attr.Contains("#"))
                    attr = attr.Substring(0, attr.IndexOf("#", StringComparison.InvariantCultureIgnoreCase));

                if (!attr.StartsWith("http://www.vk.se/"))
                    continue;

                if (_blacklist.Contains(attr))
                    continue;

                if (IsLatestNewsNode(link))
                {
                    if (!latestNewsLinks.Contains(attr))
                    { 
                        latestNewsLinks.Add(attr);
                    }
                }
                else if (IsMainColumnNode(link))
                {
                    if (!mainNewsLinks.Contains(attr))
                        mainNewsLinks.Add(attr);
                }
                else
                    if (!links.Contains(attr))
                        links.Add(attr);
            }

            data.TotalLinks = latestNewsLinks.Count + links.Count + mainNewsLinks.Count;
            data.TotalLatestNewsLinks = latestNewsLinks.Count;
            data.TotalMainLinks = mainNewsLinks.Count;

            foreach (var link in links)
            {
                if (link.StartsWith("http://www.vk.se/plus/"))
                {
                    data.TotalPlusLinks++;
                }

                Console.WriteLine(link);
            }

            foreach (var link in latestNewsLinks)
            {
                if (link.StartsWith("http://www.vk.se/plus/"))
                {
                    data.TotalPlusLinks++;
                    data.TotalLatestNewsPlusLinks++;
                }
            }

            foreach (var link in mainNewsLinks)
            {
                if (link.StartsWith("http://www.vk.se/plus/"))
                {
                    data.TotalPlusLinks++;
                    data.TotalMainPlusLinks++;
                }
            }

            return data;
        }

        private static bool IsLatestNewsNode(HtmlNode node)
        {
            var n = node;
            while ((n = n.ParentNode) != null)
            {
                if (n.Attributes["class"] != null)
                {

                    if (n.Attributes["class"].Value.Contains("latest-news"))
                        return true;
                }
            }
            return false;
        }

        private static bool IsMainColumnNode(HtmlNode node)
        {
            var n = node;
            while ((n = n.ParentNode) != null)
            {
                if (n.Attributes["class"] != null)
                {

                    if (n.Attributes["class"].Value.Contains("block-articles"))
                        return true;
                }
            }
            return false;
        }
        
        public static Regex BlockNumbeRegex = new Regex("block-(?<nr>\\d)", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static Int32 GetBlockNumber(HtmlNode node)
        {
            var n = node;
            while ((n = n.ParentNode) != null)
            {
                if (n.Attributes["class"] != null)
                {
                    if (n.Attributes["class"].Value.Contains("block-")) {
                        var attr = n.Attributes["class"].Value;
                        var match = BlockNumbeRegex.Match(attr);
                        if (!match.Success)
                            continue;

                        return Convert.ToInt32(match.Groups["nr"].Value);
                    }
                }
            }
            return -1;
        }

        private static bool IsValidNode(HtmlNode node)
        {
            var n = node;
            while ((n = n.ParentNode) != null)
            {
                if (n.Attributes["class"] != null)
                {
                    if (n.Attributes["class"].Value.Contains("lokus-ads"))
                        return false;

                    if (n.Attributes["class"].Value.Contains("blog-list"))
                        return false;

                    if (n.Attributes["class"].Value.Contains("ad "))
                        return false;

                    if (n.Attributes["class"].Value.Contains(" ad"))
                        return false;

                    if (n.Attributes["class"].Value == "ad")
                        return false;

                    if (n.Attributes["class"].Value.Contains("opinion-teaser"))
                        return false;

                    if (n.Attributes["class"].Value.Contains("most-read"))
                        return false;

                    if (n.Attributes["class"].Value.Contains("most-commented"))
                        return false;

                    if (n.Attributes["class"].Value.Contains("footer-content"))
                        return false;
                }

                if (n.Attributes["id"] != null)
                {

                    if (n.Attributes["id"].Value.Contains("karriar-ads"))
                        return false;

                    if (n.Attributes["id"].Value.Contains("header"))
                        return false;
                }

                if (n.Name.ToLowerInvariant() == "h1")
                    return false;

                if (n.Name.ToLowerInvariant() == "h2")
                    return false;

                if (n.Name.ToLowerInvariant() == "h3")
                    return false;

                if (n.Name.ToLowerInvariant() == "h4")
                    return false;

                if (n.Name.ToLowerInvariant() == "h5")
                    return false;

                if (n.Name.ToLowerInvariant() == "h6")
                    return false;
            }

            return true;
        }
    }
}
