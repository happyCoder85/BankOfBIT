namespace BankOfBit_JS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stored_procedure : DbMigration
    {
        public override void Up()
        {
            // Call script to create the stored procedure
            this.Sql(Properties.StoredProcs.create_next_number);
        }
        
        public override void Down()
        {
            // Call script to drop the stored procedure
            this.Sql(Properties.StoredProcs.drop_next_number);
        }
    }
}
