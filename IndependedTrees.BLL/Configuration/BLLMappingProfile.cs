using AutoMapper;
using IndependedTrees.BLL.Models.Journal;
using IndependedTrees.DAL.Models.Journal;

namespace IndependedTrees.BLL.Configuration
{
    public class BLLMappingProfile : Profile
    {
        public BLLMappingProfile()
        {
            CreateMap<JournalRecord, JournalRecordModel>()
                .ReverseMap();
        }
    }
}
