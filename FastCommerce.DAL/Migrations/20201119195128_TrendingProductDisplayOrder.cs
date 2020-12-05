﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace FastCommerce.DAL.Migrations
{
    public partial class TrendingProductDisplayOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "TrendingProducts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "TrendingProducts");
        }
    }
}
