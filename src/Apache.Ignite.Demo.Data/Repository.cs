using System.Collections.Generic;
using System.Linq;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Demo.Data.Impl;

namespace Apache.Ignite.Demo.Data
{
    /// <summary>
    /// Encapsulates data access.
    /// </summary>
    public class Repository
    {
        public static Repository Instance { get; } = new Repository();

        private readonly IIgnite _ignite = Ignition.Start();
        private readonly ICache<long, Person> _persons;

        public Repository()
        {
            _persons = _ignite.GetOrCreateCache<long, Person>("persons");
        }

        public IEnumerable<IPerson> GetPersons()
        {
            return _persons.Select(x => x.Value);
        }
    }
}
