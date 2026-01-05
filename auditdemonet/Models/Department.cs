using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace auditdemonet.Models;

public partial class Department
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [InverseProperty("FromDepartment")]
    public virtual ICollection<EmployeeTransfer> EmployeeTransferFromDepartments { get; set; } = new List<EmployeeTransfer>();

    [InverseProperty("ToDepartment")]
    public virtual ICollection<EmployeeTransfer> EmployeeTransferToDepartments { get; set; } = new List<EmployeeTransfer>();

    [InverseProperty("Department")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
