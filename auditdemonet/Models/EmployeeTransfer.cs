using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace auditdemonet.Models;

public partial class EmployeeTransfer
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int FromDepartmentId { get; set; }

    public int ToDepartmentId { get; set; }

    public DateTime TransferDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Reason { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ApprovedBy { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("EmployeeTransfers")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("FromDepartmentId")]
    [InverseProperty("EmployeeTransferFromDepartments")]
    public virtual Department FromDepartment { get; set; } = null!;

    [ForeignKey("ToDepartmentId")]
    [InverseProperty("EmployeeTransferToDepartments")]
    public virtual Department ToDepartment { get; set; } = null!;
}
