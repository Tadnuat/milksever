﻿using System;
using System.Collections.Generic;

namespace MilkStore.Repo.Entities;

public partial class Admin
{
    public int AdminID { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
    public int Delete { get; set; }
}
