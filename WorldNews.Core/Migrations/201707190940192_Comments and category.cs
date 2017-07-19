namespace WorldNews.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Commentsandcategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoryEntities", "IsDisabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CategoryEntities", "IsDisabled");
        }
    }
}
