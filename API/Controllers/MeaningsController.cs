using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using VocabularyAPI.Meanings;
using Microsoft.AspNetCore.Mvc;
using Model.Exceptions;
using View.Meanings;

namespace VocabularyAPI.Controllers
{
    [ApiController]
    [Route("entries/{lemma}/meanings")]
    public sealed class MeaningsController : ControllerBase
    {
        private readonly MeaningsService meaningsService;

        public MeaningsController(MeaningsService meaningsService)
        {
            this.meaningsService = meaningsService;
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateAsync(
            string lemma,
            [FromBody] MeaningCreateInfo createInfo,
            CancellationToken token)
        {
            try
            {
                var meaning = await this.meaningsService
                    .CreateMeaningAsync(lemma, createInfo, token).ConfigureAwait(false);
                return this.Ok(meaning);
            }
            catch (EntryNotFoundException)
            {
                return this.NotFound();
            }
            catch (ValidationException ex)
            {
                return this.BadRequest(ex.ValidationResult);
            }
        }

        [HttpPatch("{meaningId}")]
        public async Task<ActionResult> UpdateAsync(
            string lemma,
            string meaningId,
            [FromBody] MeaningUpdateInfo updateInfo,
            CancellationToken token)
        {
            try
            {
                await this.meaningsService
                    .UpdateMeaningAsync(lemma, meaningId, updateInfo, token).ConfigureAwait(false);
                return this.Ok();
            }
            catch (EntryNotFoundException)
            {
                return this.NotFound();
            }
            catch (MeaningNotFoundException)
            {
                return this.NotFound();
            }
            catch (ValidationException ex)
            {
                return this.BadRequest(ex.ValidationResult);
            }
        }

        [HttpDelete("{meaningId}")]
        public async Task<ActionResult> DeleteAsync(
            string lemma,
            string meaningId,
            CancellationToken token)
        {
            try
            {
                await this.meaningsService
                    .DeleteMeaningAsync(lemma, meaningId, token).ConfigureAwait(false);
                return this.Ok();
            }
            catch (EntryNotFoundException)
            {
                return this.NotFound();
            }
            catch (MeaningNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}
