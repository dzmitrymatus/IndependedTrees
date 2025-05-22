using AutoMapper;
using IndependedTrees.BLL.Models.Journal;
using IndependedTrees.BLL.Models.Trees;
using IndependedTrees.WebApi.Models.IndependedTrees;
using IndependedTrees.WebApi.Models.Journal;

namespace IndependedTrees.WebApi.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        { 
            CreateMap<JournalRecordModel, JournalRecordApiModel>()
                .ReverseMap();
            CreateMap<TreeNodeModel, TreeNodeApiModel>()
                .ReverseMap();
        }
    }
}
