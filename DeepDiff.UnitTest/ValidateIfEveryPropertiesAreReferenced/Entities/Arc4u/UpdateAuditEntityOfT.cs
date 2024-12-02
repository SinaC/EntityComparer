namespace DeepDiff.UnitTest.ValidateIfEveryPropertiesAreReferenced.Entities.Arc4u
{
    public abstract class UpdateAuditEntity<TId, TCreatedBy, TCreatedOn, TUpdatedBy, TUpdatedOn> : CreateAuditEntity<TId, TCreatedBy, TCreatedOn>, IUpdateAuditEntity<TCreatedBy, TCreatedOn, TUpdatedBy, TUpdatedOn>, ICreateAuditEntity<TCreatedBy, TCreatedOn>, IUpdateAuditEntity<TUpdatedBy, TUpdatedOn>
    {
        public virtual TUpdatedBy UpdatedBy { get; set; } = default!;

        public virtual TUpdatedOn UpdatedOn { get; set; } = default!;

        public UpdateAuditEntity()
            : this(PersistChange.None)
        {
        }

        protected UpdateAuditEntity(PersistChange persistChange)
            : base(persistChange)
        {
        }

        protected UpdateAuditEntity(UpdateAuditEntity<TId, TCreatedBy, TCreatedOn, TUpdatedBy, TUpdatedOn> entity)
            : base((CreateAuditEntity<TId, TCreatedBy, TCreatedOn>)entity)
        {
            UpdatedBy = entity.UpdatedBy;
            UpdatedOn = entity.UpdatedOn;
        }
    }
}
