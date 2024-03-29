﻿using ATC.Abstractions;

namespace ATC.Core.Models
{
    public class Client : IClient
    {
        public string FirstName { get; }

        public string LastName { get; }

        public Client(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString()
        {
            return string.Format($"{FirstName} {LastName}");
        }
    }
}
