using System;
using System.Collections.Generic;

namespace Web_APP_BTL.Models;

public partial class InventoryTransaction
{
    public int TransactionId { get; set; }

    public int? WarehouseId { get; set; }

    public int? ProductId { get; set; }

    public string TransactionType { get; set; } = null!;

    public int Quantity { get; set; }

    public DateTime TransactionDate { get; set; }

    public int? SupplierId { get; set; }

    public string? Reason { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Supplier? Supplier { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
