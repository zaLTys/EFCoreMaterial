using FluentMigrator.Runner.VersionTableInfo;

namespace DataAccessLayer.ManualMigrations.VersionMetadata;

[VersionTableMetaData]
public class CustomVersionTableMetaData : IVersionTableMetaData
{
    public object ApplicationContext { get; set; }
    public string SchemaName => "public";
    public string TableName => "__LevelUpManualMigrationsHistory";
    public string ColumnName => "Version";
    public string UniqueIndexName => "IX_LevelUpMigration_Version";
    public string AppliedOnColumnName => "AppliedOn";
    public bool CreateWithPrimaryKey => false;
    public string DescriptionColumnName => "Description";
    public bool OwnsSchema => false;
}