using System;
using FluentMigrator;

namespace Infi.DojoEventSourcing.ReadModelDbMigrator.Migrations
{
    [Migration(202002041512)]
    public class AddRoomReadModels : Migration
    {
        public override void Up()
        {
            Create.Table("Room")
                .WithColumn("AggregateId").AsString().PrimaryKey()
                .WithColumn("RoomNumber").AsInt32();
                
            Create.Table("RoomOccupation")
                .WithColumn("OccupationId").AsString().PrimaryKey()
                .WithColumn("AggregateId").AsString().Indexed()
                .WithColumn("StartDate").AsDateTime2()
                .WithColumn("EndDate").AsDateTime2();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}