using AutoMapper;
using IndependedTrees.BLL.Exceptions;
using IndependedTrees.BLL.Models.Journal;
using IndependedTrees.BLL.Services.Journal;
using IndependedTrees.WebApi.Models.Exception;
using IndependedTrees.WebApi.Models.Journal;
using Microsoft.AspNetCore.Diagnostics;

namespace IndependedTrees.WebApi.ExceptionsHandler
{
    public class GlobalExceptionsHandler : IExceptionHandler
    {
        private readonly IMapper _mapper;

        public GlobalExceptionsHandler(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            var exceptionsJournalService = httpContext.RequestServices.GetService<IExceptionsJournalService>();

            var journalRecord = new JournalRecordApiModel()
            {
                EventId = Guid.NewGuid(),
                TimeStamp = DateTime.UtcNow,
                StackTrace = exception.StackTrace,
                QueryParameters = string.Join("$", httpContext.Request.Query.Select(x => $"{x.Key}={x.Value}"))
            };

            await exceptionsJournalService.CreateAsync(this._mapper.Map<JournalRecordModel>(journalRecord));

            var exceptionModel = new ExceptionApiModel
            {
                Id = journalRecord.EventId,
                Type = exception.GetType().ToString(),
                Data = new ExceptionDataApiModel()
            };

            switch(exception)
            {
                case SecureException secureException:
                    { 
                        exceptionModel.Data.Message = secureException.Message; 
                        break; 
                    };
                default: 
                    {
                        exceptionModel.Data.Message = $"Internal server error ID = {exceptionModel.Id}";
                        break;
                    }
            }

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response
                .WriteAsJsonAsync(exceptionModel, cancellationToken);

            return true;
        }
    }
}
