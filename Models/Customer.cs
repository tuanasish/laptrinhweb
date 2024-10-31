using System;
using System.Collections.Generic;

namespace Web_APP_BTL.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? ContactInfo { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
