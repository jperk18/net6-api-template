﻿namespace Health.Patient.Storage.Sql.Core;

public interface IStorageConfiguration
{
    SqlDatabaseConfiguration PatientDatabase { get; set; }
}

public class StorageConfiguration : IStorageConfiguration
{
    public StorageConfiguration(SqlDatabaseConfiguration patientDatabaseConfiguration)
    {
        PatientDatabase = patientDatabaseConfiguration ?? throw new ArgumentNullException(nameof(patientDatabaseConfiguration));
    }
    
    public SqlDatabaseConfiguration PatientDatabase { get; set; }
}

public class SqlDatabaseConfiguration
{
    public SqlDatabaseConfiguration() { }
    public SqlType DbType { get; set; }
    public string? ConnectionString { get; set; }
}

public enum SqlType
{
    Sql = 0,
    Postgres = 1,
    InMemory = 99
}