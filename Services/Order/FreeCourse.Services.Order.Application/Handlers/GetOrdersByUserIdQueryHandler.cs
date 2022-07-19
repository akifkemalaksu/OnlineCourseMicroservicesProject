using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Services.Order.Application.Mapping;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDto>>>
    {
        private readonly OrderDbContext _dbContext;

        public GetOrdersByUserIdQueryHandler(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _dbContext.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId.Equals(request.UserId)).ToListAsync();

            if (!orders.Any())
                return Response<List<OrderDto>>.Success(new List<OrderDto>(), HttpStatusCode.OK);

            var orderDtos = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

            return Response<List<OrderDto>>.Success(orderDtos, HttpStatusCode.OK);
        }
    }
}