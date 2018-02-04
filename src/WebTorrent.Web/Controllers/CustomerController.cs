using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebTorrent.Data;
using WebTorrent.Web.Helpers;
using WebTorrent.Web.ViewModels;

namespace WebTorrent.Web.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly IEmailer _emailer;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;


        public CustomerController(IUnitOfWork unitOfWork, ILogger<CustomerController> logger, IEmailer emailer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailer = emailer;
        }


        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var allCustomers = _unitOfWork.Customers.GetAllCustomersData();
            return Ok(Mapper.Map<IEnumerable<CustomerViewModel>>(allCustomers));
        }


        [HttpGet("throw")]
        public IEnumerable<CustomerViewModel> Throw()
        {
            throw new InvalidOperationException("This is a test exception: " + DateTime.Now);
        }


        [HttpGet("email")]
        public async Task<string> Email()
        {
            var recepientName = "QickApp Tester"; //         <===== Put the recepient's name here
            var recepientEmail = "test@ebenmonney.com"; //   <===== Put the recepient's email here

            var message = EmailTemplates.GetTestEmail(recepientName, DateTime.UtcNow);

            var response =
                await _emailer.SendEmailAsync(recepientName, recepientEmail, "Test Email from QuickApp", message);

            if (response.success)
            {
                return "Success";
            }

            return "Error: " + response.errorMsg;
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value: " + id;
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }


        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}