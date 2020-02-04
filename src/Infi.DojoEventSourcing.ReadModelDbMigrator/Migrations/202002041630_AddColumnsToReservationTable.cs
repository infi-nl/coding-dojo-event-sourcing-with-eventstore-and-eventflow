using FluentMigrator;

namespace Infi.DojoEventSourcing.ReadModelDbMigrator.Migrations
{
    [Migration(202002041631)]
    public class AddColumnsToReservationTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Reservation")
                .AlterColumn("AggregateId").AsString().PrimaryKey()
                .AddColumn("Email").AsString().Nullable()
                .AddColumn("Name").AsString().Nullable()
                .AddColumn("Status").AsString().Nullable()
                .AddColumn("RoomNumber").AsString().Nullable()
                .AddColumn("RoomId").AsString().Nullable()
                .AddColumn("CheckOutTime").AsDateTime().Nullable()
                .AddColumn("CheckInTime").AsDateTime().Nullable()
                .AddColumn("Departure").AsDateTime().Nullable()
                .AddColumn("Arrival").AsDateTime().Nullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}