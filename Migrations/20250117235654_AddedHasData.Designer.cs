﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace draft_data.Migrations
{
    [DbContext(typeof(DraftContext))]
    [Migration("20250117235654_AddedHasData")]
    partial class AddedHasData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("DataSet", b =>
                {
                    b.Property<int>("DataSetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("DataSetId");

                    b.ToTable("DataSets");

                    b.HasData(
                        new
                        {
                            DataSetId = 1,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            DataSetId = 2,
                            CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Draftee", b =>
                {
                    b.Property<int>("DrafteeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BirthDateYear")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DataSetId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Department")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Info")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Position")
                        .HasColumnType("INTEGER");

                    b.HasKey("DrafteeId");

                    b.HasIndex("DataSetId");

                    b.ToTable("Draftee");
                });

            modelBuilder.Entity("KeyValueCombo", b =>
                {
                    b.Property<int>("KeyValueComboId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DataSetId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("KeyValueComboId");

                    b.HasIndex("DataSetId");

                    b.ToTable("KeyValueCombo");
                });

            modelBuilder.Entity("Draftee", b =>
                {
                    b.HasOne("DataSet", null)
                        .WithMany("Draftees")
                        .HasForeignKey("DataSetId");
                });

            modelBuilder.Entity("KeyValueCombo", b =>
                {
                    b.HasOne("DataSet", null)
                        .WithMany("AdditionalProperties")
                        .HasForeignKey("DataSetId");
                });

            modelBuilder.Entity("DataSet", b =>
                {
                    b.Navigation("AdditionalProperties");

                    b.Navigation("Draftees");
                });
#pragma warning restore 612, 618
        }
    }
}
