using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomControllerBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand()
            {
                BuyerId = paymentDto.Order.BuyerId,
                OrderItems = paymentDto.Order.OrderItems.Select(item => new OrderItem
                {
                    PictureUrl = item.PictureUrl,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                }).ToList(),
                Address = new Address
                {
                    District = paymentDto.Order.Address.District,
                    Line = paymentDto.Order.Address.Line,
                    Province = paymentDto.Order.Address.Province,
                    Street = paymentDto.Order.Address.Street,
                    ZipCode = paymentDto.Order.Address.ZipCode,
                }
            };

            await sendEndpoint.Send(createOrderMessageCommand);

            //todo gerçekci ödeme işlemi yap
            return CreateActionResultInstance(Shared.Dtos.Response<NoContent>.Success(HttpStatusCode.OK));
        }
    }
}