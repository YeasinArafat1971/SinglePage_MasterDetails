using System;
using System.Collections.Generic;

namespace SPMasterDetails.Models
{
    public partial class SaleMaster
    {
        public long SaleId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? Gender { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
