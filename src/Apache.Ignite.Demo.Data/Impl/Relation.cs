using System;
using Apache.Ignite.Core.Cache.Configuration;

namespace Apache.Ignite.Demo.Data.Impl
{
    internal class Relation
    {
        public Relation(long sourceId, long targetId)
        {
            if (sourceId == targetId)
                throw new ArgumentException();

            SourceId = sourceId;
            TargetId = targetId;
        }

        [QuerySqlField(IsIndexed = true)]
        public long SourceId { get; }

        [QuerySqlField(IsIndexed = true)]
        public long TargetId { get; }


        public override string ToString()
        {
            return $"Relation [SourceId: {SourceId}, TargetId: {TargetId}]";
        }
    }
}
