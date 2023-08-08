using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CasseroleX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConfigData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Sms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 8, 11, 18, 8, 287, DateTimeKind.Local).AddTicks(320),
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 19, DateTimeKind.Local).AddTicks(4358),
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Ems",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 8, 11, 18, 8, 284, DateTimeKind.Local).AddTicks(3141),
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 17, DateTimeKind.Local).AddTicks(6340),
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Attachments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 8, 11, 18, 8, 283, DateTimeKind.Local).AddTicks(7493),
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 17, DateTimeKind.Local).AddTicks(1466),
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "AdminLogs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 8, 11, 18, 8, 279, DateTimeKind.Local).AddTicks(3209),
                comment: "操作时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 12, DateTimeKind.Local).AddTicks(6787),
                oldComment: "操作时间");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Group",
                value: "System");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Group",
                value: "System");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Group",
                value: "System");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 4,
                column: "Group",
                value: "System");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 5,
                column: "Group",
                value: "System");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 6,
                column: "Group",
                value: "System");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 7,
                column: "Group",
                value: "System");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 8,
                column: "Group",
                value: "System");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 9,
                column: "Group",
                value: "System");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Group", "Value" },
                values: new object[] { "System", "{\"system\":\"System\",\"sms\":\"Sms\",\"email\":\"Email\",\"upload\":\"Upload\",\"account\":\"Account\",\"example\":\"Example\"}" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Sms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 19, DateTimeKind.Local).AddTicks(4358),
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 8, 11, 18, 8, 287, DateTimeKind.Local).AddTicks(320),
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Ems",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 17, DateTimeKind.Local).AddTicks(6340),
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 8, 11, 18, 8, 284, DateTimeKind.Local).AddTicks(3141),
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Attachments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 17, DateTimeKind.Local).AddTicks(1466),
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 8, 11, 18, 8, 283, DateTimeKind.Local).AddTicks(7493),
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "AdminLogs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 12, DateTimeKind.Local).AddTicks(6787),
                comment: "操作时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 8, 11, 18, 8, 279, DateTimeKind.Local).AddTicks(3209),
                oldComment: "操作时间");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Group",
                value: "Basic");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Group",
                value: "Basic");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Group",
                value: "Basic");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 4,
                column: "Group",
                value: "Basic");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 5,
                column: "Group",
                value: "Basic");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 6,
                column: "Group",
                value: "Basic");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 7,
                column: "Group",
                value: "Basic");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 8,
                column: "Group",
                value: "Basic");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 9,
                column: "Group",
                value: "Basic");

            migrationBuilder.UpdateData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Group", "Value" },
                values: new object[] { "Basic", "{\"basic\":\"Basic\",\"sms\":\"Sms\",\"email\":\"Email\",\"upload\":\"Upload\",\"account\":\"Account\",\"example\":\"Example\"}" });
        }
    }
}
