using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceDay.API.Migrations
{
    /// <inheritdoc />
    public partial class AlignPart1Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Events_EventID",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Categories_CategoryID",
                table: "Enrolments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Events_EventID",
                table: "Enrolments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Participants_ParticipantID",
                table: "Enrolments");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_OrganiserID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Users_UserID",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Enrolments_EnrolmentID",
                table: "Results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Results",
                table: "Results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrolments",
                table: "Enrolments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_EventID",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Results",
                newName: "Result");

            migrationBuilder.RenameTable(
                name: "Participants",
                newName: "Participant");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.RenameTable(
                name: "Enrolments",
                newName: "Enrolment");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "User",
                newName: "IX_User_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Results_EnrolmentID",
                table: "Result",
                newName: "IX_Result_EnrolmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Participants_UserID",
                table: "Participant",
                newName: "IX_Participant_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganiserID",
                table: "Event",
                newName: "IX_Event_OrganiserID");

            migrationBuilder.RenameIndex(
                name: "IX_Enrolments_ParticipantID_EventID",
                table: "Enrolment",
                newName: "IX_Enrolment_ParticipantID_EventID");

            migrationBuilder.RenameIndex(
                name: "IX_Enrolments_EventID",
                table: "Enrolment",
                newName: "IX_Enrolment_EventID");

            migrationBuilder.RenameIndex(
                name: "IX_Enrolments_CategoryID",
                table: "Enrolment",
                newName: "IX_Enrolment_CategoryID");

            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "User",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Event",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "EventName",
                table: "Event",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDate",
                table: "Event",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "Distance",
                table: "Event",
                type: "decimal(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Event",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Enrolment",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnrolmentDate",
                table: "Enrolment",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSDATETIME()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "Category",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Result",
                table: "Result",
                column: "ResultID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participant",
                table: "Participant",
                column: "ParticipantID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "EventID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrolment",
                table: "Enrolment",
                column: "EnrolmentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "CategoryID");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Role",
                table: "User",
                sql: "[Role] IN ('Organiser', 'Participant')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Result_Position",
                table: "Result",
                sql: "[Position] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Event_EventType",
                table: "Event",
                sql: "[EventType] IN ('Run', 'Walk', 'Cycle')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Enrolment_Status",
                table: "Enrolment",
                sql: "[Status] IN ('Pending', 'Confirmed', 'Cancelled')");

            migrationBuilder.CreateIndex(
                name: "IX_Category_EventID_CategoryName",
                table: "Category",
                columns: new[] { "EventID", "CategoryName" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Event_EventID",
                table: "Category",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolment_Category_CategoryID",
                table: "Enrolment",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolment_Event_EventID",
                table: "Enrolment",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolment_Participant_ParticipantID",
                table: "Enrolment",
                column: "ParticipantID",
                principalTable: "Participant",
                principalColumn: "ParticipantID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_User_OrganiserID",
                table: "Event",
                column: "OrganiserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_User_UserID",
                table: "Participant",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Result_Enrolment_EnrolmentID",
                table: "Result",
                column: "EnrolmentID",
                principalTable: "Enrolment",
                principalColumn: "EnrolmentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Event_EventID",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrolment_Category_CategoryID",
                table: "Enrolment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrolment_Event_EventID",
                table: "Enrolment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrolment_Participant_ParticipantID",
                table: "Enrolment");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_User_OrganiserID",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_User_UserID",
                table: "Participant");

            migrationBuilder.DropForeignKey(
                name: "FK_Result_Enrolment_EnrolmentID",
                table: "Result");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Role",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Result",
                table: "Result");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Result_Position",
                table: "Result");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participant",
                table: "Participant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Event_EventType",
                table: "Event");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrolment",
                table: "Enrolment");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Enrolment_Status",
                table: "Enrolment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_EventID_CategoryName",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Result",
                newName: "Results");

            migrationBuilder.RenameTable(
                name: "Participant",
                newName: "Participants");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.RenameTable(
                name: "Enrolment",
                newName: "Enrolments");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Result_EnrolmentID",
                table: "Results",
                newName: "IX_Results_EnrolmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Participant_UserID",
                table: "Participants",
                newName: "IX_Participants_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Event_OrganiserID",
                table: "Events",
                newName: "IX_Events_OrganiserID");

            migrationBuilder.RenameIndex(
                name: "IX_Enrolment_ParticipantID_EventID",
                table: "Enrolments",
                newName: "IX_Enrolments_ParticipantID_EventID");

            migrationBuilder.RenameIndex(
                name: "IX_Enrolment_EventID",
                table: "Enrolments",
                newName: "IX_Enrolments_EventID");

            migrationBuilder.RenameIndex(
                name: "IX_Enrolment_CategoryID",
                table: "Enrolments",
                newName: "IX_Enrolments_CategoryID");

            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Events",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "EventName",
                table: "Events",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDate",
                table: "Events",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<decimal>(
                name: "Distance",
                table: "Events",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,2)",
                oldPrecision: 6,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Enrolments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnrolmentDate",
                table: "Enrolments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "SYSDATETIME()");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Results",
                table: "Results",
                column: "ResultID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                column: "ParticipantID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "EventID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrolments",
                table: "Enrolments",
                column: "EnrolmentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_EventID",
                table: "Categories",
                column: "EventID");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Events_EventID",
                table: "Categories",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Categories_CategoryID",
                table: "Enrolments",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Events_EventID",
                table: "Enrolments",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Participants_ParticipantID",
                table: "Enrolments",
                column: "ParticipantID",
                principalTable: "Participants",
                principalColumn: "ParticipantID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_OrganiserID",
                table: "Events",
                column: "OrganiserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Users_UserID",
                table: "Participants",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Enrolments_EnrolmentID",
                table: "Results",
                column: "EnrolmentID",
                principalTable: "Enrolments",
                principalColumn: "EnrolmentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
