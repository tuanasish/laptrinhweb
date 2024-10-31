using System;
using System.Collections.Generic;

namespace Web_APP_BTL.Models;

public partial class Sale
{
    public int SalesId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime SalesDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();

    public virtual ICollection<SalesPromotion> SalesPromotions { get; set; } = new List<SalesPromotion>();
}
