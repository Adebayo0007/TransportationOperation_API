﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.OtherRequest
{
    public class CreateOtherRequestRequestModel
    {
        [Required]
        public string Subject { get; set; }
        [Required]  
        public string Content { get; set; }
    }
}
