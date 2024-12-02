namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u
{
    public abstract class AuditEntity<TId, TAuditedBy, TAuditedOn> : IdEntity<TId>
    {
        public virtual TAuditedBy AuditedBy { get; set; } = default!;

        public virtual TAuditedOn AuditedOn { get; set; } = default!;

        public override PersistChange PersistChange { get; set; }

        public AuditEntity()
            : this(PersistChange.None)
        {
        }

        protected AuditEntity(PersistChange persistChange)
            : base(persistChange)
        {
        }

        protected AuditEntity(AuditEntity<TId, TAuditedBy, TAuditedOn> entity)
            : base(entity)
        {
            AuditedBy = entity.AuditedBy;
            AuditedOn = entity.AuditedOn;
        }
    }
}
