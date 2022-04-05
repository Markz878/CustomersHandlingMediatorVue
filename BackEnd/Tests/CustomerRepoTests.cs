using AutoFixture.Xunit2;
using Core.Abstractions;
using Core.Commands;
using Core.Notifications;
using Core.Queries;
using FluentAssertions;
using MediatR;
using NSubstitute;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests;

public class CustomerRepoTests
{
    [Theory, AutoData]
    public async Task GetCustomers(GetCustomerListQuery getCustomersQuery, List<CustomerModel> customers)
    {
        ICustomerRepository customerRepo = Substitute.For<ICustomerRepository>();
        customerRepo.GetCustomers().Returns(customers);
        GetCustomerListQueryHandler getCustomerListQueryHandler = new(customerRepo);
        IEnumerable<CustomerModel> customerListResponse = await getCustomerListQueryHandler.Handle(getCustomersQuery, CancellationToken.None);
        await customerRepo.Received().GetCustomers();
        customerListResponse.Should().BeEquivalentTo(customers);
    }

    [Theory, AutoData]
    public async Task GetCustomerById(GetCustomerQuery getCustomerQuery, CustomerModel customer)
    {
        ICustomerRepository customerRepo = Substitute.For<ICustomerRepository>();
        customerRepo.GetCustomerById(getCustomerQuery.Id).Returns(customer);
        GetCustomerQueryHandler getCustomerQueryHandler = new(customerRepo);
        CustomerModel customerResponse = await getCustomerQueryHandler.Handle(getCustomerQuery, CancellationToken.None);
        await customerRepo.Received().GetCustomerById(Arg.Is<int>(x=>x==getCustomerQuery.Id));
        customerResponse.Should().BeEquivalentTo(customer);
    }

    [Theory, AutoData]
    public async Task CreateCustomer(AddCustomerCommand addCustomerCommand)
    {
        IMediator mediator = Substitute.For<IMediator>();
        mediator.Publish(Arg.Any<CustomerListChangedNotification>(), CancellationToken.None).Returns(Task.CompletedTask);
        ICustomerRepository customerRepo = Substitute.For<ICustomerRepository>();
        customerRepo.AddCustomer(addCustomerCommand.CustomerRequest).Returns(1);
        AddCustomerCommandHandler addCustomerCommandHandler = new(customerRepo, mediator);
        int customerId = await addCustomerCommandHandler.Handle(addCustomerCommand, CancellationToken.None);
        await mediator.Received().Publish(Arg.Any<CustomerListChangedNotification>(), CancellationToken.None);
        await customerRepo.Received().AddCustomer(addCustomerCommand.CustomerRequest);
        Assert.Equal(1, customerId);
    }

    [Theory, AutoData]
    public async Task DeleteCustomer(DeleteCustomerCommand deleteCustomerCommand)
    {
        IMediator mediator = Substitute.For<IMediator>();
        mediator.Publish(Arg.Any<CustomerListChangedNotification>(), CancellationToken.None).Returns(Task.CompletedTask);
        ICustomerRepository customerRepo = Substitute.For<ICustomerRepository>();
        customerRepo.DeleteCustomer(deleteCustomerCommand.CustomerId).Returns(true);
        DeleteCustomerCommandHandler deleteCustomerCommandHandler = new(customerRepo, mediator);
        bool success = await deleteCustomerCommandHandler.Handle(deleteCustomerCommand, CancellationToken.None);
        await mediator.Received().Publish(Arg.Any<CustomerListChangedNotification>(), CancellationToken.None);
        await customerRepo.Received().DeleteCustomer(deleteCustomerCommand.CustomerId);
        success.Should().BeTrue();
    }

    [Theory, AutoData]
    public async Task UpdateCustomer(UpdateCustomerCommand updateCustomerCommand)
    {
        IMediator mediator = Substitute.For<IMediator>();
        mediator.Publish(Arg.Any<CustomerListChangedNotification>(), CancellationToken.None).Returns(Task.CompletedTask);
        ICustomerRepository customerRepo = Substitute.For<ICustomerRepository>();
        customerRepo.UpdateCustomer(updateCustomerCommand.CustomerRequest).Returns(true);
        UpdateCustomerCommandHandler updateCustomerCommandHandler = new(customerRepo, mediator);
        bool success = await updateCustomerCommandHandler.Handle(updateCustomerCommand, CancellationToken.None);
        await mediator.Received().Publish(Arg.Is<CustomerListChangedNotification>(x=>x.CustomerId == updateCustomerCommand.CustomerRequest.Id), CancellationToken.None);
        await customerRepo.Received().UpdateCustomer(updateCustomerCommand.CustomerRequest);
        success.Should().BeTrue();
    }
}