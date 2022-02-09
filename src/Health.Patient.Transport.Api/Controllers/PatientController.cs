using System.Net.Mime;
using Health.Patient.Api.Middleware;
using Health.Patient.Api.Requests;
using Health.Patient.Domain.Commands.Core;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core.Models;
using Health.Patient.Domain.Queries.Core;
using Health.Patient.Domain.Queries.GetAllPatientsQuery;
using Health.Patient.Domain.Queries.GetPatientQuery;
using Microsoft.AspNetCore.Mvc;

namespace Health.Patient.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;
    private readonly ICommandHandler<CreatePatientCommand, Guid> _createPatientHandler;
    private readonly IQueryHandler<GetPatientQuery, PatientRecord> _getPatientHandler;
    private readonly IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>> _getAllPatientsHandler;

    public PatientController(ILogger<PatientController> logger,
        ICommandHandler<CreatePatientCommand, Guid> createPatientHandler,
        IQueryHandler<GetPatientQuery, PatientRecord> getPatientHandler,
        IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientRecord>> getAllPatientsHandler
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _createPatientHandler = createPatientHandler ?? throw new ArgumentNullException(nameof(createPatientHandler));
        _getPatientHandler = getPatientHandler ?? throw new ArgumentNullException(nameof(getPatientHandler));
        _getAllPatientsHandler = getAllPatientsHandler ?? throw new ArgumentNullException(nameof(getAllPatientsHandler));
    }

    [HttpPost()]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatePatientApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiGenericException))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreatePatientApiRequest request)
    {
        var response =
            await _createPatientHandler.Handle(new CreatePatientCommand(request.FirstName, request.LastName,
                request.DateOfBirth));
        return new ObjectResult(new CreatePatientApiResponse() {PatientId = response})
            {StatusCode = StatusCodes.Status201Created};
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPatientApiResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiGenericException))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPatient([FromQuery] GetPatientApiRequest request)
    {
        var response = await _getPatientHandler.Handle(new GetPatientQuery() {PatientId = request.PatientId});
        return Ok(new GetPatientApiResponse(response.Id, response.FirstName, response.LastName, response.DateOfBirth));
    }
    
    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetPatientApiResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPatients()
    {
        var r = (await _getAllPatientsHandler.Handle(new GetAllPatientsQuery()))
            .Select(response => new GetPatientApiResponse(response.Id, response.FirstName, response.LastName, response.DateOfBirth));
        return Ok(r);
    }
}