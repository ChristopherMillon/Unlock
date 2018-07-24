using System;

namespace UnlockDemConsole.PowerShell
{
    public class PowerShellBoi
    {
        public void Unlock(string accountToUnlock)
        {
            using (System.Management.Automation.PowerShell PowerShellInstance = System.Management.Automation.PowerShell.Create())
            {
                PowerShellInstance.AddScript(
$@"
unlock thing {accountToUnlock}
");

                PowerShellInstance.Invoke();
            }
        }
    }
}
