dddusing System;
using System.Windows.Forms;
using HelpDeskSystemFixed.Forms;

namespace HelpDeskSystemFixed
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                // Mostrar tela de login primeiro
                var loginForm = new LoginForm();
                var loginResult = loginForm.ShowDialog();
                
                if (loginResult == DialogResult.OK && loginForm.UsuarioLogado != null)
                {
                    // Login bem-sucedido, abrir sistema principal
                    var mainForm = new MainFormComLogin(loginForm.UsuarioLogado);
                    Application.Run(mainForm);
                }
                // Se login foi cancelado ou falhou, o programa encerra
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
