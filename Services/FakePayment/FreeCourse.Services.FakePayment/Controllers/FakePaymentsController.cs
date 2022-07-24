﻿using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomControllerBase
    {
        [HttpPost]
        public IActionResult ReceivePayment()
        {
            return CreateActionResultInstance(Response<NoContent>.Success(HttpStatusCode.OK));
        }
    }
}