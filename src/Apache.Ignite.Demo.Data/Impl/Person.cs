﻿using System;
using Apache.Ignite.Core.Cache.Configuration;

namespace Apache.Ignite.Demo.Data.Impl
{
    internal class Person : IPerson
    {
        private string _name;

        public Person(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; }

        [QueryTextField]
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException();

                _name = value;
            }
        }

        [QueryTextField]
        public string Country { get; set; }

        [QueryTextField]
        public string City { get; set; }

        public DateTime Birthday { get; set; }

        public override string ToString()
        {
            return $"Person [Id: {Id}, Name: {Name}]";
        }
    }
}
