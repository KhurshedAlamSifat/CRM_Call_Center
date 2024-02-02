using Base.Services.Customers;
using Base.Services.Districts;
using Base.Services.Notification;
using Base.Services.ServiceCenters;
using Base.Services.Thanas;
using Microsoft.AspNetCore.Mvc;

namespace Base.Web.Areas.Secure.Controllers
{
    public class CustomerController : BaseAdminController
    {
        private readonly ICustomerService _customerService;
        private readonly IThanaService _thanaService;
        private readonly IDistrictService _districtService;
        private readonly INotificationService _notificationService;
        private readonly IServiceCenterService _serviceCenterService;

        public CustomerController(ICustomerService customerService, IThanaService thanaService, IDistrictService districtService, INotificationService notificationService, IServiceCenterService serviceCenterService)
        {
            _customerService = customerService;
            _thanaService = thanaService;
            _districtService = districtService;
            _notificationService = notificationService;
            _serviceCenterService = serviceCenterService;
        }
        public IActionResult Index(string phoneNumber)
        {
            //    if (!string.IsNullOrEmpty(phoneNumber))
            //    {
            //        // Check if a customer with the provided phone number exists in the database
            //        var customer = _customerService.GetCustomerAsync(phoneNumber).Result;

            //        if (customer != null)
            //        {
            //            // If a customer exists, pre-fill the form with customer data
            //            var model = new CustomerViewModel
            //            {
            //                // Map customer properties to your view model
            //                // Example:
            //                ClientName = customer.ClientName,
            //                District = customer.District,
            //                Thana = customer.Thana,
            //                // Map other properties as needed
            //            };

            //            return View(model);
            //        }
            //    }

            //    // If the phone number doesn't exist or is not provided, show an empty form
            //    return View(new CustomerViewModel());
            //}
            return null;
        }

    }
}
