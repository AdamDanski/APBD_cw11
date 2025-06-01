using Microsoft.AspNetCore.Mvc;
using zad11.DTOs;
using zad11.Services;

namespace zad11.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IDbService _dbService;

    public PrescriptionsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpPost]
    public async Task<ActionResult> AddPrescription(PrescriptionCreateDTO dto)
    {
        try
        {
            await _dbService.AddPrescriptionAsync(dto);
            return Ok("Recepta została dodana.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}