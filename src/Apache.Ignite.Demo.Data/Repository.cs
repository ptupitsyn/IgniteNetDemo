using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Binary;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Core.DataStructures;
using Apache.Ignite.Demo.Data.Impl;

namespace Apache.Ignite.Demo.Data
{
    /// <summary>
    /// Encapsulates data access.
    /// </summary>
    public class Repository
    {
        public static Repository Instance { get; } = new Repository();

        private readonly IIgnite _ignite = Ignition.Start(GetConfiguration());

        private readonly ICache<long, IPerson> _persons;
        private readonly IAtomicLong _idCounter;

        private Repository()
        {
            _persons = _ignite.GetOrCreateCache<long, IPerson>("persons");
            _idCounter = _ignite.GetAtomicLong("IgniteDemoIdCounter", 0, true);
        }

        /// <summary>
        /// Creates a person with specified name, but does not save it.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>New person.</returns>
        public IPerson CreatePerson(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var id = _idCounter.Increment();

            return new Person(id, name);
        }

        public Task SavePersonAsync(IPerson person)
        {
            return _persons.PutAsync(person.Id, person);
        }

        public Task<IPerson> GetPersonAsync(long id)
        {
            return _persons.GetAsync(id);
        }

        public IEnumerable<IPerson> GetPersons()
        {
            return _persons.Select(x => x.Value);
        }

        private static IgniteConfiguration GetConfiguration()
        {
            return new IgniteConfiguration
            {
                BinaryConfiguration =
                    new BinaryConfiguration
                    {
                        TypeConfigurations =
                            new List<BinaryTypeConfiguration> {new BinaryTypeConfiguration(typeof (Person))}
                    }
            };
        }
    }
}
