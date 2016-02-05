/* Console app that get weekly feed from SSI and tweet changes in national flu level.
 * Copyright(c) 2016 Mads Breusch Klinkby.
 * GPLv3 license: https://www.gnu.org/licenses/gpl-3.0.html
 */

using Klinkby.Flu.Properties;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Linq;
using System.Xml;
using System.Globalization;

namespace Klinkby.Flu
{
    internal class SsiFeed
    {
        private readonly static Settings Settings = Settings.Default;

        public static WeeklyFluData GetWeeklyFluData(CultureInfo cultureInfo)
        {
            var feedItem = GetFeedItems();
            var data = feedItem.Select(x => WeeklyFluData.FromSyndicationItem(x, cultureInfo))
                               .First(x => null != x);
            return data;
        }

        private static IEnumerable<SyndicationItem> GetFeedItems()
        {
            try
            {
                SyndicationFeed feed;
                using (var rdr = XmlReader.Create(Settings.FeedUrl.AbsoluteUri))
                {
                    feed = SyndicationFeed.Load(rdr);
                }
                return feed.Items;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Feed service didn't behave", e);
            }
        }
    }
}
