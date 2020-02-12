using FluentMigrator;

namespace Infi.DojoEventSourcing.ReadModelDbMigrator.Migrations
{
    [Migration(202002121152)]
    public class AddTotalPriceToReservation : Migration
    {
        public override void Up()
        {
            Alter.Table("Reservation").AddColumn("TotalPrice").AsDecimal().Nullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}