using System.Threading;
using System.Threading.Tasks;
using VocabularyAPI.EntriesByMeaning;
using Microsoft.AspNetCore.Mvc;
using Model.Exceptions;

namespace VocabularyAPI.Controllers
{
    [ApiController]
    [Route("entriesByMeaning")]
    public sealed class EntriesByMeaningController : ControllerBase
    {
        private readonly IEntriesByMeaningService entriesByMeaningService;

        public EntriesByMeaningController(IEntriesByMeaningService entryByMeaningService)
        {
            this.entriesByMeaningService = entryByMeaningService;
        }

        [HttpGet("{meaningId}")]
        public async Task<ActionResult> GetEntryByMeaningAsync(string meaningId, CancellationToken token)
        {
            try
            {
                var entry = await this.entriesByMeaningService.GetEntryByMeaningAsync(meaningId, token).ConfigureAwait(false);
                return this.Ok(entry);
            }
            catch (MeaningNotFoundException ex)
            {
                return this.NotFound(new ExceptionError(ex.Message));
            }
        }
    }
}
