using System;
using System.Collections.Generic;

namespace Web_APP_BTL.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string PromotionName { get; set; } = null!;

    public string PromotionType { get; set; } = null!;

    public int? DiscountId { get; set; }

    public int? ProductId { get; set; }

    public int? MinQuantity { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ICollection<SalesPromotion> SalesPromotions { get; set; } = new List<SalesPromotion>();
}
