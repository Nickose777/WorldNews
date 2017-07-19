namespace WorldNews.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BanReasonEnable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BanReasonEntities", "IsEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.CategoryEntities", "IsEnabled", c => c.Boolean(nullable: false));
            DropColumn("dbo.CategoryEntities", "IsDisabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CategoryEntities", "IsDisabled", c => c.Boolean(nullable: false));
            DropColumn("dbo.CategoryEntities", "IsEnabled");
            DropColumn("dbo.BanReasonEntities", "IsEnabled");
        }
    }
}
