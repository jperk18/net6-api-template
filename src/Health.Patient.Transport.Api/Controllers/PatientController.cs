using System.Net.Mime;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Mediator;
using Health.Patient.Domain.Queries.GetAllPatientsQuery;
using Health.Patient.Domain.Queries.GetPatientQuery;
using Health.Patient.Transport.Api.Middleware;
using Health.Patient.Transport.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Health.Patient.Transport.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;
    private readonly IMediator _mediator;

    public PatientController(ILogger<PatientController> logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator;
    }

    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatePatientApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiGenericValidationResultObject))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiGenericValidationResultObject))]
    public async Task<IActionResult> RegisterPatient([FromBody] CreatePatientApiRequest request)
    {
        var response = await _mediator.SendAsync(new CreatePatientCommand(request.FirstName, request.LastName,
            request.DateOfBirth));
        
        return new ObjectResult(new CreatePatientApiResponse() {PatientId = response})
            {StatusCode = StatusCodes.Status201Created};
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPatientApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiGenericValidationResultObject))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPatient([FromQuery] GetPatientApiRequest request)
    {
        var response = await _mediator.SendAsync(new GetPatientQuery(request.PatientId));
        return Ok(new GetPatientApiResponse(response.Id, response.FirstName, response.LastName, response.DateOfBirth));
    }
    
    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetPatientApiResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPatients()
    {
        var r = await _mediator.SendAsync(new GetAllPatientsQuery());
        return Ok(r.Select(response => new GetPatientApiResponse(response.Id, response.FirstName, response.LastName, response.DateOfBirth)).ToArray());
    }
}