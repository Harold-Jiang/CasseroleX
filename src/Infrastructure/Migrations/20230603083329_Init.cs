using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CasseroleX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "更新时间"),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, defaultValueSql: "''", comment: "用户名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NickName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "''", comment: "昵称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValueSql: "''", comment: "密码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Salt = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValueSql: "''", comment: "密码盐")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "''", comment: "头像")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "电子邮箱")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mobile = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true, defaultValueSql: "''", comment: "手机号码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoginFailure = table.Column<byte>(type: "tinyint unsigned", nullable: false, comment: "失败次数"),
                    LoginTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "登录时间"),
                    LoginIp = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "登录IP")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Token = table.Column<string>(type: "varchar(59)", maxLength: 59, nullable: true, defaultValueSql: "''", comment: "token标识")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", maxLength: 30, nullable: false, defaultValue: 0, comment: "状态"),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                },
                comment: "管理员表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sha1 = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true, defaultValueSql: "''", comment: "文件 sha1编码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Storage = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "'local'", comment: "存储位置")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UploadTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "上传时间"),
                    ExtParam = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "''", comment: "透传数据")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MimeType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "mime类型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileSize = table.Column<int>(type: "int", nullable: false, comment: "文件大小"),
                    FileName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "文件名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageFrames = table.Column<int>(type: "int", nullable: false, comment: "图片帧数"),
                    ImageType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, defaultValueSql: "''", comment: "图片类型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageHeight = table.Column<int>(type: "int", nullable: false, comment: "高度"),
                    ImageWidth = table.Column<int>(type: "int", nullable: false, comment: "宽度"),
                    Url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "''", comment: "物理路径")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false, comment: "用户ID"),
                    Category = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "''", comment: "类别")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2023, 6, 3, 16, 33, 28, 725, DateTimeKind.Local).AddTicks(7544), comment: "创建时间"),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "更新时间"),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Event = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, defaultValueSql: "''", comment: "事件")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "邮箱")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, defaultValueSql: "''", comment: "验证码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Times = table.Column<int>(type: "int", nullable: false, comment: "验证次数"),
                    Ip = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, defaultValueSql: "''", comment: "IP")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2023, 6, 3, 16, 33, 28, 730, DateTimeKind.Local).AddTicks(1935), comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ems", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(type: "enum('menu','file')", nullable: false, defaultValueSql: "'file'", comment: "menu为菜单,file为权限节点")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pid = table.Column<int>(type: "int", nullable: false, comment: "父ID"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "规则名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "''", comment: "规则名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "''", comment: "图标")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "''", comment: "规则URL")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Condition = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "''", comment: "条件")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remark = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "''", comment: "备注")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsMenu = table.Column<byte>(type: "tinyint unsigned", nullable: false, comment: "是否为菜单"),
                    MenuType = table.Column<string>(type: "enum('addtabs','blank','dialog','ajax')", nullable: true, comment: "菜单类型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Extend = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "''", comment: "扩展属性")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Py = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, defaultValueSql: "''", comment: "拼音首字母")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PinYin = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "拼音")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Weigh = table.Column<int>(type: "int", nullable: false, comment: "权重"),
                    Status = table.Column<int>(type: "int", maxLength: 30, nullable: false, defaultValue: 0, comment: "状态"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "更新时间"),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Pid = table.Column<int>(type: "int", nullable: false, comment: "父组别"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "组名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rules = table.Column<string>(type: "text", nullable: false, comment: "规则ID")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", maxLength: 30, nullable: false, defaultValue: 0, comment: "状态"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "更新时间"),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SiteConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Group = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tip = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Visible = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rule = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Extend = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteConfigurations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Event = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, defaultValueSql: "''", comment: "事件")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mobile = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, defaultValueSql: "''", comment: "手机号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, defaultValueSql: "''", comment: "验证码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Times = table.Column<int>(type: "int", nullable: false, comment: "验证次数"),
                    Ip = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, defaultValueSql: "''", comment: "IP")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2023, 6, 3, 16, 33, 28, 750, DateTimeKind.Local).AddTicks(2977), comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sms", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "''", comment: "组名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rules = table.Column<string>(type: "text", nullable: true, comment: "权限节点")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", maxLength: 30, nullable: false, defaultValue: 0, comment: "状态"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "更新时间"),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                },
                comment: "会员组表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Gender = table.Column<byte>(type: "tinyint unsigned", nullable: false, comment: "性别"),
                    RealName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BirthDay = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "生日"),
                    IdCard = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bio = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "格言")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Money = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, comment: "余额"),
                    Score = table.Column<int>(type: "int", nullable: false, comment: "积分"),
                    GroupId = table.Column<int>(type: "int", nullable: false, comment: "组别ID"),
                    Level = table.Column<int>(type: "int", nullable: false, comment: "等级"),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    JoinIp = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "''", comment: "加入IP")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JoinTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "加入时间"),
                    Successions = table.Column<int>(type: "int", nullable: false, defaultValueSql: "'1'", comment: "连续登录天数"),
                    MaxSuccessions = table.Column<int>(type: "int", nullable: false, defaultValueSql: "'1'", comment: "最大连续登录天数"),
                    PrevTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "上次登录时间"),
                    RegisterIp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MobileConfirm = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "手机号验证"),
                    EmailConfirm = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "邮箱验证"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "更新时间"),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, defaultValueSql: "''", comment: "用户名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NickName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "''", comment: "昵称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValueSql: "''", comment: "密码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Salt = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValueSql: "''", comment: "密码盐")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "''", comment: "头像")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "电子邮箱")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Mobile = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true, defaultValueSql: "''", comment: "手机号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoginFailure = table.Column<byte>(type: "tinyint unsigned", nullable: false, comment: "失败次数"),
                    LoginTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "登录时间"),
                    LoginIp = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "''", comment: "登录IP")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Token = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "''", comment: "Token")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", maxLength: 30, nullable: false, defaultValue: 0, comment: "状态"),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdminRoles",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false, comment: "会员ID"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "级别ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminRoles", x => new { x.AdminId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AdminRoles_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdminRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Avatar", "CreateTime", "CreatedBy", "Email", "LastModified", "LastModifiedBy", "LockoutEnabled", "LockoutEnd", "LoginFailure", "LoginIp", "LoginTime", "Mobile", "NickName", "PasswordHash", "Salt", "Token", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, "/assets/img/avatar.png", new DateTime(2023, 6, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "info@xxx.com", new DateTime(2023, 6, 3, 10, 0, 33, 0, DateTimeKind.Unspecified), 0, false, null, (byte)0, "::1", new DateTime(2023, 6, 3, 10, 0, 31, 0, DateTimeKind.Unspecified), "13800138000", "admin", "98692CEA72ED669009D68D0CF5B5054DA2520265", "123456", "0c2a2ca0b5b04cba996a0e3ad2144e60", false, "admin" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "Condition", "CreateTime", "CreatedBy", "Extend", "Icon", "IsMenu", "LastModified", "LastModifiedBy", "MenuType", "Name", "Pid", "PinYin", "Py", "Remark", "Title", "Type", "Url", "Weigh" },
                values: new object[,]
                {
                    { 1, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-dashboard", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "dashboard", 0, "kongzhitai", "kzt", "Dashboard tips", "Dashboard", "file", "", 143 },
                    { 2, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-cogs", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general", 0, "changguiguanli", "cggl", "", "General", "file", "", 137 },
                    { 3, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-leaf", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "category", 0, "fenleiguanli", "flgl", "Category tips", "Category", "file", "", 119 },
                    { 4, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-rocket", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "addon", 0, "chajianguanli", "cjgl", "Addon tips", "Addon", "file", "", 0 },
                    { 5, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-group", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth", 0, "quanxianguanli", "qxgl", "", "Auth", "file", "", 99 },
                    { 6, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-cog", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/config", 2, "xitongpeizhi", "xtpz", "Config tips", "Config", "file", "", 60 },
                    { 7, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-file-image-o", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/attachment", 2, "fujianguanli", "fjgl", "Attachment tips", "Attachment", "file", "", 53 },
                    { 8, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-user", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/profile", 2, "gerenziliao", "grzl", "", "Profile", "file", "", 34 },
                    { 9, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-user", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/admin", 5, "guanliyuanguanli", "glygl", "Admin tips", "Admin", "file", "", 118 },
                    { 10, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-list-alt", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/adminlog", 5, "guanliyuanrizhi", "glyrz", "Admin log tips", "Admin log", "file", "", 113 },
                    { 11, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-group", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/group", 5, "juesezu", "jsz", "Group tips", "Group", "file", "", 109 },
                    { 12, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-bars", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/rule", 5, "caidanguize", "cdgz", "Rule tips", "Rule", "file", "", 104 },
                    { 13, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "dashboard/index", 1, "", "", "", "View", "file", "", 136 },
                    { 14, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "dashboard/add", 1, "", "", "", "Add", "file", "", 135 },
                    { 15, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "dashboard/del", 1, "", "", "", "Delete", "file", "", 133 },
                    { 16, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "dashboard/edit", 1, "", "", "", "Edit", "file", "", 134 },
                    { 17, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "dashboard/multi", 1, "", "", "", "Multi", "file", "", 132 },
                    { 18, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/config/index", 6, "", "", "", "View", "file", "", 52 },
                    { 19, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/config/add", 6, "", "", "", "Add", "file", "", 51 },
                    { 20, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/config/edit", 6, "", "", "", "Edit", "file", "", 50 },
                    { 21, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/config/del", 6, "", "", "", "Delete", "file", "", 49 },
                    { 22, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/config/multi", 6, "", "", "", "Multi", "file", "", 48 },
                    { 23, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/attachment/index", 7, "", "", "Attachment tips", "View", "file", "", 59 },
                    { 24, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/attachment/select", 7, "", "", "", "Select attachment", "file", "", 58 },
                    { 25, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/attachment/add", 7, "", "", "", "Add", "file", "", 57 },
                    { 26, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/attachment/edit", 7, "", "", "", "Edit", "file", "", 56 },
                    { 27, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/attachment/del", 7, "", "", "", "Delete", "file", "", 55 },
                    { 28, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/attachment/multi", 7, "", "", "", "Multi", "file", "", 54 },
                    { 29, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/profile/index", 8, "", "", "", "View", "file", "", 33 },
                    { 30, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/profile/update", 8, "", "", "", "Update profile", "file", "", 32 },
                    { 31, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/profile/add", 8, "", "", "", "Add", "file", "", 31 },
                    { 32, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/profile/edit", 8, "", "", "", "Edit", "file", "", 30 },
                    { 33, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/profile/del", 8, "", "", "", "Delete", "file", "", 29 },
                    { 34, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "general/profile/multi", 8, "", "", "", "Multi", "file", "", 28 },
                    { 35, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "category/index", 3, "", "", "Category tips", "View", "file", "", 142 },
                    { 36, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "category/add", 3, "", "", "", "Add", "file", "", 141 },
                    { 37, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "category/edit", 3, "", "", "", "Edit", "file", "", 140 },
                    { 38, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "category/del", 3, "", "", "", "Delete", "file", "", 139 },
                    { 39, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "category/multi", 3, "", "", "", "Multi", "file", "", 138 },
                    { 40, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/admin/index", 9, "", "", "Admin tips", "View", "file", "", 117 },
                    { 41, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/admin/add", 9, "", "", "", "Add", "file", "", 116 },
                    { 42, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/admin/edit", 9, "", "", "", "Edit", "file", "", 115 },
                    { 43, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/admin/del", 9, "", "", "", "Delete", "file", "", 114 },
                    { 44, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/adminlog/index", 10, "", "", "Admin log tips", "View", "file", "", 112 },
                    { 45, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/adminlog/detail", 10, "", "", "", "Detail", "file", "", 111 },
                    { 46, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/adminlog/del", 10, "", "", "", "Delete", "file", "", 110 },
                    { 47, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/group/index", 11, "", "", "Group tips", "View", "file", "", 108 },
                    { 48, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/group/add", 11, "", "", "", "Add", "file", "", 107 },
                    { 49, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/group/edit", 11, "", "", "", "Edit", "file", "", 106 },
                    { 50, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/group/del", 11, "", "", "", "Delete", "file", "", 105 },
                    { 51, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/rule/index", 12, "", "", "Rule tips", "View", "file", "", 103 },
                    { 52, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/rule/add", 12, "", "", "", "Add", "file", "", 102 },
                    { 53, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/rule/edit", 12, "", "", "", "Edit", "file", "", 101 },
                    { 54, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "auth/rule/del", 12, "", "", "", "Delete", "file", "", 100 },
                    { 55, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "addon/index", 4, "", "", "Addon tips", "View", "file", "", 0 },
                    { 56, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "addon/add", 4, "", "", "", "Add", "file", "", 0 },
                    { 57, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "addon/edit", 4, "", "", "", "Edit", "file", "", 0 },
                    { 58, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "addon/del", 4, "", "", "", "Delete", "file", "", 0 },
                    { 59, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "addon/downloaded", 4, "", "", "", "Local addon", "file", "", 0 },
                    { 60, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "addon/state", 4, "", "", "", "Update state", "file", "", 0 },
                    { 63, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "addon/config", 4, "", "", "", "Setting", "file", "", 0 },
                    { 64, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "addon/refresh", 4, "", "", "", "Refresh", "file", "", 0 },
                    { 65, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "addon/multi", 4, "", "", "", "Multi", "file", "", 0 },
                    { 66, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-user-circle", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user", 0, "huiyuanguanli", "hygl", "", "User", "file", "", 0 },
                    { 67, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-user", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/user", 66, "huiyuanguanli", "hygl", "", "User", "file", "", 0 },
                    { 68, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/user/index", 67, "", "", "", "View", "file", "", 0 },
                    { 69, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/user/edit", 67, "", "", "", "Edit", "file", "", 0 },
                    { 70, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/user/add", 67, "", "", "", "Add", "file", "", 0 },
                    { 71, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/user/del", 67, "", "", "", "Del", "file", "", 0 },
                    { 72, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/user/multi", 67, "", "", "", "Multi", "file", "", 0 },
                    { 73, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-users", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/group", 66, "huiyuanfenzu", "hyfz", "", "User group", "file", "", 0 },
                    { 74, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/group/add", 73, "", "", "", "Add", "file", "", 0 },
                    { 75, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/group/edit", 73, "", "", "", "Edit", "file", "", 0 },
                    { 76, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/group/index", 73, "", "", "", "View", "file", "", 0 },
                    { 77, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/group/del", 73, "", "", "", "Del", "file", "", 0 },
                    { 78, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/group/multi", 73, "", "", "", "Multi", "file", "", 0 },
                    { 79, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/rule", 66, "huiyuanguize", "hygz", "", "User rule", "file", "", 0 },
                    { 80, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/rule/index", 79, "", "", "", "View", "file", "", 0 },
                    { 81, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/rule/del", 79, "", "", "", "Del", "file", "", 0 },
                    { 82, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/rule/add", 79, "", "", "", "Add", "file", "", 0 },
                    { 83, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/rule/edit", 79, "", "", "", "Edit", "file", "", 0 },
                    { 84, "", new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "", "fa fa-circle-o", (byte)0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, null, "user/rule/multi", 79, "", "", "", "Multi", "file", "", 0 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreateTime", "CreatedBy", "LastModified", "LastModifiedBy", "Name", "Pid", "Rules" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "Admin group", 0, "*" },
                    { 2, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "Second group", 1, "13,14,16,15,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,40,41,42,43,44,45,46,47,48,49,50,55,56,57,58,59,60,61,62,63,64,65,1,9,10,11,7,6,8,2,4,5" },
                    { 3, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "Third group", 2, "1,4,9,10,11,13,14,15,16,17,40,41,42,43,44,45,46,47,48,49,50,55,56,57,58,59,60,61,62,63,64,65,5" },
                    { 4, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "Second group 2", 1, "1,4,13,14,15,16,17,55,56,57,58,59,60,61,62,63,64,65" },
                    { 5, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, new DateTime(2022, 10, 1, 10, 1, 1, 0, DateTimeKind.Unspecified), 0, "Third group 2", 2, "1,2,6,7,8,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34" }
                });

            migrationBuilder.InsertData(
                table: "AdminRoles",
                columns: new[] { "AdminId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminRoles_AdminId",
                table: "AdminRoles",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminRoles_AdminId_RoleId",
                table: "AdminRoles",
                columns: new[] { "AdminId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminRoles_RoleId",
                table: "AdminRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserName",
                table: "Admins",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_Name",
                table: "RolePermissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_Pid",
                table: "RolePermissions",
                column: "Pid");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_Weigh",
                table: "RolePermissions",
                column: "Weigh");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Mobile",
                table: "Users",
                column: "Mobile");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminRoles");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "Ems");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "SiteConfigurations");

            migrationBuilder.DropTable(
                name: "Sms");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
