using Xunit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Northwind.WebApi.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Northwind.Contracts;
using Northwind.UserRegistrationService;
using FluentAssertions;

namespace Northwind.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task Should_Send_RegisterUser_OnRegisterWithEndpointConvention()
        {
            // Setup
            var services = new ServiceCollection()
                .AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddConsumer<RegisterUserConsumer>();
                })
                .AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBus>())
                .AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            var provider = services.BuildServiceProvider(true);
            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            var bus = harness.Bus;

            // Endpoint convention setup
            EndpointConvention.Map<RegisterUser>(new Uri("queue:register-user-queue"));

            var controller = new UserController(bus, bus);

            // Invoke
            var result = await controller.RegisterWithEndpointConvention(new RegisterUserRequest
            {
                Username = "testuser",
                Password = "password",
                Email = "testuser@example.com"
            });

            // FluentAssertions
            result.Should().BeOfType<AcceptedResult>();

            var sentMessages = harness.Sent.Select<RegisterUser>().ToList();
            sentMessages.Should().NotBeNullOrEmpty();

            // Check count of sent messages of type RegisterUser
            sentMessages.Should().HaveCount(1);

            // Check content of the sent message
            var sentMessage = sentMessages.FirstOrDefault()?.Context.Message;
            sentMessage.Should().NotBeNull();
            sentMessage.Username.Should().Be("testuser");
            sentMessage.Password.Should().Be("password");
            sentMessage.Email.Should().Be("testuser@example.com");

            await harness.Stop();
        }
    }
}