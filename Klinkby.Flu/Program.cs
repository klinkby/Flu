/* Console app that get weekly feed from SSI and tweet changes in national flu level.
 * Copyright(c) 2016 Mads Breusch Klinkby.
 * GPLv3 license: https://www.gnu.org/licenses/gpl-3.0.html
 */

using Klinkby.Flu.Properties;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Klinkby.Flu
{
    internal static class Program
    {
        private readonly static Settings Settings = Settings.Default;

        private static int Main()
        {
            try
            {
                var cultureInfo = CultureInfo.CreateSpecificCulture(Settings.Culture);
                var newData = SsiFeed.GetWeeklyFluData(cultureInfo);
                var twtr = new Twtr().Authenticate();
                var recentLevel = twtr.GetMyRecentTweets()
                                      .Select(GetRecentLevel)
                                      .First(x => null != x)
                                      .Trim();
                if (recentLevel != newData.Level)
                {
                    string newStatus = string.Format(
                        cultureInfo,
                        Settings.TweetFormat,
                        recentLevel,
                        newData.Level,
                        newData.Link.AbsoluteUri);
                    twtr.PostTweet(newStatus);
                    return 1;
                }
                return 0;
            }
            catch (ApplicationException e)
            {
                Console.Error.WriteLine(e.Message);
                return -1;
            }
        }       

        private static string GetRecentLevel(string recentTweet)
        {
            var regex = new Regex(
                Settings.LevelPattern, 
                RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Singleline);
            string level = regex.Match(recentTweet)?.Groups[1].Value;
            return level;
        }
    }
}
