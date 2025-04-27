```
reLIFE.sln // The overall solution file
│
//======================================================================
├── reLIFE.BusinessLogic (Class Library Project)
// PURPOSE: Combines data access (using Microsoft.Data.SqlClient) and business logic/services.
//          Knows how to read configuration, talk to the database, apply business rules.
//======================================================================
│   ├── Data/
│   │   └── DbHelper.cs         // Reads connection string from appsettings.json.
│   │
│   ├── Repositories/
│   │   // PURPOSE: Classes responsible for direct SQL execution via Microsoft.Data.SqlClient.
│   │   //           Constructors take the connection string provided by the UI layer.
│   │   ├── CategoryRepository.cs   // CRUD for Categories (uses string connectionString constructor)
│   │   ├── EventRepository.cs      // CRUD for Events (Modified to exclude Archived in main get) (uses string connectionString constructor)
│   │   ├── UserRepository.cs       // CRUD for Users (uses string connectionString constructor)
│   │   ├── ArchivedEventRepository.cs // CRUD for Archived Events (move to/from, get) (uses string connectionString constructor)
│   │   └── ReminderRepository.cs   // CRUD for Reminders (get by event, add, update, delete, maybe get upcoming) (uses string connectionString constructor)
│   │
│   ├── Security/
│   │   └── PasswordHasher.cs     // Implements password hashing (SHA256 with salt).
│   │
│   ├── Services/
│   │   // PURPOSE: Orchestrates operations, performs validation, uses Repositories.
│   │   //           Constructors take Repository and other Service/Security dependencies.
│   │   ├── AuthService.cs        // Handles Login/Register logic (uses UserRepository, PasswordHasher)
│   │   ├── CategoryService.cs    // Handles category CRUD (uses CategoryRepository)
│   │   ├── EventService.cs       // Handles event CRUD and Archiving logic (uses EventRepository, ArchivedEventRepository, CategoryRepository, ReminderRepository)
│   │   └── ReminderService.cs    // Handles Reminder CRUD and checking logic (uses ReminderRepository, maybe EventRepository)
│   │
│   ├── Validators/
│   │   └── Validation.cs         // Static helper for validating basic text inputs (username, email, password format).
│   │
│   └── Properties/               // Standard project properties
│
//======================================================================
├── reLIFE.Core (Class Library Project)
// PURPOSE: Contains only basic data structures (models) and common enums used across projects.
//======================================================================
│   ├── Common/                   // Might still be empty or have basic enums/helpers if needed later
│   │   └── (Enums if added e.g. ViewType)
│   │
│   ├── Models/
│   │   // PURPOSE: Plain C# Objects mirroring database tables.
│   │   ├── User.cs             // (No change needed for schema)
│   │   ├── Category.cs         // (No change needed for schema)
│   │   ├── Event.cs            // (Original simplified Event schema, no recurrence columns here now)
│   │   ├── ArchivedEvent.cs    // Model for the ArchivedEvents table structure.
│   │   └── Reminder.cs         // Model for the Reminders table structure.
│   │
│   └── Properties/               // Standard project properties
│
//======================================================================
└── reLIFE.WinFormsUI (Windows Forms App Project)
// PURPOSE: The user interface. Forms interact directly with Services.
//======================================================================
    ├── Properties/               // Standard project properties (Settings, Resources, AssemblyInfo etc.)
    │
    ├── Resources/                // Application resources (icons, images)
    │
    ├── Assets/                   // Any other assets (custom fonts, etc.)
    │
    ├── Forms/
    │   // PURPOSE: Your application's windows and dialogs.
    │   //           Constructors take Service dependencies.
    │   ├── LoginForm.cs          // Login window. Interacts with AuthService. (Takes AuthService in constructor)
    │   ├── LoginForm.Designer.cs // Designer generated code
    │   ├── LoginForm.resx        // Resources for Login form
    │   │
    │   ├── MainForm.cs           // Main scheduler/event view. Interacts with EventService, CategoryService, ReminderService. (Takes Services and User in constructor)
    │   ├── MainForm.Designer.cs
    │   ├── MainForm.resx
    │   │
    │   ├── RegistrationForm.cs     // Registration window. Interacts with AuthService. (Takes AuthService in constructor)
    │   ├── RegistrationForm.Designer.cs
    │   ├── RegistrationForm.resx
    │   │
    │   └── EventForm.cs            // Add/Edit event dialog. Interacts with EventService, CategoryService, ReminderService. (Takes Services, maybe Event ID, maybe User in constructor)
    │   └── (ArchiveViewerForm.cs)  // Optional form to view/restore archived events (Interacts with EventService or dedicated ArchiveService?)
    │   └── (CategoryManagerForm.cs)// Optional form to manage categories (Interacts with CategoryService)
    │
    ├── appsettings.json          // Stores the database connection string and other configuration.
    │
    └── Program.cs                // Main entry point. Reads connection string using DbHelper, manually creates services/repositories, launches the initial form (LoginForm).
```
