namespace Health.Patient.Domain.Core.Services;

public interface IRetrievalService
{
    Task<string> Get();
}