﻿// MIT License

namespace Leo.Data.Domain.Entities
{
    public interface IAuditableEntity
    {
        DateTime? CreatedAt { get; set; }

        string? CreatedBy { get; set; }

        DateTime? UpdatedAt { get; set; }

        string? UpdatedBy { get; set; }

        bool? IsDeleted { get; set; }

        DateTime? DeletedAt { get; set; }
    }
}
