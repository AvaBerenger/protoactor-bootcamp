﻿using Messages;
using Proto.Persistence;
using Proto.Persistence.SnapshotStrategies;

namespace Proto.Actor.Bootcamp.Actors
{
    public class Calculator : IActor
    {
        private double _result;
        private Persistence.Persistence _persistence;

        public Calculator(IProvider provider)
        {

            //_persistence = Persistence.Persistence.WithEventSourcing(
            //   provider,
            //   "demo-app-id",
            //   ApplyEvent);

            _persistence = Persistence.Persistence.WithEventSourcingAndSnapshotting(
                provider,
                provider,
                "demo-app-id",
                ApplyEvent,
                ApplySnapshot,
                new IntervalStrategy(2),
                () => _result);
        }

        private void ApplySnapshot(Snapshot snapshot)
        {
            switch (snapshot)
            {
                case RecoverSnapshot msg:
                    if (msg.State is double ss)
                    {
                        _result = ss;
                        Console.WriteLine("MyPersistenceActor - RecoverSnapshot = Snapshot.Index = {0}, Snapshot.State = {1}", _persistence.Index, ss);
                    }
                    break;
            }
        }

        public async Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Started msg:
                    Console.WriteLine("MyPersistenceActor - Started");
                    await _persistence.RecoverStateAsync();
                    break;

                case AddCommand msg:
                    await _persistence.PersistEventAsync(new AddedEvent { Value = msg.Value });
                    _result += msg.Value;
                    break;

                case SubtractCommand msg:
                    await _persistence.PersistEventAsync(new SubtractedEvent { Value = msg.Value });
                    _result -= msg.Value;
                    break;

                case DivideCommand msg:
                    await _persistence.PersistEventAsync(new DividedEvent { Value = msg.Value });
                    _result /= msg.Value;
                    break;

                case MultiplyCommand msg:
                    await _persistence.PersistEventAsync(new MultipliedEvent { Value = msg.Value });
                    _result *= msg.Value;
                    break;

                case ClearCommand msg:
                    await _persistence.PersistEventAsync(new ResetEvent());
                    _result = 0;
                    break;

                case PrintResultCommand msg:
                    Console.WriteLine(_result);
                    break;
            }
        }

        private void ApplyEvent(Event @event)
        {
            switch (@event)
            {
                case RecoverEvent msg:
                    if (msg.Data is AddedEvent addedEvent)
                    {
                        _result += addedEvent.Value;
                    }
                    else if (msg.Data is SubtractedEvent subtractedEvent)
                    {
                        _result -= subtractedEvent.Value;
                    }
                    else if (msg.Data is DividedEvent dividedEvent)
                    {
                        _result /= dividedEvent.Value;
                    }
                    else if (msg.Data is MultipliedEvent multipliedEvent)
                    {
                        _result *= multipliedEvent.Value;
                    }
                    else if (msg.Data is ResetEvent resetEvent)
                    {
                        _result = 0;
                    }
                    break;
                case ReplayEvent msg:
                    break;
                case PersistedEvent msg:
                    break;
            }
        }
    }
}
