using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using reLIFE.BusinessLogic.Services;
using reLIFE.BusinessLogic.Validators;
using reLIFE.Core.Models;

namespace reLIFE.WinFormsUI.Forms
{
    public partial class RegistrationForm : Form
    {
        private readonly AuthService _authService;
        public RegistrationForm(AuthService authService)
        {
            InitializeComponent();
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {

        }
    }
}
