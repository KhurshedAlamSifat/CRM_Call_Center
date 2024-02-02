using FluentMigrator.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Base.Web.Models.Ticket
{
    public class TicketModel
    {
        public TicketModel()
        {
            ComplainTypes = new List<SelectListItem>();
            Technicians = new List<SelectListItem>();
            Districts = new List<SelectListItem>();
            Thanas = new List<SelectListItem>();
            BusinessUnits = new List<SelectListItem>();
            Brands = new List<SelectListItem>();
            Categories = new List<SelectListItem>();
            ServiceCenters = new List<SelectListItem>();
            Problems = new List<SelectListItem>();
        }

        [Required(ErrorMessage = "Complain is required")]
        public int ComplainType_Id { get; set; }
        public IList<SelectListItem> ComplainTypes { get; set; }
        public int Technician_Id { get; set; }
        public IList<SelectListItem> Technicians { get; set; }

        public int? District_Id { get; set; }
        public IList<SelectListItem> Districts { get; set; }

        public int? Thana_Id { get; set; }
        public IList<SelectListItem> Thanas { get; set; }

        public int Business_Id { get; set; }
        public IList<SelectListItem> BusinessUnits { get; set; }

        public int Brand_Id { get; set; }
        public IList<SelectListItem> Brands { get; set; }

        public int Category_Id { get; set; }
        public IList<SelectListItem> Categories { get; set; }

        [Required(ErrorMessage = "Phone no is required")]
        public string CustomerPhoneNo { get; set; }

        public int Problem_Id { get; set; }
        public IList<SelectListItem> Problems { get; set; }


        public string Note { get; set; }

        public int ServiceCenter_Id { get; set; }
        public IList<SelectListItem> ServiceCenters { get; set; }

        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }

    }
}
