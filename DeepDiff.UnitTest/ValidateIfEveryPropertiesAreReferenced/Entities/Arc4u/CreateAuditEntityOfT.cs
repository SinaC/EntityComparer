namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u
{
    public abstract class CreateAuditEntity<TId, TCreatedBy, TCreatedOn> : IdEntity<TId>, ICreateAuditEntity<TCreatedBy, TCreatedOn>
    {
        public virtual TCreatedBy CreatedBy { get; set; } = default!;

        public virtual TCreatedOn CreatedOn { get; set; } = default!;

        public CreateAuditEntity()
            : this(PersistChange.None)
        {
        }

        public CreateAuditEntity(PersistChange persistChange)
            : base(persistChange)
        {
        }

        protected CreateAuditEntity(CreateAuditEntity<TId, TCreatedBy, TCreatedOn> entity)
            : base((IdEntity<TId>)entity)
        {
            CreatedBy = entity.CreatedBy;
            CreatedOn = entity.CreatedOn;
        }
    }
}
