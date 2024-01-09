using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto.Actor.Bootcamp.Actors
{
    public class UserCoordinatorActor : IActor
    {
        private readonly PID _playbackStatisticsActorRef;
        private readonly Dictionary<int, PID> _users = new Dictionary<int, PID>();

        public UserCoordinatorActor(PID playbackStatisticsActorRef)
        {
            _playbackStatisticsActorRef = playbackStatisticsActorRef;
        }

        public Task ReceiveAsync(IContext context)
        {
            switch(context.Message)
            {
                case PlayMovieMessage msg:
                    ProcessPlayMovieMessage(context, msg);
                    break;
                case StopMovieMessage msg:
                    ProcessStopMovieMessage(context, msg);
                    break;
            }

            return Task.CompletedTask;
        }

        private void ProcessStopMovieMessage(IContext context, StopMovieMessage msg)
        {
            var childActorRef = GetOrCreateChildUserIfNotExists(context, msg.UserId);
            context.Send(childActorRef, msg);
        }

        private void ProcessPlayMovieMessage(IContext context, PlayMovieMessage msg)
        {
            var childActorRef = GetOrCreateChildUserIfNotExists(context, msg.UserId);
            context.Send(childActorRef, msg);
        }

        private PID GetOrCreateChildUserIfNotExists(IContext context, int userId)
        {
            if(!_users.ContainsKey(userId))
            {
                var props = Props.FromProducer(() => new UserActor(userId, _playbackStatisticsActorRef));
                var pid = context.SpawnNamed(props, $"User{userId}");
                _users.Add(userId, pid);
                ColorConsole.WriteLineCyan($"UserCoordinateActore created new child UserActor for {userId} (Total Users: {_users.Count})");
                return pid;
            }
            else
            {
                return _users[userId];
            }
        } 
    }
}
