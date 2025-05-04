// File: reLIFE.WinFormsUI/Helpers/ThemeHelper.cs
using MaterialSkin;
using MaterialSkin.Controls;
using reLIFE.WinFormsUI.Forms;
using System.Drawing; // Needed for Color
using System.Linq;
using System.Windows.Forms;

namespace reLIFE.WinFormsUI.Helpers
{
    public static class ThemeHelper
    {
        // Define Color Schemes
        public static readonly ColorScheme BlueScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.Amber400, TextShade.WHITE);
        public static readonly ColorScheme IndigoScheme = new ColorScheme(Primary.Indigo700, Primary.Indigo900, Primary.Indigo500, Accent.Pink200, TextShade.WHITE);
        public static readonly ColorScheme BlueGreyScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Teal400, TextShade.WHITE);
        public static readonly ColorScheme GreenScheme = new ColorScheme(Primary.Green800, Primary.Green900, Primary.Green500, Accent.DeepOrange400, TextShade.WHITE);

        // Store the current scheme name for retrieval
        private static string currentSchemeName = "Blue"; // Default

        public static void ApplyTheme(MaterialSkinManager.Themes theme, ColorScheme scheme, string schemeName)
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.Theme = theme;
            skinManager.ColorScheme = scheme;
            currentSchemeName = schemeName;

            var formsToUpdate = Application.OpenForms.OfType<MaterialForm>().ToList();
            foreach (var form in formsToUpdate)
            {
                // More aggressive update: Remove, Add, Invalidate
                skinManager.RemoveFormToManage(form);
                skinManager.AddFormToManage(form);
                // Invalidate children recursively AFTER re-adding might help
                form.Invalidate(true); // Invalidate with children
                form.Update();
            }
            // Also force redraw of the main window if it's a MaterialForm
            var mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault(); // Assuming MainForm is MaterialForm
            if (mainForm != null)
            {
                mainForm.Invalidate(true);
                mainForm.Update();
            }
        }

        public static ColorScheme GetSchemeByName(string? schemeName)
        {
            switch (schemeName?.ToLowerInvariant())
            {
                case "indigo": return IndigoScheme;
                case "bluegrey": return BlueGreyScheme;
                case "green": return GreenScheme;
                case "blue":
                default: return BlueScheme;
            }
        }

        // Helper to get the name of the currently applied scheme
        public static string GetCurrentSchemeName() => currentSchemeName;
    }
}