using AutoMapper;
using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;
        private readonly IMapper _mapper;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext, IMapper mapper)
        {
            _orderDbContext = orderDbContext;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var address = new Domain.OrderAggregate.Address(
                                                            context.Message.Address.Province,
                                                            context.Message.Address.District,
                                                            context.Message.Address.Street,
                                                            context.Message.Address.ZipCode,
                                                            context.Message.Address.Line
                                                            );

            var order = new Domain.OrderAggregate.Order(address, context.Message.BuyerId);

            Parallel.ForEach(context.Message.OrderItems, orderItem =>
            {
                order.AddOrderItem(
                                    orderItem.ProductId,
                                    orderItem.ProductName,
                                    orderItem.PictureUrl,
                                    orderItem.Price
                                    );
            });

            await _orderDbContext.Orders.AddAsync(order);
            await _orderDbContext.SaveChangesAsync();
        }
    }
}
