using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace auditdemonet.Models;

[Microsoft.EntityFrameworkCore.Index("CreatedDate", Name = "IX_AuditEvents_CreatedDate")]
public partial class AuditEvent
{
    [Key]
    public int EventId { get; set; }

    [StringLength(200)]
    public string? EventType { get; set; }

    public string Data { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
}
