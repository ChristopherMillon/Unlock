using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnlockDemConsole.Slack
{
    public class SlackChannelHistory
    {
        public bool ok { get; set; }

        public string latest { get; set; }

        public SlackMessage[] messages { get; set; }

        public bool has_more { get; set; }
    }
}
