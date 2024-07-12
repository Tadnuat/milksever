﻿using System;
using System.Collections.Generic;

namespace MilkStore.Repo.Entities;

public partial class AgeRange
{
    public int AgeRangeID { get; set; }

    public string? Baby { get; set; }

    public string? Mama { get; set; }

    public int? ProductItemID { get; set; }
    public int Delete { get; set; }
    public virtual ProductItem? ProductItem { get; set; }
}
