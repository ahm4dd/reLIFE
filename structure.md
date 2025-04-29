```
reLIFE.sln // The overall solution file
│
//======================================================================
├── reLIFE.BusinessLogic (Class Library Project)
// PURPOSE: Combines data access and business logic.
//======================================================================
│   ├── Data/
│   │   └── DbHelper.cs         // Reads connection string.
│   │
│   ├── Repositories/
│   │   ├── ArchivedEventRepository.cs
│   │   ├── CategoryRepository.cs
│   │   ├── EventRepository.cs
│   │   ├── ReminderRepository.cs
│   │   └── UserRepository.cs       // Methods: AddUser, GetUserByUsername, GetUserByEmail, GetUserById, UpdateUserEmail, UpdatePassword
│   │
│   ├── Security/
│   │   └── PasswordHasher.cs
│   │
│   ├── Services/
│   │   ├── AuthService.cs        // Login, Register (Uses UserRepo, PasswordHasher)
│   │   ├── CategoryService.cs    // CRUD (Uses CategoryRepo)
│   │   ├── EventService.cs       // CRUD, Archive (Uses EventRepo, CategoryRepo, ArchiveRepo, ReminderRepo)
│   │   ├── ReminderService.cs    // CRUD (Uses ReminderRepo, EventRepo)
│   │   └── UserService.cs         // *** NEW *** UpdateEmail, ChangePassword (Uses UserRepo, PasswordHasher)
│   │
│   ├── Validators/
│   │   └── Validation.cs         // Static input validation (username, email, password).
│   │
│   └── Properties/
│
//======================================================================
├── reLIFE.Core (Class Library Project)
// PURPOSE: Basic data models and common elements.
//======================================================================
│   ├── Common/
│   │   └── (Enums: e.g., ViewType if different views added later)
│   │
│   ├── Models/
│   │   ├── ArchivedEvent.cs
│   │   ├── Category.cs
│   │   ├── Event.cs
│   │   ├── Reminder.cs
│   │   └── User.cs
│   │
│   └── Properties/
│
//======================================================================
└── reLIFE.WinFormsUI (Windows Forms App Project)
// PURPOSE: The user interface (Dashboard Shell, User Controls, Modal Forms).
//======================================================================
    ├── Properties/               // AssemblyInfo, Settings.settings
    │
    ├── Resources/                // Resources.resx (Images, Icons, etc.)
    │
    ├── Assets/                   // Other assets if any (fonts?)
    │
    ├── Forms/
    │   // PURPOSE: Standalone windows, primarily modal dialogs or the main shell.
    │   ├── ChangePasswordForm.cs   // *** NEW *** (Modal dialog for password change)
    │   ├── EventForm.cs            // *** MODIFIED *** (Modal dialog for Add/Edit Event details, includes Reminders)
    │   ├── LoginForm.cs          // (Modal dialog shown by Program.cs)
    │   ├── MainForm.cs           // *** MODIFIED *** (Dashboard Shell - contains navigation and content panel)
    │   └── RegistrationForm.cs     // (Modal dialog shown by Program.cs via LoginForm)
    │
    ├── UserControls/               // *** NEW Folder ***
    │   // PURPOSE: Reusable UI panels loaded into MainForm's content area.
    │   ├── AccountSettingsControl.cs // *** NEW *** (UI for Username(display)/Email update, Change Password button)
    │   ├── ArchiveViewControl.cs   // *** NEW *** (UI for viewing list of archived events)
    │   ├── CalendarViewControl.cs  // *** NEW *** (UI for selecting date, filtering, viewing event list, Add/Edit/Delete/Archive buttons)
    │   └── ReminderListControl.cs  // *** NEW *** (UI for viewing upcoming reminders - maybe less critical initially)
    │
    ├── appsettings.json          // Stores connection string. (Copy to Output Directory: Copy if newer)
    │
    └── Program.cs                // *** MODIFIED *** (Handles full app lifecycle, manual instantiation, Login->MainForm flow, Logout->Restart)
```
