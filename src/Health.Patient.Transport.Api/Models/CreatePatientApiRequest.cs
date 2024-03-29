﻿using System.Text.Json.Serialization;
using Health.Patient.Transport.Api.Models.Interfaces;

namespace Health.Patient.Transport.Api.Models;

public class CreatePatientApiRequest : IPatient
{
    [JsonConstructor]
    public CreatePatientApiRequest(string firstName, string lastName, DateTime dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}