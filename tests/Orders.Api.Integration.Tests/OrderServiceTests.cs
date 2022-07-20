// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentAssertions;
using Orders.Api.Integration.Tests.Fixture;
using Orders.Proto;
using Orders.Test.Common;
using StackExchange.Redis;

namespace Orders.Api.Integration.Tests;

public class OrderServiceTests : IClassFixture<WebApplicationFactory>
{
    private readonly OrderService.OrderServiceClient _grpcClient;
    private readonly IDatabase _redisDb;

    public OrderServiceTests(WebApplicationFactory webApplicationFactory)
    {
        _grpcClient = webApplicationFactory.CreateGrpcClient();
        _redisDb = webApplicationFactory.GetRedis().GetDatabase();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task When_OrderRequest_Is_Valid_Then_Orders_Validated_And_Persisted()
    {
        var request = OrdersRequestMother.Create();

        var response = await _grpcClient.ProcessOrdersAsync(request);

        await ShouldBeValidatedAndPersisted(response, request);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task When_OrderRequest_Is_Not_Valid_Then_ValidationErrors_Should_Be_Received_And_No_Orders_Persisted()
    {
        var request = OrdersRequestMother.Create(r => r.Orders.First().Currency = "ABC");

        var response = await _grpcClient.ProcessOrdersAsync(request);

        await ShouldHaveValidationErrorsAndOrdersNotPersisted(response, request);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task When_Request_Contains_OrderIds_That_Already_Exist_Then_ValidationErrors_Should_Be_Received_And_No_Orders_Persisted()
    {
        var request = OrdersRequestMother.Create();
        await _grpcClient.ProcessOrdersAsync(request);
        var anotherRequest = OrdersRequestMother.Create();
        anotherRequest.Orders.AddRange(request.Orders);

        var response = await _grpcClient.ProcessOrdersAsync(request);

        await ShouldHaveValidationErrorsAndOrdersNotPersisted(response, request);
    }

    private async Task ShouldBeValidatedAndPersisted(
        ProcessOrdersResponse response,
        OrdersRequest request)
    {
        response.Successful.Should().BeTrue();
        response.ValidationResults.Should().BeEmpty();
        await OrdersShouldBePersisted(GetOrderIds(request));
    }

    private async Task ShouldHaveValidationErrorsAndOrdersNotPersisted(
        ProcessOrdersResponse response,
        OrdersRequest request)
    {
        response.Successful.Should().BeFalse();
        response.ValidationResults.Should().NotBeEmpty();
        await OrdersShouldNotBePersisted(GetOrderIds(request));
    }

    private async Task OrdersShouldBePersisted(IEnumerable<string> orderIds)
    {
        foreach (var orderId in orderIds)
        {
            var exists = await _redisDb.HashExistsAsync(orderId, "order");
            exists.Should().BeTrue();
        }
    }

    private async Task OrdersShouldNotBePersisted(IEnumerable<string> orderIds)
    {
        foreach (var orderId in orderIds)
        {
            var exists = await _redisDb.HashExistsAsync(orderId, "order");
            exists.Should().BeFalse();
        }
    }

    private static IEnumerable<string> GetOrderIds(OrdersRequest? request)
    {
        var ids = request!.Orders.SelectMany(o =>
            o!.ChildOrders.Select(co => co.OrderId));
        return ids.Concat(request.Orders.Select(o => o.OrderId));
    }
}
