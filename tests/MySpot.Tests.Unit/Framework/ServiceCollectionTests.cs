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

        serviceCollection.AddScoped<IMessenger, Messenger>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        IMessenger messenger, messenger2, messenger3, messenger4;

        using (var scope = serviceProvider.CreateScope())
        {
            messenger = scope.ServiceProvider.GetRequiredService<IMessenger>();
            messenger2 = scope.ServiceProvider.GetRequiredService<IMessenger>();
        }

        using (var otherScope = serviceProvider.CreateScope())
        {
            messenger3 = otherScope.ServiceProvider.GetRequiredService<IMessenger>();
            messenger4 = otherScope.ServiceProvider.GetRequiredService<IMessenger>();
        }

        messenger.GetId().ShouldBe(messenger2.GetId());
        messenger3.GetId().ShouldBe(messenger4.GetId());
        messenger.GetId().ShouldNotBe(messenger4.GetId());
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