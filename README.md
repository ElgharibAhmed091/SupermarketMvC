
# 🛒 Supermarket MVC Application

A professional ASP.NET Core MVC web application that simulates an online supermarket. It features a categorized product catalog (Meat, Fish, Vegetables, Electronics, etc.), cart functionality, order processing, user authentication, and secure Stripe payment integration.

---

## 👨‍💻 Developed By

**Elgharib Ahmed Blat**  
Backend Developer – ASP.NET Core MVC  

---

## 🚀 Features

- 🧾 **User Registration & Login** (ASP.NET Identity)
- 📦 **Product Management** (CRUD)
- 🗂️ **Product Categorization** (Meat, Fish, Groceries, etc.)
- 🛒 **Shopping Cart System**
- 💳 **Stripe Payment Integration** (Test Mode)
- 📃 **Order Tracking & History**
- 🔐 **Session Management**
- 📸 **Product Image Upload & Display**
- 🌐 **Responsive Design with Bootstrap**
- 🧠 **EF Core with Code First Migrations**
- 🧮 **Total Price Calculation**
- 🔄 **Role Management (Admin / User)**
- 🔒 **Secure Configuration using User Secrets**

---

## 🛠️ Technologies Used

| Tech                    | Description                             |
|-------------------------|-----------------------------------------|
| ASP.NET Core MVC        | Web Application Framework               |
| Entity Framework Core   | ORM for Database Access                 |
| SQL Server              | Relational Database                     |
| ASP.NET Identity        | Authentication and Role Management      |
| Stripe API              | Online Payments (test mode)             |
| Bootstrap 5             | Responsive Design Framework             |
| Git & GitHub            | Source Control                          |

---

## 📁 Project File Structure

```
SupermarketMvc/
│
├── Controllers/             # Logic (Product, Category, Order)
├── Models/                  # Entities (Product, Category, Order, etc.)
├── Views/                   # Razor Views
│   ├── Product/
│   ├── Order/
│   ├── Shared/
│
├── wwwroot/
│   ├── images/
│   ├── css/
│   └── js/
│
├── Data/                    # ApplicationDbContext and Migrations
├── Services/                # Cart & Order Services
├── appsettings.json
├── Program.cs
└── README.md
```

---

## 📦 Setup Instructions

```bash
git clone https://github.com/ElgharibAhmed091/SupermarketMvC.git
cd SupermarketMvC
dotnet restore
dotnet ef database update
dotnet run
```

Then open:

```
https://localhost:5001
```

---

## 🧪 Test Stripe Payment

- Card Number: `4242 4242 4242 4242`
- Expiry: Any future date (e.g., 12/34)
- CVC: Any 3 digits

> **Note:** Test keys are removed from source. Use `User Secrets` for local dev.

---

## 📌 To Do / Future Features

- Admin dashboard for managing all entities  
- Discount system  
- Multilingual (Arabic/English toggle)  
- Product reviews and ratings  
- Image optimization and caching

---

## 📄 License

This project is for educational/demo purposes only.

---

✅ **Star** the repo if you like it!  
💬 **Feel free to contribute or report issues.**
## 🔧 How to Clone the Project

To clone this project to your local machine, follow these steps:

```bash
git clone https://github.com/ElgharibAhmed091/SupermarketMvC.git
cd SupermarketMvC
