using System;
using System.Collections.Generic;

namespace Web_APP_BTL.Models;

public partial class SalesPromotion
{
    public int SalesId { get; set; }

    public int PromotionId { get; set; }

    public int? DiscountId { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual Promotion Promotion { get; set; } = null!;

    public virtual Sale Sales { get; set; } = null!;
}
