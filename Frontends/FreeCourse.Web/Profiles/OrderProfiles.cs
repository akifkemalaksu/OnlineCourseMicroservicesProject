using AutoMapper;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Models.Payments;

namespace FreeCourse.Web.Profiles
{
    public class OrderProfiles : Profile
    {
        public OrderProfiles()
        {
            CreateMap<CheckoutInfoInput, PaymentInfoInput>();
            CreateMap<CheckoutInfoInput, AddressCreateInput>();
        }
    }
}
