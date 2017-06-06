using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace TwitterAppX
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program started.");

            try
            {
                var result = Task.Run(() => GetTweets());
                result.Wait();
                if (result == null)
                {
                    Console.WriteLine("Error while fetching tweets");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Program completed.");
            Console.Read();
        }

        static async Task<Status> SendTweet()
        {
            var auth = Credentials.auth;

            var context = new TwitterContext(auth);

            var status = await context.TweetAsync(
                "Who's a good boy?"
            );

            return status;
        }

        static async Task GetTweets()
        {
            var auth = Credentials.auth;

            var twitterCtx = new TwitterContext(auth);

            var searchResponse =
                await
                (from search in twitterCtx.Search
                 where search.Type == SearchType.Search &&
                       search.Query == "\"speedrun\""
                 select search)
                .SingleOrDefaultAsync();

            if (searchResponse != null && searchResponse.Statuses != null)
                searchResponse.Statuses.ForEach(tweet => {
                    Console.WriteLine("User: {0}, Tweet: {1}",
                        tweet.User.ScreenNameResponse,
                        tweet.Text, 
                        tweet.CreatedAt);
                    using (StreamWriter f = new StreamWriter(File.Open(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                        + "\\TweetList.txt", FileMode.Append)))
                    {
                        f.WriteLine("Created at: {0}, User: {1}, Tweet: {2}", tweet.CreatedAt, tweet.User.ScreenNameResponse, tweet.Text);
                    }
                });


        }

        public class Credentials
        {
            public static SingleUserAuthorizer auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = "your key",
                    ConsumerSecret = "your key",
                    AccessToken = "your key",
                    AccessTokenSecret = "your key"
                }
            };
        }

    }
}
