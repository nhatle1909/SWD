using AutoMapper;
using Models.Model;
using Models.ModelView;
namespace EXE.Tools
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
