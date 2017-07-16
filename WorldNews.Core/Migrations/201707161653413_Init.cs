namespace WorldNews.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(nullable: false, maxLength: 100),
                        ShortDescription = c.String(nullable: false, maxLength: 200),
                        PhotoLink = c.String(nullable: false, maxLength: 200),
                        Text = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ModeratorEntities", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.CategoryEntities", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.ModeratorEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PhotoLink = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.CommentEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 200),
                        DateCreated = c.DateTime(nullable: false),
                        DateBanned = c.DateTime(),
                        AuthorId = c.String(nullable: false, maxLength: 128),
                        ArticleId = c.Int(nullable: false),
                        ModeratorWhoBannedId = c.String(maxLength: 128),
                        BanReasonId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleEntities", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.UserEntities", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.BanReasonEntities", t => t.BanReasonId)
                .ForeignKey("dbo.ModeratorEntities", t => t.ModeratorWhoBannedId)
                .Index(t => t.AuthorId)
                .Index(t => t.ArticleId)
                .Index(t => t.ModeratorWhoBannedId)
                .Index(t => t.BanReasonId);
            
            CreateTable(
                "dbo.UserEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.BanReasonEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ArticleEntities", "CategoryId", "dbo.CategoryEntities");
            DropForeignKey("dbo.ArticleEntities", "AuthorId", "dbo.ModeratorEntities");
            DropForeignKey("dbo.CommentEntities", "ModeratorWhoBannedId", "dbo.ModeratorEntities");
            DropForeignKey("dbo.CommentEntities", "BanReasonId", "dbo.BanReasonEntities");
            DropForeignKey("dbo.CommentEntities", "AuthorId", "dbo.UserEntities");
            DropForeignKey("dbo.UserEntities", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ModeratorEntities", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommentEntities", "ArticleId", "dbo.ArticleEntities");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UserEntities", new[] { "Id" });
            DropIndex("dbo.CommentEntities", new[] { "BanReasonId" });
            DropIndex("dbo.CommentEntities", new[] { "ModeratorWhoBannedId" });
            DropIndex("dbo.CommentEntities", new[] { "ArticleId" });
            DropIndex("dbo.CommentEntities", new[] { "AuthorId" });
            DropIndex("dbo.ModeratorEntities", new[] { "Id" });
            DropIndex("dbo.ArticleEntities", new[] { "AuthorId" });
            DropIndex("dbo.ArticleEntities", new[] { "CategoryId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.CategoryEntities");
            DropTable("dbo.BanReasonEntities");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserEntities");
            DropTable("dbo.CommentEntities");
            DropTable("dbo.ModeratorEntities");
            DropTable("dbo.ArticleEntities");
        }
    }
}
