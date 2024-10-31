using System;
using System.Collections.Generic;

namespace Web_APP_BTL.Models;

public partial class SalesDetail
{
    public int SalesDetailId { get; set; }

    public int? SalesId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int? DiscountId { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal FinalPrice { get; set; }

    public decimal? Tax { get; set; }

    public decimal? ShippingFee { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Sale? Sales { get; set; }
}
