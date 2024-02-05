using AutoMapper;
using Repository.Model;
using Repository.ModelView;
namespace Repository.Tools
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TemplateModel, TemplateModelView>().ReverseMap();
            CreateMap<Account, AccountView>().ReverseMap();
        }
    }
}
