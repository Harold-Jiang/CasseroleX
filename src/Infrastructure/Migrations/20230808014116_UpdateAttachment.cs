using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CasseroleX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachment",
                table: "Attachment");

            migrationBuilder.RenameTable(
                name: "Attachment",
                newName: "Attachments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Sms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 19, DateTimeKind.Local).AddTicks(4358),
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 3, 14, 22, 52, 456, DateTimeKind.Local).AddTicks(4389),
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
                oldDefaultValue: new DateTime(2023, 8, 3, 14, 22, 52, 454, DateTimeKind.Local).AddTicks(5708),
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
                oldDefaultValue: new DateTime(2023, 8, 3, 14, 22, 52, 449, DateTimeKind.Local).AddTicks(4090),
                oldComment: "操作时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Attachments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 17, DateTimeKind.Local).AddTicks(1466),
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 3, 14, 22, 52, 454, DateTimeKind.Local).AddTicks(650),
                oldComment: "创建时间");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments");

            migrationBuilder.RenameTable(
                name: "Attachments",
                newName: "Attachment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Sms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 3, 14, 22, 52, 456, DateTimeKind.Local).AddTicks(4389),
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
                defaultValue: new DateTime(2023, 8, 3, 14, 22, 52, 454, DateTimeKind.Local).AddTicks(5708),
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 17, DateTimeKind.Local).AddTicks(6340),
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "AdminLogs",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 3, 14, 22, 52, 449, DateTimeKind.Local).AddTicks(4090),
                comment: "操作时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 12, DateTimeKind.Local).AddTicks(6787),
                oldComment: "操作时间");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Attachment",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 3, 14, 22, 52, 454, DateTimeKind.Local).AddTicks(650),
                comment: "创建时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 8, 8, 9, 41, 16, 17, DateTimeKind.Local).AddTicks(1466),
                oldComment: "创建时间");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachment",
                table: "Attachment",
                column: "Id");
        }
    }
}
