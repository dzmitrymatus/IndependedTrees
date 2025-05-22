using AutoMapper;
using IndependedTrees.BLL.Models.Journal;
using IndependedTrees.BLL.Models.Trees;
using IndependedTrees.DAL.Models.Journal;
using IndependedTrees.DAL.Models.Tree;

namespace IndependedTrees.BLL.Configuration
{
    public class BLLMappingProfile : Profile
    {
        public BLLMappingProfile()
        {
            CreateMap<JournalRecord, JournalRecordModel>()
                .ReverseMap();
            CreateMap<TreeNode, TreeNodeModel>()
                .ReverseMap();
        }
    }
}
