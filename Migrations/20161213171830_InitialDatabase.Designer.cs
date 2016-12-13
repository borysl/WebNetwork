using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WebNetwork.Models;

namespace WebNetwork.Migrations
{
    [DbContext(typeof(NetworkContext))]
    [Migration("20161213171830_InitialDatabase")]
    partial class InitialDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("WebNetwork.Models.Asset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("asset_id");

                    b.Property<string>("Name");

                    b.Property<int?>("SiteId");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("WebNetwork.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("service_id");

                    b.Property<int?>("InputAssetId");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<int?>("OutputAssetId");

                    b.HasKey("Id");

                    b.HasIndex("InputAssetId");

                    b.HasIndex("OutputAssetId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("WebNetwork.Models.Site", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("site_id");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<double>("GpsX")
                        .HasColumnName("gps_x");

                    b.Property<double>("GpsY")
                        .HasColumnName("gps_y");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<string>("Trigram")
                        .HasColumnName("trigram");

                    b.HasKey("Id");

                    b.ToTable("Site");
                });

            modelBuilder.Entity("WebNetwork.Models.Asset", b =>
                {
                    b.HasOne("WebNetwork.Models.Site", "Site")
                        .WithMany()
                        .HasForeignKey("SiteId");
                });

            modelBuilder.Entity("WebNetwork.Models.Service", b =>
                {
                    b.HasOne("WebNetwork.Models.Asset", "InputAsset")
                        .WithMany()
                        .HasForeignKey("InputAssetId");

                    b.HasOne("WebNetwork.Models.Asset", "OutputAsset")
                        .WithMany()
                        .HasForeignKey("OutputAssetId");
                });
        }
    }
}
