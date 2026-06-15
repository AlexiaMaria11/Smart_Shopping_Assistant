using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess;

public static class DataSeeder
{
    public static async Task SeedAsync(SmartShoppingAssistantDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Categories.AnyAsync())
            return;

        // ── Categories ────────────────────────────────────────────────────────
        var electronics = new Category { Name = "Electronics", Description = "Gadgets and electronic devices" };
        var clothing = new Category { Name = "Clothing", Description = "Apparel and fashion items" };
        var food = new Category { Name = "Food & Beverages", Description = "Groceries and drinks" };
        var sports = new Category { Name = "Sports", Description = "Sports equipment and accessories" };
        var beauty = new Category { Name = "Beauty & Personal Care", Description = "Skincare, haircare and grooming" };
        var home = new Category { Name = "Home & Garden", Description = "Furniture, décor and garden tools" };
        var books = new Category { Name = "Books & Stationery", Description = "Books, notebooks and office supplies" };
        var toys = new Category { Name = "Toys & Games", Description = "Games and toys for all ages" };
        var automotive = new Category { Name = "Automotive", Description = "Car accessories and maintenance" };
        var health = new Category { Name = "Health & Wellness", Description = "Supplements, medical devices and wellness products" };

        await context.Categories.AddRangeAsync(
            electronics, clothing, food, sports,
            beauty, home, books, toys, automotive, health
        );
        await context.SaveChangesAsync();

