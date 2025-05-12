using MaterialSkin;
using MaterialSkin.Controls;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace reLIFE.WinFormsUI.Helpers
{
    public static class ThemeHelper
    {
        public static readonly ColorScheme BlueScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.Amber400, TextShade.WHITE);
        public static readonly ColorScheme IndigoScheme = new ColorScheme(Primary.Indigo700, Primary.Indigo900, Primary.Indigo500, Accent.Pink200, TextShade.WHITE);
        public static readonly ColorScheme BlueGreyScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.Teal400, TextShade.WHITE);
        public static readonly ColorScheme GreenScheme = new ColorScheme(Primary.Green800, Primary.Green900, Primary.Green500, Accent.DeepOrange400, TextShade.WHITE);

        private static string currentSchemeName = "Blue"; // Default

        public static void ApplyTheme(MaterialSkinManager.Themes theme, ColorScheme scheme, string schemeName)
        {
            var skinManager = MaterialSkinManager.Instance;
            skinManager.Theme = theme;
            skinManager.ColorScheme = scheme;
            currentSchemeName = schemeName;

            // Update existing managed forms
            var formsToUpdate = Application.OpenForms.OfType<MaterialForm>().ToList();
            foreach (var form in formsToUpdate)
            {
                // Re-adding might be necessary for some theme aspects
                skinManager.RemoveFormToManage(form); // Try removing first
                skinManager.AddFormToManage(form);    // Re-add applies current settings
                form.Invalidate(true); // Force redraw
                form.Update();
            }
        }

        public static ColorScheme GetSchemeByName(string? schemeName)
        {
            switch (schemeName?.ToLowerInvariant())
            {
                case "indigo": return IndigoScheme;
                case "bluegrey": return BlueGreyScheme;
                case "green": return GreenScheme;
                case "blue": default: return BlueScheme;
            }
        }
        public static string GetCurrentSchemeName() => currentSchemeName;
    }
}