﻿

namespace PTS_CORE.Domain.Entities
{
    public class Expenditure : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}