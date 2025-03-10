﻿// <auto-generated />
using System;
using CoursesWebAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoursesWebAPI.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231002110703_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ActivityContract", b =>
                {
                    b.Property<Guid>("ClassesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ContractsId")
                        .HasColumnType("uuid");

                    b.HasKey("ClassesId", "ContractsId");

                    b.HasIndex("ContractsId");

                    b.ToTable("ActivityContract");
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ActivityTypeId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Begin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid?>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("End")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ActivityTypeId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.ActivityType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ActivityTypes");
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.Contract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Closed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset>("Concluded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Cost")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(7, 2)
                        .HasColumnType("numeric(7,2)")
                        .HasDefaultValue(0m);

                    b.Property<short>("Hours")
                        .HasColumnType("smallint");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<short>("PaidHours")
                        .HasColumnType("smallint");

                    b.Property<short>("PassedHours")
                        .HasColumnType("smallint");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("StudentId");

                    b.ToTable("Contracts", (string)null);
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FirsName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .IsFixedLength(false);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .IsFixedLength(false);

                    b.Property<string>("Patronomic")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .IsFixedLength(false);

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<byte>("Sex")
                        .HasColumnType("smallint");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Persosns", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Identity.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("Expires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("JwtId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens", (string)null);
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Identity.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(36)
                        .HasColumnType("character(36)")
                        .IsFixedLength();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .IsFixedLength(false);

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .IsFixedLength(false);

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.HasIndex("UserId");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Identity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(36)
                        .HasColumnType("character(36)")
                        .IsFixedLength();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .IsFixedLength(false);

                    b.Property<bool>("EmailConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .IsFixedLength(false);

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .IsFixedLength(false);

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(84)
                        .HasColumnType("character(84)")
                        .IsFixedLength();

                    b.Property<Guid?>("PersonId")
                        .HasColumnType("uuid");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(14)
                        .HasColumnType("character varying(14)")
                        .IsFixedLength(false);

                    b.Property<bool>("PhoneNumberConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("SecurityStamp")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .IsFixedLength(false);

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .IsFixedLength(false);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .IsFixedLength(false);

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .IsFixedLength(false);

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .IsFixedLength(false);

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .IsFixedLength(false);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .IsFixedLength(false);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .IsFixedLength(false);

                    b.Property<string>("ProviderDisplayName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .IsFixedLength(false);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .IsFixedLength(false);

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .IsFixedLength(false);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .IsFixedLength(false);

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.Employee", b =>
                {
                    b.HasBaseType("CoursesWebAPI.Core.Entities.Person");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.Student", b =>
                {
                    b.HasBaseType("CoursesWebAPI.Core.Entities.Person");

                    b.ToTable("Students", (string)null);
                });

            modelBuilder.Entity("ActivityContract", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Entities.Activity", null)
                        .WithMany()
                        .HasForeignKey("ClassesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoursesWebAPI.Core.Entities.Contract", null)
                        .WithMany()
                        .HasForeignKey("ContractsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.Activity", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Entities.ActivityType", "ActivityType")
                        .WithMany()
                        .HasForeignKey("ActivityTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoursesWebAPI.Core.Entities.Employee", null)
                        .WithMany("Activities")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("CoursesWebAPI.Core.Entities.Student", null)
                        .WithMany("Activities")
                        .HasForeignKey("StudentId");

                    b.HasOne("CoursesWebAPI.Core.Entities.Employee", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId");

                    b.Navigation("ActivityType");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.Contract", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Entities.Student", "Student")
                        .WithMany("Contracts")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Identity.RefreshToken", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Identity.User", null)
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Identity.Role", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Identity.User", null)
                        .WithMany("Roles")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Identity.User", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Entities.Person", "Person")
                        .WithOne()
                        .HasForeignKey("CoursesWebAPI.Core.Identity.User", "PersonId");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoursesWebAPI.Core.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.Employee", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Entities.Person", null)
                        .WithOne()
                        .HasForeignKey("CoursesWebAPI.Core.Entities.Employee", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.Student", b =>
                {
                    b.HasOne("CoursesWebAPI.Core.Entities.Person", null)
                        .WithOne()
                        .HasForeignKey("CoursesWebAPI.Core.Entities.Student", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Identity.User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.Employee", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("CoursesWebAPI.Core.Entities.Student", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("Contracts");
                });
#pragma warning restore 612, 618
        }
    }
}
