/* Console app that get weekly feed from SSI and tweet changes in national flu level.
 * Copyright(c) 2016 Mads Breusch Klinkby.
 * GPLv3 license: https://www.gnu.org/licenses/gpl-3.0.html
 */

using Klinkby.Flu.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using TweetSharp;

namespace Klinkby.Flu
{
    internal class Twtr
    {
        private readonly static Settings Settings = Settings.Default;
        private TwitterService _ws = new TwitterService(Settings.ApiKey, Settings.ApiSecret);

        public Twtr Authenticate()
        {
            _ws.AuthenticateWith(Settings.AccessToken, Settings.AccessTokenSecret);
            return this;
        }

        public IEnumerable<string> GetMyRecentTweets()
        {
            var recentTweets = _ws.ListTweetsOnList(new ListTweetsOnListOptions
            {
                Slug = Settings.ListSlug,
                OwnerScreenName = Settings.ListOwner,
                Count = 10
            });
            return recentTweets.Select(x => x.Text);
        }

        public void PostTweet(string status)
        {
            _ws.SendTweet(new SendTweetOptions
            {
                DisplayCoordinates = true,
                Status = status.Replace("%20", Uri.HexEscape('%') + 20), // Tweetshap bug workaround double escaping
                Lat = Settings.Lat,
                Long = Settings.Lon
            });
        }
    }
}
