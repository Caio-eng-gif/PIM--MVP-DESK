using System;
using System.Drawing;
using System.Windows.Forms;

namespace HelpDeskSystemFixed.Forms
{
    public partial class UsuariosForm : Form
    {
        public UsuariosForm()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Gerenciar Usuários";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Título
            var lblTitulo = new Label
            {
                Text = "Gerenciamento de Usuários",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true,
                ForeColor = Color.DarkBlue
            };
            
            // Mensagem informativa
            var lblInfo = new Label
            {
                Text = "Funcionalidade em desenvolvimento...\n\nEsta tela permitirá:\n• Adicionar colaboradores e técnicos\n• Remover usuários\n• Resetar senhas automaticamente",
                Location = new Point(20, 80),
                Size = new Size(500, 200),
                Font = new Font("Arial", 12)
            };
            
            // Botão fechar
            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new Point(20, 320),
                Size = new Size(80, 30)
            };
            btnFechar.Click += (s, e) => this.Close();
            
            // Adicionar controles
            this.Controls.AddRange(new Control[] { lblTitulo, lblInfo, btnFechar });
        }
    }
}
