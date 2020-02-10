using System;
using FluentMigrator;

namespace Infi.DojoEventSourcing.ReadModelDbMigrator.Migrations
{
    [Migration(202001101307)]
    public class AddReservationTable : Migration
    {
        public override void Up()
        {
            Create.Table("Reservation")
                .WithColumn("AggregateId").AsString().PrimaryKey();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}