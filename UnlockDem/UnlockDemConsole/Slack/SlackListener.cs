using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UnlockDemConsole.Slack;

namespace UnlockDemConsole.Slack
{
    public class SlackListener
    {
        public HttpClient HttpClient { get; }
        public HttpRequestMessage HttpRequestMessage { get; }

        public event EventHandler<IEnumerable<SlackMessage>> OnNewMessages;

        public SlackListener(HttpClient httpClient)
        {
            HttpClient = httpClient;

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("token", "TOKEN_HERE"));
            nvc.Add(new KeyValuePair<string, string>("channel", "CHANNEL_HERE"));

            HttpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://slack.com/api/channels.history")
            {
                Content = new FormUrlEncodedContent(nvc),
            };

            Listen();
        }

        protected void Listen()
        {
            Observable
                .Interval(TimeSpan.FromSeconds(10))
                .Select(x => HttpClient.SendAsync(HttpRequestMessage).Result.Content.ReadAsStringAsync().Result)
                .Select(x => JsonConvert.DeserializeObject<SlackChannelHistory>(x))
                .Subscribe(
                    x =>
                    {
                        OnNewMessages?.Invoke(this, x.messages);
                    });
        }
    }
}
