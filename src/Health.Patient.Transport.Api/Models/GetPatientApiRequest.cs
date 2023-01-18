using System.Text.Json.Serialization;
using Health.Patient.Transport.Api.Models.Interfaces;

namespace Health.Patient.Transport.Api.Models;

public class GetPatientApiRequest : IPatientIdentifer
{
    public GetPatientApiRequest() { }
    
    [JsonConstructor]
    public GetPatientApiRequest(Guid patientId)
    {
        PatientId = patientId;
    }
    
    public Guid PatientId { get; set; }
}