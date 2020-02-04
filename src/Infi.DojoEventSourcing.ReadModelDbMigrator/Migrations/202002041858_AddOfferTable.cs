using FluentMigrator;

namespace Infi.DojoEventSourcing.ReadModelDbMigrator.Migrations
{
    [Migration(202002041858)]
    public class AddOfferTable : Migration
    {
        public override void Up()
        {
            Create.Table("Offer")
                .WithColumn("AggregateId").AsString().PrimaryKey()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("Expires").AsDateTime().NotNullable()
                .WithColumn("Price").AsDecimal(9,4).NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}