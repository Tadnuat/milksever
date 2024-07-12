using System;
using System.Collections.Generic;

namespace MilkStore.Repo.Entities;

public partial class Payment
{
    public int PaymentID { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public int? OrderID { get; set; }
    public int Delete { get; set; }
    public virtual Order? Order { get; set; }
}
