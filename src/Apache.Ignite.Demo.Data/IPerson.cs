using System;

namespace Apache.Ignite.Demo.Data
{
    public interface IPerson
    {
        long Id { get; }

        string Name { get; set; }

        string Country { get; set; }

        string City { get; set; }

        DateTime Birthday { get; set; }
    }
}