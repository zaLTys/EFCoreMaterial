using FluentMigrator;

namespace DataAccessLayer.ManualMigrations;

[Tags("Postgres")]
[Migration(202501010002)]
public class SqlFromFile : Migration
{
    public override void Up()
    {
        Console.WriteLine("202501010002_SqlFromFile");

        var path = @"../DataAccessLayer/ManualMigrations/Scripts/202501010002_SqlFromFile.sql";
        // Execute the script
        Execute.Script(path);
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}