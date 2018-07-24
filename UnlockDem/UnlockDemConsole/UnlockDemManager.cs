using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UnlockDemConsole.ParsyBoi;
using UnlockDemConsole.PowerShell;
using UnlockDemConsole.Slack;

namespace UnlockDemConsole
{
    public class UnlockDemManager : IDisposable
    {
        public SlackListener SlackListener { get; }
        public ParseMan ParseMan { get; }
        public PowerShellBoi ShellBoi { get; }

        public IDisposable EventLoop { get; }

        public UnlockDemManager(SlackListener slackListener, ParseMan parseMan, PowerShellBoi shellBoi)
        {
            SlackListener = slackListener;
            ParseMan = parseMan;
            ShellBoi = shellBoi;

            EventLoop = Observable.FromEventPattern<IEnumerable<SlackMessage>>(
                ev => SlackListener.OnNewMessages += ev,
                ev => SlackListener.OnNewMessages -= ev)
                .SelectMany(x => x.EventArgs)
                .Where(x => ParseMan.IsUnlockRequest(x.text))
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe((slackMessage) =>
                {
                    string accountToUnlock = ParseMan.AccountToUnlock(slackMessage.text);

                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry($"Going to unlock {accountToUnlock}...", EventLogEntryType.Information, 101, 1);
                    }

                    ShellBoi.Unlock(accountToUnlock);
                });
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    EventLoop.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
