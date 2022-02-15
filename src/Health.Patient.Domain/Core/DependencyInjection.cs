using FluentValidation.AspNetCore;
using Health.Patient.Domain.Commands.CreatePatientCommand;
using Health.Patient.Domain.Core.RegistrationHelpers;
using Health.Patient.Storage.Sql.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Health.Patient.Domain.Core;

public static class DependencyInjection
{
    public static void AddDomainServices(this IServiceCollection services, IDomainConfiguration config)
    {
        if (config == null || config.StorageConfiguration == null)
            throw new ApplicationException("Configuration is needed for domain services");

        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreatePatientCommandValidator>());
        services.AddHandlers();
        services.AddSingleton(config);
        
        //Add Dependant Database services
        services.AddStorageServices(config.StorageConfiguration);
    }
}