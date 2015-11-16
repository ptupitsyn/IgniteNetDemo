using System;

namespace Apache.Ignite.Demo.Data.Impl
{
    internal struct Relation : IEquatable<Relation>
    {
        private readonly long _sourceId;
        private readonly long _targetId;

        public Relation(long sourceId, long targetId)
        {
            if (sourceId == targetId)
                throw new ArgumentException();

            _sourceId = sourceId;
            _targetId = targetId;
        }

        public long SourceId => _sourceId;

        public long TargetId => _targetId;

        public bool Equals(Relation other)
        {
            return _sourceId == other._sourceId && _targetId == other._targetId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Relation && Equals((Relation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_sourceId.GetHashCode()*397) ^ _targetId.GetHashCode();
            }
        }

        public static bool operator ==(Relation left, Relation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Relation left, Relation right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"Relation [SourceId: {_sourceId}, TargetId: {_targetId}]";
        }
    }
}
