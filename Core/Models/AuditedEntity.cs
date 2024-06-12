using System;

namespace Core.Models
{
    public abstract class AuditedEntity
    {
        public DateTime ModifiedAt { get; private set; } = DateTime.Now;
        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        protected virtual void Update()
        {
            ModifiedAt = DateTime.Now;
        }
    }
}