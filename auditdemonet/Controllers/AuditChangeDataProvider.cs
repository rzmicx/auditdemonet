using Audit.Core;
using Audit.EntityFramework;
using Microsoft.Data.SqlClient;
using System.Text.Json;

public class AuditChangeDataProvider : AuditDataProvider
{
    public override object InsertEvent(AuditEvent auditEvent)
    {
        var efEvent = auditEvent.GetEntityFrameworkEvent();
        if (efEvent == null) return null;

        using var conn = new SqlConnection("Server=localhost;Database=AuditDemoDB;Trusted_Connection=True;TrustServerCertificate=True");
        conn.Open();

        foreach (var entry in efEvent.Entries)
        {
            foreach (var change in entry.Changes)
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO AuditChanges
                    (TableName, Action, ColumnName, BeforeValue, AfterValue, PrimaryKey)
                    VALUES
                    (@table, @action, @column, @before, @after, @pk)", conn);

                cmd.Parameters.AddWithValue("@table", entry.Table);
                cmd.Parameters.AddWithValue("@action", entry.Action);
                cmd.Parameters.AddWithValue("@column", change.ColumnName);
                cmd.Parameters.AddWithValue("@before", change.OriginalValue?.ToString());
                cmd.Parameters.AddWithValue("@after", change.NewValue?.ToString());
                cmd.Parameters.AddWithValue("@pk", JsonSerializer.Serialize(entry.PrimaryKey)); 

                cmd.ExecuteNonQuery();
            }
        }

        return null;
    }
}
