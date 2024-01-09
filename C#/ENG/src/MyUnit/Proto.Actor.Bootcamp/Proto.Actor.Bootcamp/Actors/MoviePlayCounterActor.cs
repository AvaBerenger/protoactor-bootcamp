using Messages;

namespace Proto.Actor.Bootcamp.Actors
{
    public class MoviePlayCounterActor : IActor
    {
        private Dictionary<string, int> _moviePlayCounts = new Dictionary<string, int>();

        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case PlayMovieMessage msg:
                    if (!_moviePlayCounts.ContainsKey(msg.MovieTitle))
                    {
                        _moviePlayCounts.Add(msg.MovieTitle, 0);
                    }
                    _moviePlayCounts[msg.MovieTitle]++;
                    ColorConsole.WriteMagenta($"MoviePlayerCounterActor '{msg.MovieTitle}' has been watched {_moviePlayCounts[msg.MovieTitle]} times");
                    break;
            }

            return Task.CompletedTask;
        }
    }
}