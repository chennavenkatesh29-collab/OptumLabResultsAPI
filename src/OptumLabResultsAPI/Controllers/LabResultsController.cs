using Microsoft.AspNetCore.Mvc;
using OptumLabResultsAPI.DTOs;
using OptumLabResultsAPI.Models;
using OptumLabResultsAPI.Services;

namespace OptumLabResultsAPI.Controllers;

[ApiController]
[Route("labresults")]
[Produces("application/json")]
public sealed class LabResultsController(ILabResultService service) : ControllerBase
{
    
    //POST
    [HttpPost]
    [ProducesResponseType(typeof(LabResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LabResult>> Create([FromBody] CreateLabResultRequest request, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    //GET
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(LabResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LabResult>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    //GET
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<LabResult>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<LabResult>>> Get([FromQuery] string? patientId, CancellationToken cancellationToken)
    {
        return Ok(await service.GetAsync(patientId, cancellationToken));
    }

  //Patch
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(LabResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LabResult>> Update(int id, [FromBody] UpdateLabResultRequest request, CancellationToken cancellationToken)
    {
        var updated = await service.UpdateAsync(id, request, cancellationToken);
        return updated is null ? NotFound() : Ok(updated);
    }

   //Delete
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
