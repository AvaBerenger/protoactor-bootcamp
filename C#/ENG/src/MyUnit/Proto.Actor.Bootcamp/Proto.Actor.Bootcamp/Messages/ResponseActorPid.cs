using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto.Actor.Bootcamp.Messages
{
    
    public class ResponseActorPid
    {
        public PID Pid { get; }

        public ResponseActorPid(PID pid)
        {
            Pid = pid;
        }
    }
}
