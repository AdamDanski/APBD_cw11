using Microsoft.AspNetCore.Mvc;
using zad11.Services;

namespace zad11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IDbService _dbService;

    public PatientsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientDetails(int id)
    {
        try
        {
            var result = await _dbService.GetPatientDetailsAsync(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}