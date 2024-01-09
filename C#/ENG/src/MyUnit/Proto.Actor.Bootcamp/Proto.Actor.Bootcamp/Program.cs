using Messages;
using Proto;
using Proto.Actor.Bootcamp;
using Proto.Actor.Bootcamp.Actors;
using Proto.Actor.Bootcamp.Messages;
using Proto.Router;

namespace MovieStreaming;

class Program
{
    /* UNIT 2 and 3
    static void Main(string[] args)
    {
        var system = new ActorSystem();

        Console.WriteLine("Actor system created");

        var props = Props.FromProducer(() => new PlaybackActor());
        var pid = system.Root.Spawn(props);

        //system.Root.Send(pid, new PlayMovieMessage("The Movie",44));
        system.Root.Send(pid, new PlayMovieMessage {MovieTitle = "The Movie Proto", UserId = 37 });
        system.Root.Send(pid, new PlayMovieMessage {MovieTitle = "The Movie 2", UserId = 42 });
        system.Root.Send(pid, new PlayMovieMessage {MovieTitle = "The Movie 3", UserId = 47 });
        system.Root.Send(pid, new PlayMovieMessage {MovieTitle = "The Movie 4", UserId = 13 });

        //Thread.Sleep(50);
        //Console.WriteLine("press any key to restat actor");
        //Console.ReadLine();

        //system.Root.Send(pid, new Recoverable());

        //Console.WriteLine("press any key to stop actor");
        //Console.ReadLine();
        //system.Root.Stop(pid);

        //system.Root.Send(pid, new PlayMovieMessage { MovieTitle = "The movie stopped", UserId = 13 });
        //system.Root.Send(pid, new PlayMovieMessage { MovieTitle = "The movie stopped 2", UserId = 13 });
        //system.Root.Send(pid, new PlayMovieMessage { MovieTitle = "The movie stopped 3", UserId = 13 });

        system.Root.Poison(pid);



        Console.ReadLine();
    }

    static void Main(string[] args)
    {
        var system = new ActorSystem();
        Console.WriteLine("Actor system created");

        var props = Props.FromProducer(() => new UserActor());
        var pid = system.Root.Spawn(props);

        Console.ReadKey();
        Console.WriteLine("Sending PlayMovieMessage (The Movie)");
        system.Root.Send(pid, new PlayMovieMessage { MovieTitle = "The Movie", UserId = 44 });
        Console.ReadKey();
        Console.WriteLine("Sending another PlayMovieMessage (The Movie 2)");
        system.Root.Send(pid, new PlayMovieMessage { MovieTitle = "The Movie 2", UserId = 44 });

        Console.ReadKey();
        Console.WriteLine("Sending a StopMovieMessage");
        system.Root.Send(pid, new StopMovieMessage());

        Console.ReadKey();
        Console.WriteLine("Sending another StopMovieMessage");
        system.Root.Send(pid, new StopMovieMessage());

        Console.ReadLine();
    }
    */

    /* UNIT 4
    /// <summary>
    /// Unit 4 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    static async Task Main(string[] args)
    {
        var system = new ActorSystem();
        Console.WriteLine("Actor system created");

        var props = Props.FromProducer(() => new PlaybackActor());
        var playbackActorPid = system.Root.Spawn(props);

        var actorPidMessage = await system.Root.RequestAsync<ResponseActorPid>(playbackActorPid, new RequestActorPid());
        var userCoordinatorPid = actorPidMessage.Pid;

        do
        {
            ShortPause();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            ColorConsole.WriteLineGray("enter a command and hit enter");

            var command = Console.ReadLine();

            if(command != null)
            {
                if(command.StartsWith("play"))
                {
                    var userId = int.Parse(command.Split(',')[1]);
                    var movieTitle = command.Split(",")[2];

                    system.Root.Send(userCoordinatorPid, new PlayMovieMessage { MovieTitle = movieTitle, UserId = userId });
                }
                else if (command.StartsWith("stop"))
                {
                    var userId = int.Parse(command.Split(',')[1]);

                    system.Root.Send(userCoordinatorPid, new StopMovieMessage {  UserId = userId});
                }
                else if (command == "exit")
                {
                    Terminate();
                }
            }
        } while (true);

        static void ShortPause()
        {
            Thread.Sleep(250);
        }

        static void Terminate()
        {
            Console.WriteLine("Actor system shutdown");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
    */

    /* UNIT 5 Lesson 3
    static async Task Main(string[] args)
    {
        Props MyActorProps = Props.FromProducer(() => new MyActor());

        var system = new ActorSystem();
        var context = new RootContext(system);
        var props = context.NewRoundRobinPool(MyActorProps, 5);
        var pid = context.Spawn(props);
        for (var i = 0; i< 10; i++)
        {
            context.Send(pid, new RoutedMessage { Text = $"{i}" });
        }

        Thread.Sleep(1000);
    }
    */

    static async Task Main(string[] args)
    {
        Props MyActorProps = Props.FromProducer(() => new MyActor());

        var system = new ActorSystem();
        var context = new RootContext(system);
        var props = context.NewBroadcastGroup(
            context.Spawn(MyActorProps),
            context.Spawn(MyActorProps),
            context.Spawn(MyActorProps),
            context.Spawn(MyActorProps)
        );
        var pid = context.Spawn(props);
        var anotherRoute = context.Spawn(MyActorProps);

        for (var i = 0; i < 10; i++)
        {
            
            context.Send(pid, new RoutedMessage { Text = $"{i}" });
        }

        Thread.Sleep(1000);
    }


    private class Decider
    {
        public static SupervisorDirective Decide(PID pid, Exception reason)
        {
            return SupervisorDirective.Restart;
        }
    }
}


