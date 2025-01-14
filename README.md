SocialApp - .NET Core API
This is a Social Application API built using ASP.NET Core. It allows users to:

Register and authenticate using JWT tokens.
Create and manage posts that contain text and images.
Modify posts that they only created.
Comment on posts and view comments.
Secure access using JWT (JSON Web Tokens) for authentication.
Features
User Registration & Authentication: Users can register and authenticate using JWT tokens.
Post Management: Users can create, modify, and delete posts that they have created.
Commenting System: Users can comment on posts and view existing comments.
File Upload: Users can upload images as part of their posts.
Technologies Used
.NET 6+: The API is built using the latest version of .NET Core.
Entity Framework: For ORM-based database interactions.
JWT Authentication: Secures the API endpoints to allow access only for authenticated users.
SQL Server: Used as the database for storing user data, posts, and comments.
Setup & Installation
Prerequisites
Before you begin, ensure that you have the following installed:

.NET 6 or later
SQL Server (or any other relational database)
Visual Studio Code or any preferred code editor
Steps
Clone the repository

bash
Copy code
git clone https://github.com/mostafa-bashir/SocialAPI-.NET-.git
cd SocialAPI-.NET-
Create a Database

Create a database in SQL Server or use an existing one. Update the connection string in appsettings.json with the correct credentials for your database.

Example of connection string:

json
Copy code
"ConnectionStrings": {
  "SocialConnectionString": "Server=your-server;Database=SocialAppDB;User Id=your-user;Password=your-password;"
}
Install Dependencies

Run the following command to restore the project dependencies:

bash
Copy code
dotnet restore
Migrate the Database

After setting up the database connection, apply the migrations to set up the tables:

bash
Copy code
dotnet ef migrations add InitialMigration
dotnet ef database update
Run the Application

Once everything is set up, run the application:

bash
Copy code
dotnet run
The API will start running on https://localhost:5001 (or http://localhost:5000 for non-SSL).

API Endpoints
Authentication
POST /api/auth/register

Registers a new user.
Request Body:
json
Copy code
{
  "username": "string",
  "email": "string",
  "password": "string"
}
POST /api/auth/login

Authenticates a user and returns a JWT token.
Request Body:
json
Copy code
{
  "username": "string",
  "password": "string"
}
Post Management
POST /api/posts

Creates a new post.
Request Body:
json
Copy code
{
  "script": "string",
  "imageFiles": [file1, file2] // Array of image files
}
Headers: Authorization: Bearer {JWT_TOKEN}
GET /api/posts/{postId}

Fetches a specific post by its ID.
Headers: Authorization: Bearer {JWT_TOKEN}
GET /api/posts

Fetches all posts created by the authenticated user.
Headers: Authorization: Bearer {JWT_TOKEN}
PUT /api/posts/{postId}

Updates a specific post (only by the user who created the post).
Request Body:
json
Copy code
{
  "script": "updated script",
  "imageFiles": [updatedFile1, updatedFile2]
}
Headers: Authorization: Bearer {JWT_TOKEN}
DELETE /api/posts/{postId}

Deletes a specific post (only by the user who created the post).
Headers: Authorization: Bearer {JWT_TOKEN}
Commenting System
POST /api/posts/{postId}/comments

Adds a comment to a specific post.
Request Body:
json
Copy code
{
  "content": "This is a comment."
}
Headers: Authorization: Bearer {JWT_TOKEN}
GET /api/posts/{postId}/comments

Fetches all comments for a specific post.
Headers: Authorization: Bearer {JWT_TOKEN}
Authentication with JWT Token
All requests (except authentication and registration) require the Authorization header with a Bearer token.

Use the /api/auth/login endpoint to get a JWT token after registering a user.

Include the token in the Authorization header for subsequent requests:

bash
Copy code
Authorization: Bearer {your_jwt_token}
Directory Structure
plaintext
Copy code
/ SocialApp
│
├── /Controllers       # API controllers
│   ├── AuthController.cs
│   ├── PostController.cs
│   └── CommentController.cs
│
├── /Models            # Model classes
│   ├── User.cs
│   ├── Post.cs
│   └── Comment.cs
│
├── /Repositories      # Database interaction logic
│   ├── IImageRepository.cs
│   ├── ImageRepository.cs
│   └── IPostRepository.cs
│
├── /Services          # Business logic services
│   ├── AuthService.cs
│   └── PostService.cs
│
├── /Migrations        # Entity Framework migrations
│
├── /wwwroot           # Static files (e.g., images)
│
└── appsettings.json   # Configuration file
Troubleshooting
404 Error for Images: If you're getting a 404 error for the images, make sure your application is correctly configured to serve static files as described in the code.
JWT Issues: Ensure that the token is correctly included in the Authorization header for protected routes.
Contributing
Feel free to fork the repository and submit pull requests. If you encounter any issues or have suggestions for new features, please open an issue.

GitHub Repository
For the full source code and more details, visit the repository at:
https://github.com/mostafa-bashir/SocialAPI-.NET-

