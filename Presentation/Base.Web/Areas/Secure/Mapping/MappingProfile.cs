using AutoMapper;
using Base.Core.Domain.Brands;
using Base.Core.Domain.BusinessUnits;
using Base.Core.Domain.Categories;
using Base.Core.Domain.ComplainTypes;
using Base.Core.Domain.Districts;
using Base.Core.Domain.Problems;
using Base.Core.Domain.ServiceCenters;
using Base.Core.Domain.Technicians;
using Base.Core.Domain.Thanas;
using Base.Core.Domain.Users;
using Base.Web.Areas.Secure.Models.Brand;
using Base.Web.Areas.Secure.Models.BusinessUnit;
using Base.Web.Areas.Secure.Models.Category;
using Base.Web.Areas.Secure.Models.ComplainTypes;
using Base.Web.Areas.Secure.Models.Districts;
using Base.Web.Areas.Secure.Models.Problems;
using Base.Web.Areas.Secure.Models.ServiceCenter;
using Base.Web.Areas.Secure.Models.Technicians;
using Base.Web.Areas.Secure.Models.Thanas;
using Base.Web.Areas.Secure.Models.Users;

namespace Base.Web.Areas.Secure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<ComplainType, ComplainTypeModel>().ReverseMap();
            CreateMap<District, DistrictModel>().ReverseMap();  
            CreateMap<Thana,ThanaModel>().ReverseMap();
            CreateMap<Technician, TechnicianModel>().ReverseMap();
            CreateMap<BusinessUnit, BusinessUnitModel>().ReverseMap();
            CreateMap<Brand, BrandModel>().ReverseMap();
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Problem,ProblemTypeModel>().ReverseMap();
            CreateMap<ServiceCenter,ServiceCenterModel>().ReverseMap();

        }
    }
}
