using Xunit;
using Watson.Infrastructure;
using System.Threading.Tasks;
using System.Linq;
using CQRSlite.Events;
using System;
using NSubstitute;

namespace Watson.Tests
{
    public class EventStoreTests
    {
        private Infrastructure.EventStoreOrg _eventStore;

        public EventStoreTests()
        {
            var typeLocator = Substitute.For<ITypeLocator>();
            typeLocator.Find("MyEvent").Returns(typeof(MyEvent));

            _eventStore = new EventStoreOrg(new ConsoleLogger(), typeLocator);
            _eventStore.Connect("localhost").Wait();
        }

        [Fact]
        public async Task read_all_events()
        {
            // Act
            var events = await _eventStore.ReadAllEventsFromBeginning();
            
            // Assert
            Assert.NotEqual(0, events.Count());
        }

        [Fact]
        public async Task save_new_event()
        {
            // Arrange
            var newEvent = new MyEvent {
                Id = Guid.NewGuid(),
                Data = "hello world",
                Version = -1
            };

            // Act
            await _eventStore.Save(new [] { newEvent });
            
            // Assert
            var readEvent = (MyEvent)(await _eventStore.Get(newEvent.Id, -1)).Single();
            Assert.Equal(newEvent.Data, readEvent.Data);
        }
    }

    public class MyEvent : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public string Data { get; set; }
    }
}