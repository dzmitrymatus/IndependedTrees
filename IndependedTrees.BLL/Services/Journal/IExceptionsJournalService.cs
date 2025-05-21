using IndependedTrees.BLL.Models.Journal;
using System.Linq.Expressions;

namespace IndependedTrees.BLL.Services.Journal
{
    public interface IExceptionsJournalService
    {
        Task<IEnumerable<JournalRecordModel>> GetAllAsync(
            Expression<Func<IQueryable<JournalRecordModel>, IQueryable<JournalRecordModel>>> filterExpression);
        Task<JournalRecordModel> GetAsync(Guid recordId);
        Task CreateAsync(JournalRecordModel record);
    }
}
