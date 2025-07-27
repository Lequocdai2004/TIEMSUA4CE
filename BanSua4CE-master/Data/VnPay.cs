using System;
using System.Collections.Generic;

namespace BanSua4CE.Data;

public partial class VnPay
{
    public int Id { get; set; }

    public string? OrderDescription { get; set; }

    public string? TransactionId { get; set; }

    public string? OrderId { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? DatedCreated { get; set; } = DateTime.Now;
}
