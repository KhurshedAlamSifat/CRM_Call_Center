﻿using System;

namespace Base.Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string ErrorCode { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    }
}
