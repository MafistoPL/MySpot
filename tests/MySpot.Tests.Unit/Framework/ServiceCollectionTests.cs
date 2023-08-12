using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Framework;

public class ServiceCollectionTests
{
    [Fact]
    public void test()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IMessenger, Messenger>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var messenger = serviceProvider.GetRequiredService<IMessenger>();
        var messenger2 = serviceProvider.GetRequiredService<IMessenger>();
 
        messenger.GetId().ShouldBe(messenger2.GetId());
    }
    
    private interface IMessenger
    {
        void Send();
        Guid GetId();
    }

    private class Messenger : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();

        public void Send()
            => Console.WriteLine($"Sending a message... [{_id}");

        public Guid GetId() => _id;
    }
}