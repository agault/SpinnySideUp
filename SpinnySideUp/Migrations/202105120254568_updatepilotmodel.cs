namespace SpinnySideUp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatepilotmodel : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Pilots", name: "ApplicationUser_Id", newName: "UserId");
            RenameIndex(table: "dbo.Pilots", name: "IX_ApplicationUser_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Pilots", name: "IX_UserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Pilots", name: "UserId", newName: "ApplicationUser_Id");
        }
    }
}
