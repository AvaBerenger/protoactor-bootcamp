using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto.Actor.Bootcamp.Actors
{
    public class ChildActor : IActor
    {
        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case PlayMovieMessage msg:
                    ColorConsole.WriteLineGray($"Message forwarded to child {msg.MovieTitle}");
                    break;
                case Restarting msg:
                    ProcessRestartingMessage(msg);
                    break;

                case Recoverable msg:
                    ProcessRecoverableMessage(msg);
                    break;
            }
            return Task.CompletedTask;
        }

        private void ProcessRestartingMessage(Restarting msg)
        {
            ColorConsole.WriteLineGreen("ChildActor Restarting");
        }

        private void ProcessRecoverableMessage(Recoverable msg)
        {
            throw new Exception();
        }
    }
}
