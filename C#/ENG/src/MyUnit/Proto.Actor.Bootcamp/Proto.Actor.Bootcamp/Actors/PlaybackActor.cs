using Messages;
using Proto.Actor.Bootcamp.Messages;

namespace Proto.Actor.Bootcamp.Actors
{
    public class PlaybackActor : IActor
    {
        private PID _userCoordinatorActorRef;
        private PID _playbackStatisticsActorRef;

        public PlaybackActor() => Console.WriteLine("Creating a PlaybackActor");

        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Started msg:
                    ProcessStartedMessage(context, msg);
                    break;
                case PlayMovieMessage msg:
                    ProcessPlayMovieMessage(context, msg);
                    break;               
                case Restarting msg:
                    ProcessRestartingMessage(msg);
                    break;
                case Recoverable msg:
                    ProcessRecoverableMessage(context, msg);
                    break;
                case Stopping msg:
                    ProcessStoppingMessage(msg);
                    break;
                case Stopped msg:
                    Console.WriteLine("Actor is stopped");
                    break;
                case RequestActorPid msg:
                    ProcessRequestActorPidMessage(context, msg);
                    break;

            }

            return Task.CompletedTask;
        }

        private void ProcessStartedMessage(IContext context, Started msg)
        {
            ColorConsole.WriteLineGreen("PlaybackActor Started");

            var props = Props.FromProducer(() => new PlaybackStatisticsActor());
            _playbackStatisticsActorRef = context.Spawn(props);

            props = Props.FromProducer(() => new UserCoordinatorActor(_playbackStatisticsActorRef));
            _userCoordinatorActorRef = context.Spawn(props);

            
        }

        private void ProcessRequestActorPidMessage(IContext context, RequestActorPid msg)
        {
            context.Respond(new ResponseActorPid(_userCoordinatorActorRef));
        }

        private void ProcessStoppingMessage(Stopping msg)
        {
            ColorConsole.WriteLineGreen("PlaybackActor Stopping");
        }

        private void ProcessPlayMovieMessage(IContext context, PlayMovieMessage msg)
        {
            ColorConsole.WriteLineYellow($"PlayMovieMessage {msg.MovieTitle} for user {msg.UserId} ");

            PID child;

            if (context.Children == null || context.Children.Count == 0)
            {
                var props = Props.FromProducer(() => new ChildActor());
                child = context.Spawn(props);
            }
            else
            {
                child = context.Children.First();
            }

            context.Forward(child);
        }

        private void ProcessRestartingMessage(Restarting msg)
        {
            ColorConsole.WriteLineGreen("ChildAcgor Restarting");
        }

        private void ProcessRecoverableMessage(IContext context, Recoverable msg)
        {
            ColorConsole.WriteLineRed("Recoverable message");

            PID child;

            if(context.Children == null || context.Children.Count == 0)
            {
                var props = Props.FromProducer(() => new ChildActor());
                child = context.Spawn(props);
            }
            else
            {
                child = context.Children.First();
            }

            context.Forward(child);
        }
    }
}
