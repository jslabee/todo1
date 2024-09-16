using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;

namespace ToDoApp.API;

[ApiController]
[Route("api/[controller]")]
public class OpraviloController(IOpraviloService opraviloService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var opravila = await opraviloService.GetAllOpravilaAsync();

        // Vrne HTTP 204 No Content, če ni nobenega opravila
        if (!opravila.Any()) return NoContent(); 

        return Ok(opravila);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var opravilo = await opraviloService.GetOpraviloByIdAsync(id);
        if (opravilo == null) return NotFound();
        return Ok(opravilo);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Opravilo opravilo)
    {
        // Preveri veljavnost modela
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var idOpravilo = await opraviloService.AddOpraviloAsync(opravilo);
        // Vrni HTTP 201 Created z lokacijo in novim opravilo objektom
        opravilo.Id = idOpravilo;
        return CreatedAtAction(nameof(GetById), new { id = idOpravilo }, opravilo);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Opravilo opravilo)
    {
        // Preverimo, če je bil ID opravila poslan
        if (opravilo.Id == 0) return BadRequest("Opravilo mora imeti veljaven ID.");

        // Posodobimo opravilo
        await opraviloService.UpdateOpraviloAsync(opravilo);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await opraviloService.DeleteOpraviloAsync(id);
        return NoContent();
    }
}