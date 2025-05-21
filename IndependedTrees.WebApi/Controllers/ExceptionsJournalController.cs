using AutoMapper;
using IndependedTrees.BLL.Models.Journal;
using IndependedTrees.BLL.Services.Journal;
using IndependedTrees.WebApi.Models.Filter;
using IndependedTrees.WebApi.Models.Journal;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace IndependedTrees.WebApi.Controllers
{
    [ApiController]
    public class ExceptionsJournalController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IExceptionsJournalService _exceptionsJournalService;

        public ExceptionsJournalController(IMapper mapper,
            IExceptionsJournalService exceptionsJournalService)
        {
            this._mapper = mapper;
            this._exceptionsJournalService = exceptionsJournalService;
        }

        [HttpPost("api.user.journal.getRange")]
        public async Task<IResult> GetRange(int skip, int count, [FromBody] JournalFilterModel filter)
        {
            Expression<Func<IQueryable<JournalRecordApiModel>, IQueryable<JournalRecordApiModel>>> filterExpression
                = items => items.Where(item => filter.From == null || item.TimeStamp > filter.From)
                              .Where(item => filter.To == null || item.TimeStamp < filter.To)
                              .Where(item => filter.Search == null || item.StackTrace.Contains(filter.Search))
                              .Skip(skip)
                              .Take(count);

            return await this._exceptionsJournalService
                .GetAllAsync(_mapper.Map<Expression<Func<IQueryable<JournalRecordModel>, IQueryable<JournalRecordModel>>>>(filterExpression))
                .ContinueWith(task => _mapper.Map<IEnumerable<JournalRecordApiModel>>(task.Result))
                .ContinueWith(task => Results.Json(new JournalRecordsApiModel() { 
                    Skip = skip, 
                    Count = task.Result.Count(), 
                    Items = task.Result }));
        }

        [HttpPost("api.user.journal.getSingle")]
        public async Task<IResult> Get(Guid id)
        {
            return await this._exceptionsJournalService
                .GetAsync(id)
                .ContinueWith(task => _mapper.Map<JournalRecordApiModel>(task.Result))
                .ContinueWith(task => Results.Json(task.Result));
        }
    }
}
