using AutoMapper;
using Repository.Model;
using Repository.Models;
using Repository.ModelView;
namespace Repository.Tools
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TemplateModel, TemplateModelView>().ReverseMap();
            CreateMap<Account, AccountView>().ReverseMap();
            CreateMap<AccountStatus, AccountView>().ReverseMap();
            CreateMap<Material, MaterialView>().ReverseMap();
            CreateMap<Interior, InteriorView>().ReverseMap();
        }
    }
}
