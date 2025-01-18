using FluentMigrator;

namespace DataAccessLayer.ManualMigrations;

[Tags("Postgres")]
[Migration(202501010001)] // A unique number for this migration, like a date
public class Init : Migration
{
    public override void Up()
    {
        Insert.IntoTable("Categories").Row(new
        {
            Id = Guid.NewGuid(),
            Name = "string",
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
        });
    }

    public override void Down()
    {
        //Will take care in next migration
    }
}