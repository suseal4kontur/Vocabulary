using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using VocabularyAPI.Entries;
using Microsoft.AspNetCore.Mvc;
using Model.Exceptions;
using View.Entries;

namespace VocabularyAPI.Controllers
{
    [ApiController]
    [Route("entries")]
    public class EntriesController : ControllerBase
    {
        private readonly IEntriesService entriesService;

        public EntriesController(IEntriesService entriesService)
        {
            this.entriesService = entriesService;
        }

        [HttpGet("{lemma}")]
        public async Task<ActionResult> GetAsync(string lemma, CancellationToken token)
        {
            try
            {
                var entry = await this.entriesService.GetEntryAsync(lemma, token).ConfigureAwait(false);
                return this.Ok(entry);
            }
            catch (EntryNotFoundException ex)
            {
                return this.NotFound(new ExceptionError(ex.Message));
            }
        }

        [HttpGet("")]
        public async Task<ActionResult> SearchAsync([FromQuery] EntriesSearchInfo searchInfo, CancellationToken token)
        {
            var entriesList = await this.entriesService
                .SearchEntriesAsync(searchInfo, token).ConfigureAwait(false);
            return this.Ok(entriesList);
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateAsync([FromBody] EntryCreateInfo createInfo, CancellationToken token)
        {
            try
            {
                var entry = await this.entriesService
                    .CreateEntryAsync(createInfo, token).ConfigureAwait(false);
                return this.Ok(entry);
            }
            catch (ValidationException ex)
            {
                return this.BadRequest(new ExceptionError(ex.Message));
            }
            catch (EntryAlreadyExistsException ex)
            {
                return this.BadRequest(new ExceptionError(ex.Message));
            }
        }

        [HttpPatch("{lemma}")]
        public async Task<ActionResult> UpdateAsync(
            string lemma,
            [FromBody] EntryUpdateInfo updateInfo,
            CancellationToken token)
        {
            try
            {
                await this.entriesService
                    .UpdateEntryAsync(lemma, updateInfo, token).ConfigureAwait(false);
                return this.NoContent();
            }
            catch (ValidationException ex)
            {
                return this.BadRequest(new ExceptionError(ex.Message));
            }
            catch (EntryNotFoundException ex)
            {
                return this.NotFound(new ExceptionError(ex.Message));
            }
        }

        [HttpDelete("{lemma}")]
        public async Task<ActionResult> DeleteAsync(string lemma, CancellationToken token)
        {
            try
            {
                await this.entriesService
                    .DeleteEntryAsync(lemma, token).ConfigureAwait(false);
                return this.NoContent();
            }
            catch (EntryNotFoundException ex)
            {
                return this.NotFound(new ExceptionError(ex.Message));
            }
        }
    }
}
