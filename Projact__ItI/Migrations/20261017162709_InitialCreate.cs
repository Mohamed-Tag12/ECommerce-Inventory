using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Projact__ItI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    City = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Supplies = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_Purchases_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchases_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK_Sales_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Laptops, monitors, headphones, tablets and smart gadgets", "Electronics" },
                    { 2, "Novels, textbooks and programming books", "Books" },
                    { 3, "Everyday clothing for men and women", "Clothes" },
                    { 4, "Washing machines, microwaves, refrigerators and kitchen appliances", "Home Appliances" },
                    { 5, "Balls, rackets and sports equipment", "Sports" },
                    { 6, "Toys, puzzles and collectibles", "Toys" },
                    { 7, "Makeup and personal care products", "Beauty and Personal Care" },
                    { 8, "Car batteries, tires and accessories", "Automotive" },
                    { 9, "Protein powders and nutrition supplements", "Health and Wellness" },
                    { 10, "Coffee, chocolate and groceries", "Food and Beverages" },
                    { 11, "Latest smartphones in all price ranges", "Mobile Phones" },
                    { 12, "Elegant evening dresses and gowns", "Evening Wear" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "Description", "ImagePath", "Price", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, 1, "13-inch ultra-thin Dell laptop with vivid display", "/images/Electronics/2.jpeg", 24900m, 12, "Dell Inspiron 13 Laptop" },
                    { 2, 1, "14-inch business laptop with slim aluminium body", "/images/Electronics/4.jpeg", 32000m, 7, "Dell Latitude 14 Laptop" },
                    { 3, 1, "15.6-inch Dell laptop for office and study", "/images/Electronics/10.jpeg", 21500m, 15, "Dell Vostro 15 Laptop" },
                    { 4, 1, "Reliable 15.6-inch everyday Dell laptop", "/images/Electronics/13.jpeg", 19900m, 18, "Dell Inspiron 15 Laptop" },
                    { 5, 1, "Premium 15-inch Dell laptop with sharp display", "/images/Electronics/17.jpeg", 36500m, 9, "Dell Latitude 15 Business Laptop" },
                    { 6, 1, "Lightweight grey laptop for everyday use", "/images/Machine/9.jpeg", 17500m, 14, "Slim 15.6-inch Laptop" },
                    { 7, 1, "Full HD 24-inch monitor with thin bezels", "/images/Machine/11.jpeg", 4800m, 20, "24-inch LED Monitor" },
                    { 8, 1, "Black Bluetooth headphones with deep bass", "/images/Electronics/5.jpeg", 1200m, 35, "Wireless Over-Ear Headphones" },
                    { 9, 1, "Comfortable foldable on-ear headphones", "/images/Electronics/8.jpeg", 650m, 40, "Wired On-Ear Headphones" },
                    { 10, 1, "Wireless navy headphones with soft ear cushions", "/images/Electronics/11.jpeg", 950m, 28, "Bluetooth Headphones - Navy" },
                    { 11, 1, "Fitness smartwatch with bright colour display", "/images/Electronics/15.jpeg", 2200m, 25, "Smartwatch" },
                    { 12, 1, "Android tablet with a large HD screen", "/images/Electronics/16.jpeg", 5500m, 16, "10-inch Android Tablet" },
                    { 13, 11, "Slim smartphone with dual camera in sky blue", "/images/Electronics/1.jpeg", 12500m, 22, "Smartphone - Sky Blue" },
                    { 14, 11, "256GB smartphone with triple camera in pearl white", "/images/Electronics/3.jpeg", 14500m, 18, "Smartphone - Pearl White 256GB" },
                    { 15, 11, "Curved-display smartphone in aqua green", "/images/Electronics/6.jpeg", 11900m, 24, "Smartphone - Aqua Green" },
                    { 16, 11, "Flagship-class phone with triple camera in midnight black", "/images/Electronics/7.jpeg", 18500m, 10, "Smartphone Pro - Midnight Black" },
                    { 17, 11, "Affordable 128GB smartphone with large screen", "/images/Electronics/9.jpeg", 5200m, 45, "Budget Smartphone - Navy 128GB" },
                    { 18, 11, "Premium phone with curved display and silver finish", "/images/Electronics/12.jpeg", 21000m, 5, "Flagship Smartphone - Silver Wave" },
                    { 19, 11, "5G smartphone with dual camera", "/images/Electronics/14.jpeg", 9800m, 26, "Smartphone 5G - Black" },
                    { 20, 11, "Smartphone with a vibrant ocean blue back panel", "/images/Electronics/18.jpeg", 10500m, 21, "Smartphone - Ocean Blue" },
                    { 21, 11, "Stylish lilac pink phone with triple camera", "/images/Electronics/19.jpeg", 13900m, 17, "Smartphone - Lilac Pink" },
                    { 22, 2, "Lower secondary science student book", "/images/Book/1.jpeg", 420m, 40, "Oxford Science Student Book 7" },
                    { 23, 2, "Science textbook for secondary school", "/images/Book/2.jpeg", 280m, 35, "Science Textbook - Class X" },
                    { 24, 2, "Hardcover novel with a tropical cover design", "/images/Book/3.jpeg", 390m, 16, "Akin - Hardcover Novel" },
                    { 25, 2, "Complete guide to programming with C#", "/images/Book/4.jpeg", 650m, 30, "Fundamentals of Computer Programming with C#" },
                    { 26, 2, "Arabic novel about long nights and old memories", "/images/Book/5.jpeg", 180m, 28, "Layl Taweel - Arabic Novel" },
                    { 27, 2, "Science textbook for class 10 students", "/images/Book/6.jpeg", 310m, 32, "NCERT Science Class 10" },
                    { 28, 2, "Dark fantasy novel with a gothic cover", "/images/Book/7.jpeg", 340m, 19, "An Arcane Inheritance - Fantasy Novel" },
                    { 29, 2, "Stack of 10 science and engineering textbooks", "/images/Book/8.jpeg", 4500m, 6, "University Textbooks Bundle (10 Books)" },
                    { 30, 2, "Award-winning post-apocalyptic novel", "/images/Book/9.jpeg", 360m, 14, "Station Eleven - Hardcover" },
                    { 31, 2, "Young adult fantasy novel", "/images/Book/10.jpeg", 330m, 22, "Born of Illusion - Young Adult Novel" },
                    { 32, 3, "Red tee, beige tee and blue sweatshirt set", "/images/Dress/2.jpeg", 1350m, 20, "Cotton Tees and Sweatshirt Pack" },
                    { 33, 3, "Straight-fit classic denim jeans", "/images/Dress/6.jpeg", 850m, 30, "Classic Blue Jeans" },
                    { 34, 3, "Soft crew-neck white cotton t-shirt", "/images/Dress/12.jpeg", 220m, 60, "White Cotton T-Shirt" },
                    { 35, 12, "Elegant black mermaid gown with a side slit", "/images/Dress/1.jpeg", 3800m, 8, "Black Mermaid Evening Gown" },
                    { 36, 12, "Off-shoulder black gown with lace details", "/images/Dress/3.jpeg", 4200m, 6, "Black Off-Shoulder Lace Gown" },
                    { 37, 12, "Fitted black dress with sheer lace sleeves", "/images/Dress/4.jpeg", 4500m, 9, "Black Lace-Sleeve Evening Dress" },
                    { 38, 12, "Sparkling champagne sequin gown for special nights", "/images/Dress/5.jpeg", 7500m, 4, "Champagne Sequin Evening Gown" },
                    { 39, 12, "Off-shoulder gown with a draped train", "/images/Dress/7.jpeg", 4100m, 11, "Black Draped Off-Shoulder Gown" },
                    { 40, 12, "Navy gown with beaded bodice and tulle skirt", "/images/Dress/9.jpeg", 6200m, 0, "Navy Beaded Evening Gown" },
                    { 41, 12, "Classic one-shoulder black evening gown", "/images/Dress/10.jpeg", 3600m, 10, "Black One-Shoulder Gown" },
                    { 42, 12, "Black dress with beaded neckline and puff shoulders", "/images/Dress/11.jpeg", 3300m, 12, "Black Beaded Puff-Shoulder Dress" },
                    { 43, 12, "One-shoulder black gown with crystal belt", "/images/Dress/13.jpeg", 2900m, 13, "Black Belted One-Shoulder Gown" },
                    { 44, 12, "Simple off-shoulder black maxi dress", "/images/Dress/14.jpeg", 2400m, 15, "Black Off-Shoulder Maxi Dress" },
                    { 45, 4, "LG inverter front load washer in steel grey", "/images/Machine/1.jpeg", 17500m, 4, "LG Front Load Washing Machine" },
                    { 46, 4, "Silver microwave with digital controls", "/images/Machine/2.jpeg", 4200m, 14, "Silver Microwave Oven" },
                    { 47, 4, "Drip coffee maker with glass carafe", "/images/Machine/3.jpeg", 1900m, 25, "Faber Drip Coffee Maker" },
                    { 48, 4, "10kg front load washing machine in white", "/images/Machine/4.jpeg", 14500m, 9, "White Front Load Washer 10kg" },
                    { 49, 4, "Samsung microwave with stainless steel handle", "/images/Machine/5.jpeg", 4800m, 6, "Samsung Microwave Oven" },
                    { 50, 4, "Stainless steel top-freezer refrigerator", "/images/Machine/6.jpeg", 19500m, 4, "Silver Double-Door Refrigerator" },
                    { 51, 4, "8kg front load washer with energy label", "/images/Machine/7.jpeg", 11900m, 11, "White Washing Machine 8kg" },
                    { 52, 4, "Whirlpool front load washer with white finish", "/images/Machine/8.jpeg", 16800m, 3, "Whirlpool Front Load Washer" },
                    { 53, 4, "Black Samsung microwave with rotary dial", "/images/Machine/10.jpeg", 4500m, 13, "Samsung Microwave Oven - Black" },
                    { 54, 4, "Combined washer and dryer in white and silver", "/images/Machine/12.jpeg", 15900m, 8, "Washer Dryer Combo - White" },
                    { 55, 4, "11kg silver washer dryer with digital display", "/images/Machine/13.jpeg", 18900m, 5, "Silver Washer Dryer 11kg" },
                    { 56, 4, "Whirlpool washer in graphite grey", "/images/Machine/14.jpeg", 17900m, 7, "Whirlpool Graphite Washer" },
                    { 57, 5, "Indoor/outdoor Nike Elite basketball", "/images/Tennis Racket/1.jpeg", 750m, 38, "Nike Elite Basketball" },
                    { 58, 5, "Official NBA game ball in genuine leather", "/images/Tennis Racket/3.jpeg", 2400m, 2, "Wilson NBA Official Game Basketball" },
                    { 59, 5, "Colourful Nivia training football", "/images/Tennis Racket/4.jpeg", 450m, 45, "Nivia Football" },
                    { 60, 5, "Black Wilson tennis racket for advanced players", "/images/Tennis Racket/5.jpeg", 3800m, 10, "Wilson Pro Tennis Racket - Black" },
                    { 61, 5, "Black and white Head Speed racket", "/images/Tennis Racket/6.jpeg", 4200m, 8, "Head Speed Tennis Racket" },
                    { 62, 5, "Orange Head Radical racket with a light frame", "/images/Tennis Racket/7.jpeg", 3900m, 12, "Head Radical Tennis Racket - Orange" },
                    { 63, 5, "Blue Wilson racket for club players", "/images/Tennis Racket/8.jpeg", 2600m, 17, "Wilson Blue Tennis Racket" },
                    { 64, 6, "Marvel Iron Man collectible action figure", "/images/Tennis Racket/2.jpeg", 450m, 27, "Iron Man Action Figure" },
                    { 65, 6, "Jigsaw puzzle inspired by Starry Night", "/images/Puzzle/1.jpeg", 320m, 30, "Starry Night 1000-Piece Puzzle" },
                    { 66, 6, "Artistic songbird illustration jigsaw puzzle", "/images/Puzzle/2.jpeg", 340m, 22, "Songbird Art 1000-Piece Puzzle" },
                    { 67, 6, "Alpine lake scenery 1000-piece jigsaw puzzle", "/images/Puzzle/3.jpeg", 300m, 26, "Mountain Lake 1000-Piece Puzzle" },
                    { 68, 6, "Amsterdam cityscape illustrated puzzle", "/images/Puzzle/4.jpeg", 380m, 18, "Amsterdam 1000-Piece Puzzle" },
                    { 69, 6, "Landscape art puzzle in a white gift box", "/images/Puzzle/5.jpeg", 360m, 15, "Scott Leggo 1000-Piece Puzzle" },
                    { 70, 7, "Rich red lipstick in a black case", "/images/Dress/8.jpeg", 180m, 55, "Red Matte Lipstick" },
                    { 71, 7, "Gentle shampoo for normal hair, 355 ml", "/images/Protein Powder/2.jpeg", 150m, 48, "Women's Shampoo for Normal Hair" },
                    { 72, 8, "Sealed maintenance-free automotive battery", "/images/Car Tires/1.jpeg", 3800m, 10, "Western Endurance Car Battery" },
                    { 73, 8, "Pair of durable all-season car tires", "/images/Car Tires/2.jpeg", 9500m, 6, "Reliant All-Season Tires (Pair)" },
                    { 74, 8, "Sporty performance tire with alloy wheel", "/images/Car Tires/3.jpeg", 4200m, 20, "Performance Car Tire" },
                    { 75, 8, "Two all-season tires for safe driving", "/images/Car Tires/4.jpeg", 8800m, 9, "All-Season Tires - Set of 2" },
                    { 76, 9, "Whey protein powder with chocolate flavour", "/images/Protein Powder/1.jpeg", 1650m, 24, "FitSupport Whey Protein - Chocolate" },
                    { 77, 9, "Premium whey with 23g protein per serving", "/images/Protein Powder/3.jpeg", 1450m, 18, "Urmax 100% Whey - Chocolate" },
                    { 78, 9, "Level 2 whey protein, vanilla, 60 servings", "/images/Protein Powder/4.jpeg", 1900m, 14, "Pack Nutrition Whey Protein - Vanilla" },
                    { 79, 9, "Gold Standard 100% whey protein", "/images/Protein Powder/5.jpeg", 2600m, 9, "Optimum Nutrition Gold Standard Whey" },
                    { 80, 9, "Whey protein with 22g protein per serving", "/images/Protein Powder/6.jpeg", 2200m, 20, "Optimum Nutrition Whey - Chocolate" },
                    { 81, 9, "2kg whey protein tub with shaker", "/images/Protein Powder/7.jpeg", 2400m, 12, "CNP Whey Protein 2kg" },
                    { 82, 9, "AllMax classic 100% whey protein blend", "/images/Protein Powder/8.jpeg", 2100m, 16, "AllMax Classic AllWhey" },
                    { 83, 9, "Whey protein with vanilla flavour, 29 servings", "/images/Protein Powder/9.jpeg", 2200m, 22, "Optimum Nutrition Whey - Vanilla" },
                    { 84, 10, "Medium roast ground coffee, house blend", "/images/Coffee/1.jpeg", 420m, 40, "Starbucks House Blend Coffee" },
                    { 85, 10, "Dark chocolate bar with 45% cacao", "/images/Coffee/2.jpeg", 65m, 120, "Dark Chocolate Bar" },
                    { 86, 10, "100% Arabica ground coffee in kraft bag", "/images/Coffee/3.jpeg", 280m, 50, "Ground Coffee 12oz" },
                    { 87, 10, "Specialty blend coffee in a white pouch", "/images/Coffee/4.jpeg", 360m, 30, "Serra Blend Coffee" },
                    { 88, 10, "Ground house blend coffee, 12oz", "/images/Coffee/5.jpeg", 340m, 34, "Gevalia House Blend Coffee" },
                    { 89, 10, "Medium roast ground coffee, 340g", "/images/Coffee/6.jpeg", 290m, 46, "House Blend Medium Roast Coffee" },
                    { 90, 10, "Organic ground coffee with smooth aroma", "/images/Coffee/8.jpeg", 310m, 28, "Organic Ground Coffee 250g" },
                    { 91, 10, "Tasting notes of mocha and vanilla", "/images/Coffee/9.jpeg", 330m, 24, "House Blend Coffee - Black Bag" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ProductId",
                table: "Purchases",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_SupplierId",
                table: "Purchases",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductId",
                table: "Sales",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
