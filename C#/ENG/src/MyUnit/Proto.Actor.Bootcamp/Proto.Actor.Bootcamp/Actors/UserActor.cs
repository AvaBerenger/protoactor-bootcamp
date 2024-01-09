using Messages;

namespace Proto.Actor.Bootcamp.Actors
{
    public class UserActor : IActor
    {
        private Behavior _behavior;
        private string _currentlyWatching = string.Empty;
        private PID _playbackStatisticsActorRef;
        private int _id;
        public delegate Task Receive(IContext context);

        public UserActor(int id, PID playbackStatisticsActorRef)
        {
            Console.WriteLine("Creating a UserActor");
            ColorConsole.WriteLineCyan("Setting initial behavior to stopped");
            _behavior = new Behavior(Stopped);
            _id = id;
            _playbackStatisticsActorRef = playbackStatisticsActorRef;
        }

        public Task ReceiveAsync(IContext context) => _behavior.ReceiveAsync(context);

        private Task Playing(IContext context)
        {
            switch(context.Message)
            {
                case PlayMovieMessage msg:
                    ColorConsole.WriteLineRed("Error : cannot start playing another movie before stopping existing one");
                    ColorConsole.WriteLineCyan("UserActor is still Playing");
                    break;
                case StopMovieMessage msg:
                    ColorConsole.WriteLineYellow($"Uwer has stopped watching '{_currentlyWatching}'");
                    _behavior.Become(Stopped);
                    ColorConsole.WriteLineCyan("UserActor has now become Stopped");
                    break;
            }

            return Task.CompletedTask;
        }

        private Task Stopped(IContext context)
        {
            switch (context.Message)
            {
                case PlayMovieMessage msg:
                    _currentlyWatching = msg.MovieTitle;
                    ColorConsole.WriteLineYellow($"User is currently watching '{_currentlyWatching}'");
                    context.Send(_playbackStatisticsActorRef, msg);
                    _behavior.Become(Playing);
                    ColorConsole.WriteLineCyan("UserActor has now become Playing");
                    break;
                case StopMovieMessage msg:
                    ColorConsole.WriteLineRed("Error: cannot stop if nothing is playing");
                    break;
                default:
                    ColorConsole.WriteLineCyan("UserActor has now become Stopped");
                    break;
            }
            

            return Task.CompletedTask;
        }
    }
}
