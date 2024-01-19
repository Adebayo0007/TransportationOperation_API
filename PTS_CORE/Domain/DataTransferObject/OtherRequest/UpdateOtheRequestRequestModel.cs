﻿using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.OtherRequest
{
    public class UpdateOtheRequestRequestModel
    {
        [Required]
        public string Id { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public AvailabilityType? AvailabilityType { get; set; }
        public bool? IsChairmanApproved { get; set; } = false;
        public RequestType? RequestType { get; set; }
        public bool? IsDDPCommented { get; set; } = false;
        public string? DDPComment { get; set; }
        public bool? IsAuditorCommented { get; set; } = false;
        public string? AuditorComment { get; set; }
        public bool? IsResolved { get; set; } = false;
        public bool? IsVerified { get; set; } = false;
    }
}