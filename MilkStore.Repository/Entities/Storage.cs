using System;
using System.Collections.Generic;

namespace MilkStore.Repo.Entities;

public partial class Storage
{
    public int StorageID { get; set; }

    public string StorageName { get; set; } = null!;

    public virtual ICollection<DeliveryMan> DeliveryMen { get; set; } = new List<DeliveryMan>();
    public int Delete { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
