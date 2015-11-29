using System;

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

        public long SourceId { get; }

        public long TargetId { get; }


        public override string ToString()
        {
            return $"Relation [SourceId: {SourceId}, TargetId: {TargetId}]";
        }
    }
}
