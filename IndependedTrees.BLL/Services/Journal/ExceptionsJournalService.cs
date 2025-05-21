using AutoMapper;
using IndependedTrees.BLL.Models.Journal;
using IndependedTrees.DAL.Models.Journal;
using IndependedTrees.DAL.Repository;
using System.Linq.Expressions;

namespace IndependedTrees.BLL.Services.Journal
{
    public class ExceptionsJournalService : IExceptionsJournalService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<JournalRecord> _repository;

        public ExceptionsJournalService(IMapper mapper,
            IRepository<JournalRecord> repository) 
        {
            this._mapper = mapper;
            this._repository = repository;
        }

        public async Task<IEnumerable<JournalRecordModel>> GetAllAsync(
            Expression<Func<IQueryable<JournalRecordModel>, IQueryable<JournalRecordModel>>> filterExpression)
        {
            return await this._repository
                .GetAllAsync(_mapper.Map<Expression<Func<IQueryable<JournalRecord>, IQueryable<JournalRecord>>>>(filterExpression))
                .ContinueWith(task => this._mapper.Map<IEnumerable<JournalRecordModel>>(task.Result));
        }

        public async Task<JournalRecordModel> GetAsync(Guid recordId)
        {
            return await this._repository
                .GetAsync(recordId)
                .ContinueWith(task => this._mapper.Map<JournalRecordModel>(task.Result));
        }

        public async Task CreateAsync(JournalRecordModel record)
        {
            var journalRecord = this._mapper.Map<JournalRecord>(record);
            await this._repository.InsertAsync(journalRecord);
        }
    }
}
