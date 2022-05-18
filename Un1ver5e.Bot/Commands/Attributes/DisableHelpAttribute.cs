using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Un1ver5e.Bot.Commands.Attributes
{
    /// <summary>
    /// Specifies that this wont appear in help.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableHelpAttribute : Attribute
    {

    }
}
