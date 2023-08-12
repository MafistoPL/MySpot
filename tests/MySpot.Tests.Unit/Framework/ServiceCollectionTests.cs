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
        serviceCollection.AddScoped<IMessenger, Messenger2>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        IMessenger messenger = serviceProvider.GetRequiredService<IMessenger>();

        messenger.ShouldBeOfType<Messenger2>();
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
    
    private class Messenger2 : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();

        public void Send()
            => Console.WriteLine($"Sending a message... [{_id}");

        public Guid GetId() => _id;
    }
}