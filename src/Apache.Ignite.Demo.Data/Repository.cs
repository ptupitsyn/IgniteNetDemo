using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Binary;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Core.Cache.Configuration;
using Apache.Ignite.Core.Cache.Query;
using Apache.Ignite.Core.DataStructures;
using Apache.Ignite.Demo.Data.Impl;
using Apache.Ignite.Linq;

namespace Apache.Ignite.Demo.Data
{
    /// <summary>
    /// Encapsulates data access.
    /// </summary>
    public class Repository
    {
        public static Repository Instance { get; } = new Repository();

        private readonly IIgnite _ignite = Ignition.Start(GetConfiguration());

        private readonly ICache<long, IPerson> _persons;  // Person id to person

        private readonly ICache<Relation, byte> _relations;  // Map from relation id to relation type


        private readonly IAtomicLong _idCounter;

        private Repository()
        {
            _persons = _ignite.GetCache<long, IPerson>("persons");
            _relations = _ignite.GetCache<Relation, byte>("relations");
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
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            return _persons.PutAsync(person.Id, person);
        }

        public Task AddFriendAsync(IPerson source, IPerson target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (source == target)
                throw new ArgumentException("Can't add self as friend", nameof(target));

            var relation = new Relation(source.Id, target.Id);

            return _relations.PutAsync(relation, 0);
        }

        public Task<IPerson> GetPersonAsync(long id)
        {
            return _persons.GetAsync(id);
        }

        public Task<ICollection<IPerson>> GetFriendsAsync(long id)
        {
            return _persons.GetAllAsync(GetFriendIds(id)).ContinueWith(t => t.Result.Values);
        }

        public IEnumerable<long> GetFriendIds(long id)
        {
            // Important: AsCacheQueryable enables LINQ-to-SQL
            return _relations.AsCacheQueryable()
                .Select(entry => entry.Key)
                .Where(relation => relation.SourceId == id)
                .Select(relation => relation.TargetId);
        }

        public IEnumerable<IPerson> SearchPersons(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            return _persons.Query(new TextQuery(typeof (Person), text)).Select(x => x.Value);
        }

        public IEnumerable<IPerson> GetPersons()
        {
            return _persons.Select(x => x.Value);
        }

        public Task PopulateDemoData()
        {
            return Task.WhenAll(
                File.ReadLines("SampleData.csv").Skip(1).Select(x => x.Split(',')).Select(data =>
                {
                    var person = CreatePerson(data[0]);

                    person.Country = data[1];
                    person.City = data[2];

                    DateTime dt;
                    if (DateTime.TryParseExact(data[3], "d", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        person.Birthday = dt;
                    }

                    return SavePersonAsync(person);
                }));
        }

        private static IgniteConfiguration GetConfiguration()
        {
            return new IgniteConfiguration
            {
                BinaryConfiguration = new BinaryConfiguration(typeof (Person), typeof (Relation)),
                CacheConfiguration = new[]
                {
                    new CacheConfiguration("persons", new QueryEntity(typeof (long), typeof (Person))),
                    new CacheConfiguration("relations", new QueryEntity(typeof (Relation), typeof (byte)))
                }
            };
        }
    }
}
