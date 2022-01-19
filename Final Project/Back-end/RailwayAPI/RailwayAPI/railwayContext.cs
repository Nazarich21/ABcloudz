using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Innofactor.EfCoreJsonValueConverter;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;


#nullable disable

namespace RailwayAPI
{
    public sealed class railwayContext : DbContext
    {
        //private static railwayContext instance = null;
        //private static readonly object padlock = new object();

        //railwayContext()
        //{
        //}

        //public static railwayContext Instance
        //{
        //    get
        //    {
        //        lock (padlock)
        //        {
        //            if (instance == null)
        //            {
        //                instance = new railwayContext();
        //            }
        //            return instance;
        //        }
        //    }
        //}

        //private static railwayContext _instance = null;
        //public static railwayContext Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            _instance = new railwayContext();
        //        }
        //        return _instance;
        //    }
        //}
        //public railwayContext()
        //{

        //}

        public railwayContext(DbContextOptions<railwayContext> options)
            : base(options)
        {

        }

        public DbSet<Carriage> Carriages { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteStation> RouteStations { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<User> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection1");
            optionsBuilder.UseNpgsql(connectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "English_United States.1252");


            modelBuilder.Entity<Carriage>(entity =>
            {
                entity.ToTable("carriages");

                entity.HasIndex(e => e.Id, "carriages_un")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("carriageId");

                entity.Property(e => e.Guide1Id).HasColumnName("guide1_Id");

                entity.Property(e => e.Guide2Id).HasColumnName("guide2_Id");

                entity.Property(e => e.TrainId).HasColumnName("train_Id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("type");

                entity.HasOne<Employee>()
                  .WithMany()
                  .HasForeignKey(p => p.Guide1Id);
                entity.HasOne<Employee>()
                   .WithMany()
                   .HasForeignKey(p => p.Guide2Id);
                entity.HasOne<Train>()
                   .WithMany()
                   .HasForeignKey(p => p.TrainId);


            });

            modelBuilder.Entity<Employee>(entity =>
            {

                entity.ToTable("employees");

                entity.HasIndex(e => e.Id, "employees_un")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("employeeId");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasColumnName("birthday");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("first_name");

                entity.Property(e => e.HomeAdress)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("home_adress");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("last_name");

                entity.Property(e => e.Patronymic)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("patronymic");

                entity.Property(e => e.Phones)
                    .IsRequired()
                    .HasColumnType("json")
                    .HasColumnName("phones");

                entity.Property(e => e.Photo)
                    .IsRequired()
                    .HasColumnName("photo");

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("position");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("routes_pk");

                entity.ToTable("routes");

                entity.Property(e => e.Id)
                     .ValueGeneratedNever()
                    .HasColumnName("routeId");

                entity.Property(e => e.ArrivalTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("arrival_time");

                entity.Property(e => e.DepartureTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("departure_time");

                entity.Property(e => e.FirstStationId)
                    .HasMaxLength(30)
                    .HasColumnName("first_station_id");

                entity.Property(e => e.LastStationId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("last_station_id");

                entity.HasOne<Station>()
                   .WithMany()
                   .HasForeignKey(p => p.LastStationId);

                entity.HasOne<Station>()
                   .WithMany()
                   .HasForeignKey(p => p.FirstStationId);

            });

            modelBuilder.Entity<RouteStation>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.StationId, e.Order })
                    .HasName("route_stations_pk");

                entity.ToTable("route_stations");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("route_id");

                entity.Property(e => e.StationId)
                    .HasMaxLength(30)
                    .HasColumnName("station_id");

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.ArrivalTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("arrival_time");

                entity.Property(e => e.DepartureTime)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("departure_time");

                entity.HasOne<Route>()
                     .WithMany()
                     .HasForeignKey(p => p.Id);

                entity.HasOne<Station>()
                     .WithMany()
                     .HasForeignKey(p => p.StationId);
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(e => new { e.SeatNo, e.Id })
                    .HasName("seats_pk");

                entity.ToTable("seats");

                entity.Property(e => e.SeatNo)
                    .HasMaxLength(30)
                    .HasColumnName("seat_no");

                entity.Property(e => e.Id).HasColumnName("cariage_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("status");

                entity.HasOne<Carriage>()
                   .WithMany()

                   .HasForeignKey(p => p.Id);
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("stations_pk");

                entity.ToTable("stations");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .HasColumnName("codeId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("tickets");

                entity.HasIndex(e => e.Ticketid, "tickets_un")
                    .IsUnique();

                entity.Property(e => e.Ticketid)
                    .ValueGeneratedNever()
                    .HasColumnName("ticketid");

                entity.Property(e => e.ArrivalStationId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("arrival_station_id");

                entity.Property(e => e.CarriageId).HasColumnName("carriage_id");



                entity.Property(e => e.DepartureDate)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("departure_date");

                entity.Property(e => e.DepartureStationId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("departure_station_id");

                entity.Property(e => e.DocumentNumber)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("document_number");

                entity.Property(e => e.DocumentType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("document_type");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("last_name");

                entity.Property(e => e.OperationDate)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("operation_date");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.Seat)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("seat");

                entity.Property(e => e.Status)
                   .IsRequired()
                   .HasMaxLength(30)
                   .HasColumnName("status");

                entity.Property(e => e.TrainId).HasColumnName("train_id");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("user_id");


                entity.HasOne<Station>()
                   .WithMany()
                   .HasForeignKey(p => p.ArrivalStationId);

                entity.HasOne<Station>()
                   .WithMany()
                   .HasForeignKey(p => p.DepartureStationId);
                entity.HasOne<Carriage>()
                   .WithMany()
                   .HasForeignKey(p => p.CarriageId);
                entity.HasOne<Train>()
                   .WithMany()
                   .HasForeignKey(p => p.TrainId);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(p => p.UserId);
            });

            modelBuilder.Entity<Timetable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("timetable");

                entity.Property(e => e.ArrivalStation)
                    .HasMaxLength(30)
                    .HasColumnName("arrival_station");

                entity.Property(e => e.ArrivalTime)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("arrival_time");

                entity.Property(e => e.DepartureStation)
                    .HasMaxLength(30)
                    .HasColumnName("departure_station");

                entity.Property(e => e.DepartureTime)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("departure_time");

                entity.Property(e => e.Status)
                    .HasMaxLength(30)
                    .HasColumnName("status");

                entity.Property(e => e.Train).HasColumnName("train");
            });

            modelBuilder.Entity<Train>(entity =>
            {
                entity.ToTable("trains");

                entity.HasIndex(e => e.Id, "trains_un")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("trainId");

                entity.Property(e => e.DriverAssistId)
                .HasColumnName("driver_assist_Id");

                entity.Property(e => e.DriverId)
                .HasColumnName("driver_Id");

                entity.Property(e => e.EveryDay)
                .HasColumnName("every_day");

                entity.Property(e => e.EvenDays)
                .HasColumnName("even_days");

                entity.Property(e => e.Days)
                .HasColumnName("days");


                entity.Property(e => e.RouteId)
                    .IsRequired()
                    .ValueGeneratedNever()
                    .HasColumnName("route_Id");

                entity.Property(e => e.TrainmasterId)
                .HasColumnName("trainmaster_id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("type");
                entity.HasOne<Employee>()
                   .WithMany()
                   .HasForeignKey(p => p.DriverAssistId);
                entity.HasOne<Employee>()
                   .WithMany()
                   .HasForeignKey(p => p.DriverId);
                entity.HasOne<Employee>()
                   .WithMany()
                   .HasForeignKey(p => p.TrainmasterId);
                entity.HasOne<Route>()
                   .WithMany()
                   .HasForeignKey(p => p.RouteId);


            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.id)
                    .HasName("users_pk");

                entity.ToTable("users");

                entity.Property(e => e.id)
                    .HasMaxLength(30)
                    .HasColumnName("loginId");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("role");

                entity.Property(e => e.Blocked)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnName("blocked");
            });

            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder) { };
    }
}
