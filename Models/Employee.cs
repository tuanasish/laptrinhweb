using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web_APP_BTL.Models;

public partial class Employee
{
    [Key]
    public int EmpId { get; set; }

    public string EmpName { get; set; } = null!;


    public string EmpPw { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime HireDate { get; set; }

    public int? DepartmentId { get; set; }

    public int? PositionId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Position? Position { get; set; }
}