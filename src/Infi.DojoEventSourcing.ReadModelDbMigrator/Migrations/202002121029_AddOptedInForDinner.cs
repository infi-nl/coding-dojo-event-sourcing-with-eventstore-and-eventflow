using System;
using FluentMigrator;

namespace Infi.DojoEventSourcing.ReadModelDbMigrator.Migrations
{
    [Migration(202002121029)]
    public class AddOptedInForDinner: Migration
    {
        public override void Up()
        {
            Alter.Table("Reservation")
                .AddColumn("OptedInForDinner").AsBoolean().SetExistingRowsTo(0);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}