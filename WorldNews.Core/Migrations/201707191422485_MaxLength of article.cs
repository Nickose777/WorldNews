namespace WorldNews.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaxLengthofarticle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ArticleEntities", "Header", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.ArticleEntities", "ShortDescription", c => c.String(nullable: false, maxLength: 400));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ArticleEntities", "ShortDescription", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.ArticleEntities", "Header", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
