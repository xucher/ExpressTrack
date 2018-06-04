﻿using ExpressTrack.Models;
using System.Data.Entity;

namespace ExpressTrack {
    class ExpressDBContext : DbContext {
        public ExpressDBContext(): base("name=ExpressDB") {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ExpressDBContext>());
        }
        public DbSet<Express> Express { get; set; }
        public DbSet<Shipment> Shipment { get; set; }
        public DbSet<Station> Station { get; set; }
        public DbSet<User> User { get; set; }
    }
}
