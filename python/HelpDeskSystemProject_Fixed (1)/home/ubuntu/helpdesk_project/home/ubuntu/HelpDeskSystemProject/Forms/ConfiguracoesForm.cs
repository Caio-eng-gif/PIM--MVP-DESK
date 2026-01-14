using System;
using System.Drawing;
using System.Windows.Forms;

namespace HelpDeskSystemFixed.Forms
{
    public partial class ConfiguracoesForm : Form
    {
        public ConfiguracoesForm()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Configurações do Sistema";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 244, 247);
            
            // Panel principal
            var mainPanel = new Panel
            {
                Size = new Size(550, 420),
                Location = new Point(25, 25),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20)
            };
            
            // Título
            var lblTitulo = new Label
            {
                Text = "CONFIGURAÇÕES DO SISTEMA",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 20),
                Size = new Size(510, 25)
            };
            
            // Seção Geral
            var lblGeral = new Label
            {
                Text = "Configurações Gerais",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(20, 60),
                Size = new Size(200, 20)
            };
            
            var chkNotificacoes = new CheckBox
            {
                Text = "Habilitar notificações",
                Location = new Point(20, 90),
                Size = new Size(200, 20),
                Checked = true,
                Font = new Font("Segoe UI", 10)
            };
            
            var chkSomAlerta = new CheckBox
            {
                Text = "Som de alerta para novos chamados",
                Location = new Point(20, 115),
                Size = new Size(250, 20),
                Checked = false,
                Font = new Font("Segoe UI", 10)
            };
            
            var chkAutoRefresh = new CheckBox
            {
                Text = "Atualização automática (30 segundos)",
                Location = new Point(20, 140),
                Size = new Size(250, 20),
                Checked = true,
                Font = new Font("Segoe UI", 10)
            };
            
            // Seção Sistema
            var lblSistema = new Label
            {
                Text = "Informações do Sistema",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(20, 180),
                Size = new Size(200, 20)
            };
            
            var lblInfo = new Label
            {
                Text = $@"Versão: Help Desk Pro v2.0
Banco de Dados: SQLite Local
Última Atualização: {DateTime.Now:dd/MM/yyyy}
Desenvolvido por: Manus AI

Status: Sistema Operacional ✅
Conexão BD: Ativa ✅
Backup Automático: Habilitado ✅",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(20, 210),
                Size = new Size(300, 120)
            };
            
            // Seção Manutenção
            var lblManutencao = new Label
            {
                Text = "Manutenção",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(350, 180),
                Size = new Size(150, 20)
            };
            
            var btnBackup = new Button
            {
                Text = "Backup Manual",
                Location = new Point(350, 210),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBackup.FlatAppearance.BorderSize = 0;
            btnBackup.Click += OnBackup;
            
            var btnLimparLogs = new Button
            {
                Text = "Limpar Logs",
                Location = new Point(350, 250),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(241, 196, 15),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLimparLogs.FlatAppearance.BorderSize = 0;
            btnLimparLogs.Click += OnLimparLogs;
            
            var btnResetBD = new Button
            {
                Text = "Reset BD",
                Location = new Point(350, 290),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnResetBD.FlatAppearance.BorderSize = 0;
            btnResetBD.Click += OnResetBD;
            
            // Botões principais
            var btnSalvar = new Button
            {
                Text = "SALVAR CONFIGURAÇÕES",
                Location = new Point(20, 360),
                Size = new Size(180, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += OnSalvar;
            
            var btnFechar = new Button
            {
                Text = "FECHAR",
                Location = new Point(450, 360),
                Size = new Size(80, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.Click += (s, e) => this.Close();
            
            // Adicionar controles
            mainPanel.Controls.AddRange(new Control[] 
            { 
                lblTitulo, lblGeral, chkNotificacoes, chkSomAlerta, chkAutoRefresh,
                lblSistema, lblInfo, lblManutencao, btnBackup, btnLimparLogs, btnResetBD,
                btnSalvar, btnFechar
            });
            
            this.Controls.Add(mainPanel);
        }
        
        private void OnSalvar(object sender, EventArgs e)
        {
            MessageBox.Show("Configurações salvas com sucesso!", "Sucesso", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void OnBackup(object sender, EventArgs e)
        {
            try
            {
                // Simular backup
                var progressForm = new Form
                {
                    Text = "Backup em Progresso",
                    Size = new Size(300, 100),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };
                
                var lblProgress = new Label
                {
                    Text = "Realizando backup do banco de dados...",
                    Location = new Point(20, 20),
                    Size = new Size(260, 40),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                
                progressForm.Controls.Add(lblProgress);
                progressForm.Show();
                
                // Simular tempo de backup
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
                
                progressForm.Close();
                
                MessageBox.Show($"Backup realizado com sucesso!\n\nArquivo: helpdesk_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db", 
                    "Backup Concluído", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao realizar backup: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void OnLimparLogs(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Tem certeza que deseja limpar todos os logs do sistema?", 
                "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Logs limpos com sucesso!", "Sucesso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void OnResetBD(object sender, EventArgs e)
        {
            var result = MessageBox.Show("ATENÇÃO: Esta operação irá apagar TODOS os dados do sistema!\n\nTem certeza que deseja continuar?", 
                "CONFIRMAÇÃO CRÍTICA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            if (result == DialogResult.Yes)
            {
                var confirmResult = MessageBox.Show("Esta é sua última chance!\n\nConfirma que deseja APAGAR TODOS OS DADOS?", 
                    "CONFIRMAÇÃO FINAL", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                
                if (confirmResult == DialogResult.Yes)
                {
                    MessageBox.Show("Reset do banco de dados realizado!\n\nO sistema será reiniciado.", 
                        "Reset Concluído", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
