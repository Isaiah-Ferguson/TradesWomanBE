﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TradesWomanBE.Services.Context;

#nullable disable

namespace TradesWomanBE.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TradesWomanBE.Models.AdminUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AdminUsers");
                });

            modelBuilder.Entity("TradesWomanBE.Models.ClientModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ActiveOrFormerMilitary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int?>("ChildrenOverSix")
                        .HasColumnType("int");

                    b.Property<int?>("ChildrenUnderSix")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CriminalHistory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateJoinedEAW")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateOfBirth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateRegistered")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Disabled")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Firstname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FoundUsOn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Lastname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MeetingsId")
                        .HasColumnType("int");

                    b.Property<string>("MiddleInitial")
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProgramInfoId")
                        .HasColumnType("int");

                    b.Property<string>("RecruiterName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SSNLastFour")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StipendsId")
                        .HasColumnType("int");

                    b.Property<int?>("TotalHouseholdFamily")
                        .HasColumnType("int");

                    b.Property<int?>("TotalMonthlyIncome")
                        .HasColumnType("int");

                    b.Property<string>("ValidSSNAuthToWrk")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ZipCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MeetingsId");

                    b.HasIndex("ProgramInfoId");

                    b.HasIndex("StipendsId");

                    b.ToTable("ClientInfo");
                });

            modelBuilder.Entity("TradesWomanBE.Models.MeetingNotesModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MeetingId")
                        .HasColumnType("int");

                    b.Property<int?>("MeetingsModelId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecruiterInfo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MeetingsModelId");

                    b.ToTable("MeetingNotes");
                });

            modelBuilder.Entity("TradesWomanBE.Models.MeetingsModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientID")
                        .HasColumnType("int");

                    b.Property<string>("GrantName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastContactMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastDateContacted")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumOfContacts")
                        .HasColumnType("int");

                    b.Property<string>("PreferedContact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecruiterName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("TradesWomanBE.Models.ProgramModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientID")
                        .HasColumnType("int");

                    b.Property<string>("CurrentStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnrollDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProgramEndDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProgramEnrolled")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Programs");
                });

            modelBuilder.Entity("TradesWomanBE.Models.RecruiterModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FirstTimeLogIn")
                        .HasColumnType("bit");

                    b.Property<string>("Firstname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Lastname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleInnitial")
                        .HasColumnType("nvarchar(1)");

                    b.Property<int?>("PhoneNumber")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SuperviserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RecruiterInfo");
                });

            modelBuilder.Entity("TradesWomanBE.Models.StipendsModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("IssuedDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreApprenticeshipProgram")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestedDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StipendAmountRequested")
                        .HasColumnType("int");

                    b.Property<string>("StipendDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StipendPaymentMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeOfStipend")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Stipends");
                });

            modelBuilder.Entity("TradesWomanBE.Models.ClientModel", b =>
                {
                    b.HasOne("TradesWomanBE.Models.MeetingsModel", "Meetings")
                        .WithMany()
                        .HasForeignKey("MeetingsId");

                    b.HasOne("TradesWomanBE.Models.ProgramModel", "ProgramInfo")
                        .WithMany()
                        .HasForeignKey("ProgramInfoId");

                    b.HasOne("TradesWomanBE.Models.StipendsModel", "Stipends")
                        .WithMany()
                        .HasForeignKey("StipendsId");

                    b.Navigation("Meetings");

                    b.Navigation("ProgramInfo");

                    b.Navigation("Stipends");
                });

            modelBuilder.Entity("TradesWomanBE.Models.MeetingNotesModel", b =>
                {
                    b.HasOne("TradesWomanBE.Models.MeetingsModel", null)
                        .WithMany("MeetingNotes")
                        .HasForeignKey("MeetingsModelId");
                });

            modelBuilder.Entity("TradesWomanBE.Models.MeetingsModel", b =>
                {
                    b.Navigation("MeetingNotes");
                });
#pragma warning restore 612, 618
        }
    }
}
