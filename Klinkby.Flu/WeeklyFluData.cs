/* Console app that get weekly feed from SSI and tweet changes in national flu level.
 * Copyright(c) 2016 Mads Breusch Klinkby.
 * GPLv3 license: https://www.gnu.org/licenses/gpl-3.0.html
 */

using Klinkby.Flu.Properties;
using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;

namespace Klinkby.Flu
{
    internal class WeeklyFluData
    {
        private readonly static Settings Settings = Settings.Default;

        private WeeklyFluData()
        {
            // ILB
        }

        public static WeeklyFluData FromSyndicationItem(SyndicationItem item, CultureInfo cultureInfo)
        {
            var x = item.Summary.Text;
            var regex = new Regex(
                Settings.ContentPattern, 
                RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Singleline);
            var match = regex.Match(((TextSyndicationContent)item.Summary).Text);
            if (null == match) return null;
            var ret = new WeeklyFluData
            {
                Link = item.Links.First().Uri,
                Senitel = float.Parse(match.Groups["senitel"].Value, cultureInfo),
                //Emergency = float.Parse(match.Groups["emergency"].Value, cultureInfo),
                //Title = item.Title.Text,
                //Published = item.PublishDate,
            };
            ret.Level = GetLevel(ret.Senitel, cultureInfo);
            return ret;
        }

        public Uri Link { get; private set; }
        public float Senitel { get; private set; }
        public string Level { get; private set; }
        //public float Emergency { get; private set; }
        //public DateTimeOffset Published { get; private set; }
        //public string Title { get; private set; }

        private static string GetLevel(float percentage, CultureInfo cultureInfo)
        {
            var range = Settings.Range;
            for (var i = range.Count - 1; i >= 0; i--)
            {
                string[] severityItem = range[i].Split('\t');
                float value = float.Parse(severityItem[0], cultureInfo);
                if (percentage >= value)
                {
                    return severityItem[1];
                }
            }
            throw new ApplicationException("No valid level for " + percentage + " %");
        }
    }
}
