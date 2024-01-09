using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto.Actor.Bootcamp.Actors
{
    public class MyActor : IActor
    {
        public Task ReceiveAsync(IContext context)
        {
            switch(context.Message)
            {
                case RoutedMessage msg:
                    ColorConsole.WriteLineGreen($"{msg.Text} display by {context.Self}"); 
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
