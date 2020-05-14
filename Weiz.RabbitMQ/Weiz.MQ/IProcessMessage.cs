using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weiz.MQ
{
    public interface IProcessMessage
    {
        void ProcessMsg(MyMessage msg);
    }
}
