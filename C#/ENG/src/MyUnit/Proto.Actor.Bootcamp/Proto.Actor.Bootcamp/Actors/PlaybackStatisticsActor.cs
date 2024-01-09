using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto.Actor.Bootcamp.Actors
{
    public class PlaybackStatisticsActor : IActor
    {
        private PID _moviePlayCounterActorRef;

        public Task ReceiveAsync(IContext context)
        {
            switch(context.Message)
            {
                case Started:
                    var props = Props.FromProducer(() => new MoviePlayCounterActor());
                    _moviePlayCounterActorRef = context.Spawn(props);
                    break;
                case PlayMovieMessage msg:
                    context.Send(_moviePlayCounterActorRef, msg);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
