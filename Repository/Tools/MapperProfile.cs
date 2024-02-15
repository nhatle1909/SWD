using AutoMapper;
using Repository.Model;
using Repository.Models;
using Repository.ModelView;
using static Repository.ModelView.AccountView;
namespace Repository.Tools
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TemplateModel, TemplateModelView>().ReverseMap();
            CreateMap<Account, RegisterAccountView>().ReverseMap();
            CreateMap<AccountStatus, RegisterAccountView>().ReverseMap();

            CreateMap<Account, RegisterForStaffAccountView>().ReverseMap();
            CreateMap<AccountStatus, RegisterForStaffAccountView>().ReverseMap();
            CreateMap<Material, MaterialView>().ReverseMap();
            CreateMap<Interior, InteriorView>().ReverseMap();
        }
    }
}
