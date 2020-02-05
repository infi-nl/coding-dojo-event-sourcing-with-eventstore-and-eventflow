using FluentMigrator;

namespace Infi.DojoEventSourcing.ReadModelDbMigrator.Migrations
{
    [Migration(202002041512)]
    public class AddRoomReadModels : Migration
    {
        public override void Up()
        {
            Create.Table("Room")
                .WithColumn("AggregateId").AsGuid().PrimaryKey()
                .WithColumn("RoomNumber").AsInt32();
                
            Create.Table("RoomOccupation")
                .WithColumn("OccupationId").AsInt32().Identity().PrimaryKey()
                .WithColumn("AggregateId").AsGuid()
                .WithColumn("StartDate").AsDateTime2()
                .WithColumn("EndDate").AsDateTime2();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}