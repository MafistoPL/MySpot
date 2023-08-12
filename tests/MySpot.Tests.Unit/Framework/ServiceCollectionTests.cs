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

        serviceCollection.AddTransient<IMessenger, Messenger>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        // there is no key Messenger in service collection
        var messenger = serviceProvider.GetService<Messenger>();  

        // that's why test would fail
        messenger.ShouldNotBeNull();
    }
    
    private interface IMessenger
    {
        void Send();
    }

    private class Messenger : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();

        public void Send()
            => Console.WriteLine($"Sending a message... [{_id}");
    }
}