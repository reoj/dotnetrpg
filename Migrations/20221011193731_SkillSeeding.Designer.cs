// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dotnetrpg.Data;

#nullable disable

namespace dotnetrpg.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221011193731_SkillSeeding")]
    partial class SkillSeeding
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CharacterSkill", b =>
                {
                    b.Property<int>("CharactersId")
                        .HasColumnType("int");

                    b.Property<int>("SkillsId")
                        .HasColumnType("int");

                    b.HasKey("CharactersId", "SkillsId");

                    b.HasIndex("SkillsId");

                    b.ToTable("CharacterSkill");
                });

            modelBuilder.Entity("dotnetrpg.Models.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("HitPoints")
                        .HasColumnType("int");

                    b.Property<int>("Intelligence")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Strenght")
                        .HasColumnType("int");

                    b.Property<int>("rpgClass")
                        .HasColumnType("int");

                    b.Property<int?>("userOwnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("userOwnerId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("dotnetrpg.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Damage")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Skills");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Damage = 30,
                            Name = "Fireball"
                        },
                        new
                        {
                            Id = 2,
                            Damage = 40,
                            Name = "Water Magic"
                        },
                        new
                        {
                            Id = 3,
                            Damage = 20,
                            Name = "Disaming Blow"
                        });
                });

            modelBuilder.Entity("dotnetrpg.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("dotnetrpg.Models.Weapon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.Property<int>("Damage")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId")
                        .IsUnique();

                    b.ToTable("Weapons");
                });

            modelBuilder.Entity("CharacterSkill", b =>
                {
                    b.HasOne("dotnetrpg.Models.Character", null)
                        .WithMany()
                        .HasForeignKey("CharactersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dotnetrpg.Models.Skill", null)
                        .WithMany()
                        .HasForeignKey("SkillsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dotnetrpg.Models.Character", b =>
                {
                    b.HasOne("dotnetrpg.Models.User", "userOwner")
                        .WithMany("Characters")
                        .HasForeignKey("userOwnerId");

                    b.Navigation("userOwner");
                });

            modelBuilder.Entity("dotnetrpg.Models.Weapon", b =>
                {
                    b.HasOne("dotnetrpg.Models.Character", "Holder")
                        .WithOne("CurrentWeapon")
                        .HasForeignKey("dotnetrpg.Models.Weapon", "CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Holder");
                });

            modelBuilder.Entity("dotnetrpg.Models.Character", b =>
                {
                    b.Navigation("CurrentWeapon")
                        .IsRequired();
                });

            modelBuilder.Entity("dotnetrpg.Models.User", b =>
                {
                    b.Navigation("Characters");
                });
#pragma warning restore 612, 618
        }
    }
}