        // ── Products ──────────────────────────────────────────────────────────
        var products = new List<Product>
        {
            // Electronics
            new()
            {
                Name        = "Wireless Headphones",
                Description = "Noise-cancelling over-ear headphones with 30h battery life",
                ImageUrl    = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400&h=400&fit=crop",
                Price       = 149.99m,
                Categories  = [electronics]
            },
            new()
            {
                Name        = "Smartphone Stand",
                Description = "Adjustable aluminum stand for phones and tablets",
                ImageUrl    = "https://images.unsplash.com/photo-1586816879360-004f5b0c51e5?w=400&h=400&fit=crop",
                Price       = 24.99m,
                Categories  = [electronics]
            },
            new()
            {
                Name        = "Bluetooth Speaker",
                Description = "Portable waterproof speaker with 360° sound",
                ImageUrl    = "https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?w=400&h=400&fit=crop",
                Price       = 79.99m,
                Categories  = [electronics]
            },
            new()
            {
                Name        = "4K Webcam",
                Description = "Ultra-HD webcam with built-in ring light and noise-cancelling mic",
                ImageUrl    = "https://images.unsplash.com/photo-1623949556303-5f6f5a8d4b3a?w=400&h=400&fit=crop",
                Price       = 119.99m,
                Categories  = [electronics]
            },
            new()
            {
                Name        = "Mechanical Keyboard",
                Description = "TKL mechanical keyboard with RGB backlight, red switches",
                ImageUrl    = "https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=400&h=400&fit=crop",
                Price       = 89.99m,
                Categories  = [electronics]
            },
            new()
            {
                Name        = "Wireless Mouse",
                Description = "Ergonomic wireless mouse with silent clicks, 18-month battery",
                ImageUrl    = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=400&h=400&fit=crop",
                Price       = 39.99m,
                Categories  = [electronics]
            },
            new()
            {
                Name        = "USB-C Hub 7-in-1",
                Description = "Hub with HDMI, 3× USB-A, SD card, USB-C PD and Ethernet",
                ImageUrl    = "https://images.unsplash.com/photo-1625948515291-69613efd103f?w=400&h=400&fit=crop",
                Price       = 49.99m,
                Categories  = [electronics]
            },
            new()
            {
                Name        = "Smart LED Bulb",
                Description = "16M colour Wi-Fi bulb, works with Alexa and Google Home",
                ImageUrl    = "https://images.unsplash.com/photo-1565636192335-2417c8c9a999?w=400&h=400&fit=crop",
                Price       = 14.99m,
                Categories  = [electronics, home]
            },
            new()
            {
                Name        = "Action Camera",
                Description = "4K 60fps waterproof action cam with image stabilisation",
                ImageUrl    = "https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?w=400&h=400&fit=crop",
                Price       = 199.99m,
                Categories  = [electronics, sports]
            },
            new()
            {
                Name        = "Portable Power Bank 20000mAh",
                Description = "Fast-charge power bank with dual USB-A and USB-C output",
                ImageUrl    = "https://images.unsplash.com/photo-1609091839311-d5365f9ff1c5?w=400&h=400&fit=crop",
                Price       = 44.99m,
                Categories  = [electronics]
            },

            // Clothing
            new()
            {
                Name        = "Running T-Shirt",
                Description = "Lightweight moisture-wicking running shirt",
                ImageUrl    = "https://images.unsplash.com/photo-1556906781-9a412961c28c?w=400&h=400&fit=crop",
                Price       = 29.99m,
                Categories  = [clothing, sports]
            },
            new()
            {
                Name        = "Yoga Pants",
                Description = "High-waist stretch yoga pants for comfort and performance",
                ImageUrl    = "https://images.unsplash.com/photo-1506629082955-511b1aa562c8?w=400&h=400&fit=crop",
                Price       = 49.99m,
                Categories  = [clothing, sports]
            },
            new()
            {
                Name        = "Winter Puffer Jacket",
                Description = "Lightweight down-fill jacket, water resistant, packable",
                ImageUrl    = "https://images.unsplash.com/photo-1551489186-cf8726f514f8?w=400&h=400&fit=crop",
                Price       = 129.99m,
                Categories  = [clothing]
            },
            new()
            {
                Name        = "Classic Denim Jeans",
                Description = "Straight-fit stretch denim, available in multiple washes",
                ImageUrl    = "https://images.unsplash.com/photo-1542272604-787c3835535d?w=400&h=400&fit=crop",
                Price       = 59.99m,
                Categories  = [clothing]
            },
            new()
            {
                Name        = "Cotton Hoodie",
                Description = "Premium 380gsm cotton fleece hoodie with kangaroo pocket",
                ImageUrl    = "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=400&h=400&fit=crop",
                Price       = 54.99m,
                Categories  = [clothing]
            },
            new()
            {
                Name        = "Compression Socks (3-pack)",
                Description = "Graduated compression socks for sport and travel",
                ImageUrl    = "https://images.unsplash.com/photo-1586350977771-b3b0abd50c82?w=400&h=400&fit=crop",
                Price       = 19.99m,
                Categories  = [clothing, sports, health]
            },

            // Food & Beverages
            new()
            {
                Name        = "Protein Bar (Box of 12)",
                Description = "High-protein snack bars, chocolate flavour",
                ImageUrl    = "https://images.unsplash.com/photo-1622484212385-aa5f1f8e1cc7?w=400&h=400&fit=crop",
                Price       = 19.99m,
                Categories  = [food, sports]
            },
            new()
            {
                Name        = "Green Tea (100 bags)",
                Description = "Organic green tea bags",
                ImageUrl    = "https://images.unsplash.com/photo-1627435601361-ec25f5b1d0e5?w=400&h=400&fit=crop",
                Price       = 9.99m,
                Categories  = [food]
            },
            new()
            {
                Name        = "Cold-Brew Coffee Concentrate 500ml",
                Description = "Ready-to-dilute cold brew, dark roast single origin",
                ImageUrl    = "https://images.unsplash.com/photo-1517701604599-bb29b565090c?w=400&h=400&fit=crop",
                Price       = 12.99m,
                Categories  = [food]
            },
            new()
            {
                Name        = "Whey Protein Powder 1kg",
                Description = "Vanilla flavour whey isolate, 25g protein per serving",
                ImageUrl    = "https://images.unsplash.com/photo-1579722820930-4f4e36b06414?w=400&h=400&fit=crop",
                Price       = 49.99m,
                Categories  = [food, sports, health]
            },
            new()
            {
                Name        = "Mixed Nuts & Dried Fruit 500g",
                Description = "Premium blend of cashews, almonds, cranberries and apricots",
                ImageUrl    = "https://images.unsplash.com/photo-1599599810769-bcde5a160d32?w=400&h=400&fit=crop",
                Price       = 14.99m,
                Categories  = [food]
            },
            new()
            {
                Name        = "Kombucha 6-Pack",
                Description = "Raw fermented kombucha, ginger-lemon flavour, 330ml cans",
                ImageUrl    = "https://images.unsplash.com/photo-1623065422902-30a2d299bbe4?w=400&h=400&fit=crop",
                Price       = 17.99m,
                Categories  = [food, health]
            },

            // Sports
            new()
            {
                Name        = "Resistance Bands Set",
                Description = "Set of 5 resistance bands for home workouts",
                ImageUrl    = "https://images.unsplash.com/photo-1598289431512-b97b0917affc?w=400&h=400&fit=crop",
                Price       = 34.99m,
                Categories  = [sports]
            },
            new()
            {
                Name        = "Foam Roller",
                Description = "High-density EVA foam roller for muscle recovery, 60cm",
                ImageUrl    = "https://images.unsplash.com/photo-1620188467120-5042ed1eb5da?w=400&h=400&fit=crop",
                Price       = 24.99m,
                Categories  = [sports, health]
            },
            new()
            {
                Name        = "Adjustable Dumbbell 20kg",
                Description = "Space-saving adjustable dumbbell, 2–20kg in 2kg increments",
                ImageUrl    = "https://images.unsplash.com/photo-1638536532686-d610adfc8e5c?w=400&h=400&fit=crop",
                Price       = 149.99m,
                Categories  = [sports]
            },
            new()
            {
                Name        = "Jump Rope – Speed Cable",
                Description = "Bearing-equipped speed jump rope, adjustable cable",
                ImageUrl    = "https://images.unsplash.com/photo-1517344884509-a0c97ec11bcc?w=400&h=400&fit=crop",
                Price       = 18.99m,
                Categories  = [sports]
            },
            new()
            {
                Name        = "Yoga Mat 6mm",
                Description = "Non-slip TPE yoga mat with alignment lines, carry strap",
                ImageUrl    = "https://images.unsplash.com/photo-1601925260368-ae2f83cf8b7f?w=400&h=400&fit=crop",
                Price       = 39.99m,
                Categories  = [sports]
            },
            new()
            {
                Name        = "Running Water Bottle 750ml",
                Description = "Insulated stainless steel bottle with flip straw lid",
                ImageUrl    = "https://images.unsplash.com/photo-1602143407151-7111542de6e8?w=400&h=400&fit=crop",
                Price       = 22.99m,
                Categories  = [sports]
            },

            // Beauty & Personal Care
            new()
            {
                Name        = "Vitamin C Serum 30ml",
                Description = "15% L-Ascorbic Acid brightening serum with hyaluronic acid",
                ImageUrl    = "https://images.unsplash.com/photo-1620916566398-39f1143ab7be?w=400&h=400&fit=crop",
                Price       = 34.99m,
                Categories  = [beauty, health]
            },
            new()
            {
                Name        = "Electric Toothbrush",
                Description = "Sonic toothbrush with 3 modes, 4-week battery, 2 brush heads",
                ImageUrl    = "https://images.unsplash.com/photo-1559591935-c6c92c6d3047?w=400&h=400&fit=crop",
                Price       = 44.99m,
                Categories  = [beauty, health]
            },
            new()
            {
                Name        = "SPF 50+ Sunscreen 100ml",
                Description = "Lightweight, non-greasy mineral sunscreen, reef safe",
                ImageUrl    = "https://images.unsplash.com/photo-1556228852-6d35a585d566?w=400&h=400&fit=crop",
                Price       = 16.99m,
                Categories  = [beauty]
            },
            new()
            {
                Name        = "Hair Diffuser Attachment",
                Description = "Universal hair dryer diffuser for defined curls, reduces frizz",
                ImageUrl    = "https://images.unsplash.com/photo-1522338242992-e1a54906a8da?w=400&h=400&fit=crop",
                Price       = 12.99m,
                Categories  = [beauty]
            },
            new()
            {
                Name        = "Natural Deodorant Stick",
                Description = "Aluminium-free, baking-soda-free natural deodorant, 72h protection",
                ImageUrl    = "https://images.unsplash.com/photo-1631730486572-226d1f595b68?w=400&h=400&fit=crop",
                Price       = 11.99m,
                Categories  = [beauty]
            },

            // Home & Garden
            new()
            {
                Name        = "Bamboo Cutting Board Set",
                Description = "3-piece bamboo cutting board set, juice groove, non-slip feet",
                ImageUrl    = "https://images.unsplash.com/photo-1594385208974-2e75f8d7bb18?w=400&h=400&fit=crop",
                Price       = 29.99m,
                Categories  = [home]
            },
            new()
            {
                Name        = "Scented Soy Candle",
                Description = "100% soy wax candle, 50h burn time, cedarwood & vanilla scent",
                ImageUrl    = "https://images.unsplash.com/photo-1602874801006-e26c4c5b5e93?w=400&h=400&fit=crop",
                Price       = 19.99m,
                Categories  = [home]
            },
            new()
            {
                Name        = "Stainless Steel French Press 1L",
                Description = "Double-wall insulated French press, keeps coffee hot 2 hours",
                ImageUrl    = "https://images.unsplash.com/photo-1544787219-7f47ccb76574?w=400&h=400&fit=crop",
                Price       = 34.99m,
                Categories  = [home, food]
            },
            new()
            {
                Name        = "Air Purifier – HEPA H13",
                Description = "True HEPA H13 air purifier, covers up to 40m², whisper-quiet",
                ImageUrl    = "https://images.unsplash.com/photo-1631083214070-9f6ec38a0440?w=400&h=400&fit=crop",
                Price       = 149.99m,
                Categories  = [home, health]
            },
            new()
            {
                Name        = "Indoor Plant Pot Set (3 pcs)",
                Description = "Minimalist ceramic pots with drainage holes, 10/14/18cm",
                ImageUrl    = "https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400&h=400&fit=crop",
                Price       = 24.99m,
                Categories  = [home]
            },

            // Books & Stationery
            new()
            {
                Name        = "Dot-Grid Notebook A5",
                Description = "160-page dot-grid notebook, 120gsm paper, lay-flat binding",
                ImageUrl    = "https://images.unsplash.com/photo-1531346878377-a5be20888e57?w=400&h=400&fit=crop",
                Price       = 12.99m,
                Categories  = [books]
            },
            new()
            {
                Name        = "Fountain Pen Starter Set",
                Description = "Stainless steel nib fountain pen with 10 ink cartridges",
                ImageUrl    = "https://images.unsplash.com/photo-1583485088034-697b5bc54ccc?w=400&h=400&fit=crop",
                Price       = 22.99m,
                Categories  = [books]
            },
            new()
            {
                Name        = "Self-Development Book Bundle (3 books)",
                Description = "Curated set: Atomic Habits, Deep Work, and The Power of Now",
                ImageUrl    = "https://images.unsplash.com/photo-1512820790803-83ca734da794?w=400&h=400&fit=crop",
                Price       = 49.99m,
                Categories  = [books]
            },
            new()
            {
                Name        = "Sticky Notes Assorted Pack",
                Description = "400 sticky notes in 4 sizes and 6 neon colours",
                ImageUrl    = "https://images.unsplash.com/photo-1586282391129-76a6df230234?w=400&h=400&fit=crop",
                Price       = 7.99m,
                Categories  = [books]
            },

            // Toys & Games
            new()
            {
                Name        = "Strategy Board Game",
                Description = "Award-winning strategy board game for 2–4 players, age 10+",
                ImageUrl    = "https://images.unsplash.com/photo-1610890716171-6b1bb98ffd09?w=400&h=400&fit=crop",
                Price       = 44.99m,
                Categories  = [toys]
            },
            new()
            {
                Name        = "1000-Piece Jigsaw Puzzle",
                Description = "High-quality 1000-piece puzzle, nature landscape theme",
                ImageUrl    = "https://images.unsplash.com/photo-1606503825008-99e8dd6b6c14?w=400&h=400&fit=crop",
                Price       = 19.99m,
                Categories  = [toys]
            },
            new()
            {
                Name        = "STEM Building Kit",
                Description = "250-piece magnetic STEM building kit for children 6+",
                ImageUrl    = "https://images.unsplash.com/photo-1587654780291-39c9404d746b?w=400&h=400&fit=crop",
                Price       = 34.99m,
                Categories  = [toys]
            },
            new()
            {
                Name        = "Playing Cards – Premium Set",
                Description = "2-deck set of casino-quality plastic-coated playing cards",
                ImageUrl    = "https://images.unsplash.com/photo-1529480114316-1f9c1d0c4f7e?w=400&h=400&fit=crop",
                Price       = 9.99m,
                Categories  = [toys]
            },

            // Automotive
            new()
            {
                Name        = "Dash Camera 2K",
                Description = "Front & rear 2K dash cam with night vision and loop recording",
                ImageUrl    = "https://images.unsplash.com/photo-1617814076367-b759c7d7e738?w=400&h=400&fit=crop",
                Price       = 99.99m,
                Categories  = [automotive, electronics]
            },
            new()
            {
                Name        = "Car Phone Mount",
                Description = "Magnetic dashboard phone mount, 360° rotation, universal fit",
                ImageUrl    = "https://images.unsplash.com/photo-1597762470488-3877b1f1cd55?w=400&h=400&fit=crop",
                Price       = 16.99m,
                Categories  = [automotive]
            },
            new()
            {
                Name        = "Tyre Inflator – Portable",
                Description = "Cordless electric tyre inflator with digital pressure gauge",
                ImageUrl    = "https://images.unsplash.com/photo-1632823469850-1b7b1e8b7f8a?w=400&h=400&fit=crop",
                Price       = 54.99m,
                Categories  = [automotive]
            },

            // Health & Wellness
            new()
            {
                Name        = "Multivitamin Daily (60 tablets)",
                Description = "Complete A–Z multivitamin and mineral complex, 2-month supply",
                ImageUrl    = "https://images.unsplash.com/photo-1550572017-edd951b55104?w=400&h=400&fit=crop",
                Price       = 14.99m,
                Categories  = [health]
            },
            new()
            {
                Name        = "Omega-3 Fish Oil 1000mg (90 caps)",
                Description = "High-strength EPA & DHA fish oil capsules",
                ImageUrl    = "https://images.unsplash.com/photo-1607619056574-7b8d3ee536b2?w=400&h=400&fit=crop",
                Price       = 19.99m,
                Categories  = [health]
            },
            new()
            {
                Name        = "Smart Body Scale",
                Description = "Wi-Fi body composition scale: weight, BMI, body fat, muscle mass",
                ImageUrl    = "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?w=400&h=400&fit=crop",
                Price       = 49.99m,
                Categories  = [health, electronics]
            },
            new()
            {
                Name        = "Acupressure Mat & Pillow Set",
                Description = "Linen acupressure mat with 6210 pressure points, neck pillow included",
                ImageUrl    = "https://images.unsplash.com/photo-1591343395082-e120087004b4?w=400&h=400&fit=crop",
                Price       = 39.99m,
                Categories  = [health, sports]
            },
            new()
            {
                Name        = "Digital Thermometer",
                Description = "Fast-read clinical thermometer, oral/axillary/rectal, 10s reading",
                ImageUrl    = "https://images.unsplash.com/photo-1584017911766-d451b3d0e843?w=400&h=400&fit=crop",
                Price       = 12.99m,
                Categories  = [health]
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // Helper index — match order above
        // [0]=Headphones [1]=Stand [2]=Speaker [3]=Webcam [4]=Keyboard [5]=Mouse
        // [6]=USB Hub [7]=Smart Bulb [8]=Action Cam [9]=Power Bank
        // [10]=Running T-Shirt [11]=Yoga Pants [12]=Winter Jacket [13]=Jeans [14]=Hoodie [15]=Compression Socks
        // [16]=Protein Bar [17]=Green Tea [18]=Cold Brew [19]=Whey Protein [20]=Nuts Mix [21]=Kombucha
        // [22]=Resistance Bands [23]=Foam Roller [24]=Adjustable Dumbbell [25]=Jump Rope [26]=Yoga Mat [27]=Water Bottle
        // [28]=Vitamin C Serum [29]=Electric Toothbrush [30]=Sunscreen [31]=Hair Diffuser [32]=Deodorant
        // [33]=Cutting Board [34]=Candle [35]=French Press [36]=Air Purifier [37]=Plant Pots
        // [38]=Notebook [39]=Fountain Pen [40]=Book Bundle [41]=Sticky Notes
        // [42]=Board Game [43]=Puzzle [44]=STEM Kit [45]=Playing Cards
        // [46]=Dash Cam [47]=Car Mount [48]=Tyre Inflator
        // [49]=Multivitamin [50]=Omega-3 [51]=Smart Scale [52]=Acupressure Mat [53]=Thermometer

        // ── Promotions ────────────────────────────────────────────────────────
        var promotions = new List<Promotion>
        {
            // ── Original promotions (preserved) ──────────────────────────────
            new()
            {
                Name        = "10% off Electronics over 100 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 100m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 10,
                CategoryId  = electronics.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "Buy 3 Sports items, get 1 free",
                Type        = PromotionType.Quantity,
                Threshold   = 3m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                CategoryId  = sports.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "5% off cart over 200 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 200m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 5,
                IsActive    = true
            },
            new()
            {
                Name        = "Free Protein Bar when buying 2",
                Type        = PromotionType.Quantity,
                Threshold   = 2m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                ProductId   = products[16].Id,
                IsActive    = false
            },

            // ── New promotions ────────────────────────────────────────────────
            new()
            {
                Name        = "15% off Clothing over 150 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 150m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 15,
                CategoryId  = clothing.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "Buy 2 Notebooks, get 1 Sticky Notes free",
                Type        = PromotionType.Quantity,
                Threshold   = 2m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                ProductId   = products[38].Id,
                IsActive    = true
            },
            new()
            {
                Name        = "10% off cart over 300 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 300m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 10,
                IsActive    = true
            },
            new()
            {
                Name        = "Buy 4 Health items, get 1 free",
                Type        = PromotionType.Quantity,
                Threshold   = 4m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                CategoryId  = health.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "20% off Beauty over 80 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 80m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 20,
                CategoryId  = beauty.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "Buy 2 Whey Proteins, get 1 free",
                Type        = PromotionType.Quantity,
                Threshold   = 2m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                ProductId   = products[19].Id,
                IsActive    = true
            },
            new()
            {
                Name        = "5% off Home & Garden over 120 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 120m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 5,
                CategoryId  = home.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "Buy 3 Toys, get 1 free",
                Type        = PromotionType.Quantity,
                Threshold   = 3m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                CategoryId  = toys.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "10% off Automotive over 100 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 100m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 10,
                CategoryId  = automotive.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "Free Green Tea when buying 3",
                Type        = PromotionType.Quantity,
                Threshold   = 3m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                ProductId   = products[17].Id,
                IsActive    = false
            },
            new()
            {
                Name        = "8% off Books over 60 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 60m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 8,
                CategoryId  = books.Id,
                IsActive    = true
            },
            new()
            {
                Name        = "12% off cart over 500 RON",
                Type        = PromotionType.CartTotal,
                Threshold   = 500m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 12,
                IsActive    = true
            },
            new()
            {
                Name        = "Buy 2 Smart Bulbs, get 1 free",
                Type        = PromotionType.Quantity,
                Threshold   = 2m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                ProductId   = products[7].Id,
                IsActive    = true
            },
            new()
            {
                Name        = "Free Foam Roller when buying 2 Sports items",
                Type        = PromotionType.Quantity,
                Threshold   = 2m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                ProductId   = products[23].Id,
                IsActive    = false
            },
            new()
            {
                Name        = "25% off Electronics over 400 RON – Flash Sale",
                Type        = PromotionType.CartTotal,
                Threshold   = 400m,
                Reward      = PromotionReward.PercentDiscount,
                RewardValue = 25,
                CategoryId  = electronics.Id,
                IsActive    = false
            },
            new()
            {
                Name        = "Buy 5 Food items, get 1 free",
                Type        = PromotionType.Quantity,
                Threshold   = 5m,
                Reward      = PromotionReward.FreeItems,
                RewardValue = 1,
                CategoryId  = food.Id,
                IsActive    = true
            }
        };

        await context.Promotions.AddRangeAsync(promotions);
        await context.SaveChangesAsync();
    }
}