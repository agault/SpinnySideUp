namespace SpinnySideUp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class namechangepilotidinflightmodel : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Flights", name: "PilotId", newName: "UserId");
            RenameIndex(table: "dbo.Flights", name: "IX_PilotId", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Flights", name: "IX_UserId", newName: "IX_PilotId");
            RenameColumn(table: "dbo.Flights", name: "UserId", newName: "PilotId");
        }
    }
}
