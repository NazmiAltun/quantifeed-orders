// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Grpc.Core;
using Orders.Api.Helpers;
using Orders.Api.Repositories;
using Orders.Api.Validators;
using Orders.Proto;
using OrdersRequest = Orders.Proto.OrdersRequest;

namespace Orders.Api.Services;

public class OrdersService : OrderService.OrderServiceBase
{
    private readonly OrdersRequestValidator _validator;
    private readonly OrdersRepository _repository;

    public OrdersService(
        OrdersRequestValidator validator,
        OrdersRepository repository)
    {
        _validator = validator;
        _repository = repository;
    }

    public override async Task<ProcessOrdersResponse> ProcessOrders(OrdersRequest request, ServerCallContext context)
    {
        var model = Mapper.MapToOrderRequestModel(request);
        var validationResult = _validator.Validate(model);

        if (!validationResult.IsValid)
        {
            return Mapper.MapToProcessOrdersResponse(validationResult.Errors);
        }

        var saveResult = await _repository.SaveOrdersAsync(model.Orders!);

        return Mapper.MapToProcessOrdersResponse(saveResult);
    }
}
