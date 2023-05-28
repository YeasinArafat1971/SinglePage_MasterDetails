using System;
using System.Collections.Generic;

namespace SPMasterDetails.Models
{
    public partial class SaleDetail
    {
        public long SaleDetailId { get; set; }
        public string? ProductName { get; set; }
        public long? SaleId { get; set; }
        public decimal? Price { get; set; }
    }
}
