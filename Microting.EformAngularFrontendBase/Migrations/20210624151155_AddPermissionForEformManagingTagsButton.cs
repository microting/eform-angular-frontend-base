using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.EformAngularFrontendBase.Migrations
{
    public partial class AddPermissionForEformManagingTagsButton : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "ClaimName", "CreatedAt", "CreatedByUserId", "PermissionName", "PermissionTypeId", "UpdatedAt", "UpdatedByUserId", "Version", "WorkflowState" },
                values: new object[] { 52, "eform_allow_managing_eform_tags", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Allow managing eform tags", 9, null, 0, 0, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 52);
        }
    }
}
