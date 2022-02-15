using System.Collections.Generic;
using System.Linq;
using Health.Patient.Storage.Sql.Core.Databases.PatientDb;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Health.Patient.Transport.Api.IntegrationTests.Core;

class PatientApiApplication : WebApplicationFactory<Program>
{
    private readonly IEnumerable<IDataSeed>? _seeds;

    public PatientApiApplication(IEnumerable<IDataSeed>? seeds = null)
    {
        _seeds = seeds;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<PatientDbContext>));
            services.AddDbContext<PatientDbContext>(options =>
                InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(options, "TestingPatientDb", root)
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        });

        var application= base.CreateHost(builder);
        
        if (_seeds != null && _seeds.Any())
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using (var dbContext = provider.GetRequiredService<PatientDbContext>())
                {
                    dbContext.Database.EnsureCreated();

                    foreach (var seed in _seeds)
                    {
                        seed.Populate(dbContext);
                    }

                    dbContext.SaveChanges();
                }
            }

        return application;
    }
}

public interface IDataSeed
{
    void Populate(PatientDbContext dbContext);
}