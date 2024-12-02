using System;

namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u
{
    public abstract class IdEntity<TId> : PersistEntity, IIdEntity<TId>, IEquatable<IdEntity<TId>>
    {
        public virtual TId Id { get; set; } = default!;

        public IdEntity()
            : this(PersistChange.None)
        {
        }

        protected IdEntity(PersistChange persistChange)
            : base(persistChange)
        {
        }

        protected IdEntity(IdEntity<TId> entity)
            : base(entity)
        {
            Id = entity.Id;
        }

        public override int GetHashCode()
        {
            if (!Equals(Id, default(TId)))
            {
                return Id!.GetHashCode();
            }

            return 0;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not IdEntity<TId>)
            {
                return false;
            }

            return Equals((IdEntity<TId>)obj);
        }

        public bool Equals(IdEntity<TId>? other)
        {
            if (this != other)
            {
                if (other != null)
                {
                    return Equals(Id, other.Id);
                }

                return false;
            }

            return true;
        }
    }
}
