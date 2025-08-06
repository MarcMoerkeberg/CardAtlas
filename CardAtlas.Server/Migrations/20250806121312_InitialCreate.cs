using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CardAtlas.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FrameLayouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrameLayouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GamePlatforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PrintCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Legalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrintFinishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintFinishes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rarities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rarities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScryfallId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artists_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFormats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameFormats_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ReminderText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Keywords_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PromoTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromoTypes_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScryfallId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    MtgoCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    ArenaCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    ParentSetCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    Block = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BlockCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    SetTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberOfCardsInSet = table.Column<int>(type: "int", nullable: false),
                    IsDigitalOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsFoilOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsNonFoilOnly = table.Column<bool>(type: "bit", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_SetTypes_SetTypeId",
                        column: x => x.SetTypeId,
                        principalTable: "SetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sets_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScryfallId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    OracleText = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    TypeLine = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    FlavorText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ManaCost = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConvertedManaCost = table.Column<decimal>(type: "decimal(8,1)", precision: 8, scale: 1, nullable: true),
                    Power = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Toughness = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Loyalty = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    CollectorNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsOnReservedList = table.Column<bool>(type: "bit", nullable: false),
                    CanBeFoundInBoosters = table.Column<bool>(type: "bit", nullable: false),
                    IsDigitalOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsFullArt = table.Column<bool>(type: "bit", nullable: false),
                    IsOversized = table.Column<bool>(type: "bit", nullable: false),
                    IsPromo = table.Column<bool>(type: "bit", nullable: false),
                    IsReprint = table.Column<bool>(type: "bit", nullable: false),
                    IsTextless = table.Column<bool>(type: "bit", nullable: false),
                    IsWotcOfficial = table.Column<bool>(type: "bit", nullable: false),
                    SetId = table.Column<int>(type: "int", nullable: false),
                    RarityId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    ColorIdentity = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    FrameLayoutId = table.Column<int>(type: "int", nullable: false),
                    ParentCardId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Cards_ParentCardId",
                        column: x => x.ParentCardId,
                        principalTable: "Cards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cards_FrameLayouts_FrameLayoutId",
                        column: x => x.FrameLayoutId,
                        principalTable: "FrameLayouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Rarities_RarityId",
                        column: x => x.RarityId,
                        principalTable: "Rarities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardArtists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<long>(type: "bigint", nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardArtists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardArtists_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardArtists_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardGamePlatforms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<long>(type: "bigint", nullable: false),
                    GamePlatformId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardGamePlatforms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardGamePlatforms_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardGamePlatforms_GamePlatforms_GamePlatformId",
                        column: x => x.GamePlatformId,
                        principalTable: "GamePlatforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageTypeId = table.Column<int>(type: "int", nullable: false),
                    ImageFormatId = table.Column<int>(type: "int", nullable: false),
                    ImageStatusId = table.Column<int>(type: "int", nullable: false),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<long>(type: "bigint", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardImages_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardImages_ImageFormats_ImageFormatId",
                        column: x => x.ImageFormatId,
                        principalTable: "ImageFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardImages_ImageStatuses_ImageStatusId",
                        column: x => x.ImageStatusId,
                        principalTable: "ImageStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardImages_ImageTypes_ImageTypeId",
                        column: x => x.ImageTypeId,
                        principalTable: "ImageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardImages_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardKeywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<long>(type: "bigint", nullable: false),
                    KeywordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardKeywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardKeywords_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardKeywords_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardLegalities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<long>(type: "bigint", nullable: false),
                    GameFormatId = table.Column<int>(type: "int", nullable: false),
                    LegalityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardLegalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardLegalities_GameFormats_GameFormatId",
                        column: x => x.GameFormatId,
                        principalTable: "GameFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardLegalities_Legalities_LegalityId",
                        column: x => x.LegalityId,
                        principalTable: "Legalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardPrices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    FoilPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    PurchaseUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardPrices_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardPrices_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardPrices_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardPrintFinishes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<long>(type: "bigint", nullable: false),
                    PrintFinishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPrintFinishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardPrintFinishes_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardPrintFinishes_PrintFinishes_PrintFinishId",
                        column: x => x.PrintFinishId,
                        principalTable: "PrintFinishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardPromoTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<long>(type: "bigint", nullable: false),
                    PromoTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPromoTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardPromoTypes_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardPromoTypes_PromoTypes_PromoTypeId",
                        column: x => x.PromoTypeId,
                        principalTable: "PromoTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "120a6a6d-9ba8-4291-8852-2c886425d0d1", null, "Admin", "ADMIN" },
                    { "1f3e819c-0f30-4b36-9533-64486b282b88", null, "User", "USER" },
                    { "95007ffa-933b-44bd-8312-12322e26fcd4", null, "Moderator", "MODERATOR" },
                    { "a9eb88a6-590b-4863-8c47-eacc8e76fc91", null, "Guest", "GUEST" }
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Usd" },
                    { 2, "Eur" },
                    { 3, "Tix" }
                });

            migrationBuilder.InsertData(
                table: "FrameLayouts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Year1993" },
                    { 2, "Year1997" },
                    { 3, "Year2003" },
                    { 4, "Year2015" },
                    { 5, "Future" }
                });

            migrationBuilder.InsertData(
                table: "GamePlatforms",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Paper" },
                    { 2, "Arena" },
                    { 3, "Mtgo" }
                });

            migrationBuilder.InsertData(
                table: "ImageFormats",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Png" },
                    { 2, "Jpg" }
                });

            migrationBuilder.InsertData(
                table: "ImageStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Placeholder" },
                    { 2, "LowResolution" },
                    { 3, "HighResolutionScan" }
                });

            migrationBuilder.InsertData(
                table: "ImageTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Png" },
                    { 2, "BorderCrop" },
                    { 3, "ArtCrop" },
                    { 4, "Large" },
                    { 5, "Normal" },
                    { 6, "Small" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "Name", "PrintCode" },
                values: new object[,]
                {
                    { -1, "NA", "NotImplemented", null },
                    { 1, "en", "English", "en" },
                    { 2, "es", "Spanish", "sp" },
                    { 3, "fr", "French", "fr" },
                    { 4, "de", "German", "de" },
                    { 5, "it", "Italian", "it" },
                    { 6, "pt", "Portuguese", "pt" },
                    { 7, "ja", "Japanese", "jp" },
                    { 8, "ko", "Korean", "kr" },
                    { 9, "ru", "Russian", "ru" },
                    { 10, "zhs", "SimplifiedChinese", "cs" },
                    { 11, "zht", "TraditionalChinese", "ct" },
                    { 12, "he", "Hebrew", null },
                    { 13, "la", "Latin", null },
                    { 14, "grc", "AncientGreek", null },
                    { 15, "ar", "Arabic", null },
                    { 16, "sa", "Sanskrit", null },
                    { 17, "ph", "Phyrexian", "ph" }
                });

            migrationBuilder.InsertData(
                table: "Legalities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Legal" },
                    { 2, "NotLegal" },
                    { 3, "Restricted" },
                    { 4, "Banned" }
                });

            migrationBuilder.InsertData(
                table: "PrintFinishes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Foil" },
                    { 2, "NonFoil" },
                    { 3, "Etched" }
                });

            migrationBuilder.InsertData(
                table: "Rarities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Common" },
                    { 2, "Uncommon" },
                    { 3, "Rare" },
                    { 4, "Special" },
                    { 5, "Mythic" },
                    { 6, "Bonus" }
                });

            migrationBuilder.InsertData(
                table: "SetTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Core" },
                    { 2, "Expansion" },
                    { 3, "Masters" },
                    { 4, "Alchemy" },
                    { 5, "Masterpiece" },
                    { 6, "Arsenal" },
                    { 7, "FromTheVault" },
                    { 8, "Spellbook" },
                    { 9, "PremiumDeck" },
                    { 10, "DuelDeck" },
                    { 11, "DraftInnovation" },
                    { 12, "TreasureChest" },
                    { 13, "Commander" },
                    { 14, "Planechase" },
                    { 15, "Archenemy" },
                    { 16, "Vanguard" },
                    { 17, "Funny" },
                    { 18, "Starter" },
                    { 19, "Box" },
                    { 20, "Promo" },
                    { 21, "Token" },
                    { 22, "Memorabilia" },
                    { 23, "MiniGame" }
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "Scryfall" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -1, "NotImplemented" },
                    { 1, "TcgPlayer" },
                    { 2, "CardMarket" },
                    { 3, "CardHoarder" },
                    { 4, "Mtgo" }
                });

            migrationBuilder.InsertData(
                table: "Sets",
                columns: new[] { "Id", "ArenaCode", "Block", "BlockCode", "Code", "IsDigitalOnly", "IsFoilOnly", "IsNonFoilOnly", "MtgoCode", "Name", "NumberOfCardsInSet", "ParentSetCode", "ReleaseDate", "ScryfallId", "SetTypeId", "SourceId" },
                values: new object[] { -1, null, null, null, "Default", false, false, false, null, "Unknown - Default set", 0, null, null, null, -1, -1 });

            migrationBuilder.CreateIndex(
                name: "IX_Artists_SourceId",
                table: "Artists",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CardArtists_ArtistId",
                table: "CardArtists",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_CardArtists_CardId",
                table: "CardArtists",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardGamePlatforms_CardId",
                table: "CardGamePlatforms",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardGamePlatforms_GamePlatformId",
                table: "CardGamePlatforms",
                column: "GamePlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_CardImages_CardId",
                table: "CardImages",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardImages_ImageFormatId",
                table: "CardImages",
                column: "ImageFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_CardImages_ImageStatusId",
                table: "CardImages",
                column: "ImageStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CardImages_ImageTypeId",
                table: "CardImages",
                column: "ImageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CardImages_SourceId",
                table: "CardImages",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_CardKeywords_CardId",
                table: "CardKeywords",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardKeywords_KeywordId",
                table: "CardKeywords",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_CardId",
                table: "CardLegalities",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_GameFormatId",
                table: "CardLegalities",
                column: "GameFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_CardLegalities_LegalityId",
                table: "CardLegalities",
                column: "LegalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPrices_CardId",
                table: "CardPrices",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPrices_CurrencyId",
                table: "CardPrices",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPrices_VendorId",
                table: "CardPrices",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPrintFinishes_CardId",
                table: "CardPrintFinishes",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPrintFinishes_PrintFinishId",
                table: "CardPrintFinishes",
                column: "PrintFinishId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPromoTypes_CardId",
                table: "CardPromoTypes",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardPromoTypes_PromoTypeId",
                table: "CardPromoTypes",
                column: "PromoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_FrameLayoutId",
                table: "Cards",
                column: "FrameLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_LanguageId",
                table: "Cards",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ParentCardId",
                table: "Cards",
                column: "ParentCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_RarityId",
                table: "Cards",
                column: "RarityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_SetId",
                table: "Cards",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_GameFormats_SourceId",
                table: "GameFormats",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_SourceId",
                table: "Keywords",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_PromoTypes_SourceId",
                table: "PromoTypes",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_SetTypeId",
                table: "Sets",
                column: "SetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_SourceId",
                table: "Sets",
                column: "SourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CardArtists");

            migrationBuilder.DropTable(
                name: "CardGamePlatforms");

            migrationBuilder.DropTable(
                name: "CardImages");

            migrationBuilder.DropTable(
                name: "CardKeywords");

            migrationBuilder.DropTable(
                name: "CardLegalities");

            migrationBuilder.DropTable(
                name: "CardPrices");

            migrationBuilder.DropTable(
                name: "CardPrintFinishes");

            migrationBuilder.DropTable(
                name: "CardPromoTypes");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "GamePlatforms");

            migrationBuilder.DropTable(
                name: "ImageFormats");

            migrationBuilder.DropTable(
                name: "ImageStatuses");

            migrationBuilder.DropTable(
                name: "ImageTypes");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "GameFormats");

            migrationBuilder.DropTable(
                name: "Legalities");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "PrintFinishes");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "PromoTypes");

            migrationBuilder.DropTable(
                name: "FrameLayouts");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Rarities");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "SetTypes");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
