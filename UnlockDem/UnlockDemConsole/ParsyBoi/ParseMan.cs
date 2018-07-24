using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnlockDemConsole.ParsyBoi
{
    public class ParseMan
    {
        public bool IsUnlockRequest(string message)
        {
            return Regex.IsMatch(message, @"\@christopher\.millon unlock ([A-Za-z-0-9]*\.[A-Za-z-0-9]*)");
        }

        public string AccountToUnlock(string message)
        {
            if (!IsUnlockRequest(message))
            {
                return null;
            }

            return Regex.Match(message, @"\@christopher\.millon unlock ([A-Za-z-0-9]*\.[A-Za-z-0-9]*)").Groups[1].Value;
        }
    }
}
