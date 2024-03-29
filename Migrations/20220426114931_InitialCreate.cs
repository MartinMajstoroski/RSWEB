﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProektRS.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    enrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    acquiredCredits = table.Column<int>(type: "int", nullable: true),
                    currentSemester = table.Column<int>(type: "int", nullable: true),
                    educationLevel = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    teacherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    degree = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    academicRank = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    officeNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    hireDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.teacherId);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    courseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    credits = table.Column<int>(type: "int", nullable: false),
                    semester = table.Column<int>(type: "int", nullable: false),
                    programme = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    educationLevel = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    firstTeacherId = table.Column<int>(type: "int", nullable: true),
                    secondTeacherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.courseId);
                    table.ForeignKey(
                        name: "FK_Course_Teacher_firstTeacherId",
                        column: x => x.firstTeacherId,
                        principalTable: "Teacher",
                        principalColumn: "teacherId");
                    table.ForeignKey(
                        name: "FK_Course_Teacher_secondTeacherId",
                        column: x => x.secondTeacherId,
                        principalTable: "Teacher",
                        principalColumn: "teacherId");
                });

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    courseId = table.Column<int>(type: "int", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    semester = table.Column<int>(type: "int", nullable: true),
                    year = table.Column<int>(type: "int", nullable: true),
                    grade = table.Column<int>(type: "int", nullable: true),
                    seminalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    projectUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    examPoints = table.Column<int>(type: "int", nullable: true),
                    seminalPoints = table.Column<int>(type: "int", nullable: true),
                    projectPoints = table.Column<int>(type: "int", nullable: true),
                    additionalPoints = table.Column<int>(type: "int", nullable: true),
                    finishDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Enrollment_Course_courseId",
                        column: x => x.courseId,
                        principalTable: "Course",
                        principalColumn: "courseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_firstTeacherId",
                table: "Course",
                column: "firstTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_secondTeacherId",
                table: "Course",
                column: "secondTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_courseId",
                table: "Enrollment",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_StudentID",
                table: "Enrollment",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Teacher");
        }
    }
}
