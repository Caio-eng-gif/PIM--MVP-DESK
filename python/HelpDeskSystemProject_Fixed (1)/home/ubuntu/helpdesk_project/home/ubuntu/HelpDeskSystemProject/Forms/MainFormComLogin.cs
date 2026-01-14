using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDeskSystemFixed.Models;

namespace HelpDeskSystemFixed.Forms
{
    public partial class MainFormComLogin : Form
    {
        private Panel _sidebarPanel;
        private Panel _contentPanel;
        
        private Usuario _usuarioLogado;
        
        public MainFormComLogin(Usuario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
            
            // Inicialização manual dos componentes para evitar dependência do designer
            this.Text = "MVP DESK - Sistema de Help Desk";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 244, 247);
            this.WindowState = FormWindowState.Maximized;
            
	            CreateSidebar();
	            CreateContentArea();
	        }
        
        private void CreateSidebar()
        {
            _sidebarPanel = new Panel
            {
                Width = 250,
                Height = this.Height,
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(44, 62, 80)
            };
            
            // Logo e versão
            var lblLogo = new Label
            {
                Text = "MVP DESK",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 30),
                Size = new Size(210, 30),
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            var lblVersion = new Label
            {
                Text = "Sistema de Help Desk v2.0",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(20, 65),
                Size = new Size(210, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Informações do usuário logado
            var lblUsuarioLogado = new Label
            {
                Text = $"Bem-vindo, {_usuarioLogado.Nome}",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(52, 152, 219),
                Location = new Point(20, 90),
                Size = new Size(210, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            var lblTipoUsuario = new Label
            {
                Text = $"({_usuarioLogado.Tipo})",
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                ForeColor = Color.FromArgb(149, 165, 166),
                Location = new Point(20, 110),
                Size = new Size(210, 15),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Botões do menu
            var btnDashboard = CreateMenuButton("📊 Dashboard", 150, OnDashboard);
            var btnChamados = CreateMenuButton("🎫 Gerenciar Chamados", 200, OnGerenciarChamados);
            var btnNovoChamado = CreateMenuButton("➕ Novo Chamado", 250, OnNovoChamado);
            var btnRelatorios = CreateMenuButton("📈 Relatórios", 300, OnRelatorios);
            var btnUsuarios = CreateMenuButton("👥 Usuários", 350, OnGerenciarUsuarios);
            var btnConfiguracoes = CreateMenuButton("⚙️ Configurações", 400, OnConfiguracoes);
            
            // Botão sair na parte inferior
            var btnSair = new Button
            {
                Text = "🚪 Sair do Sistema",
                Location = new Point(20, 600),
                Size = new Size(210, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0)
            };
            btnSair.FlatAppearance.BorderSize = 0;
            btnSair.Click += (s, e) => {
                var result = MessageBox.Show("Deseja realmente sair do sistema?", "Confirmar Saída", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            };
            
            _sidebarPanel.Controls.AddRange(new Control[] 
            { 
                lblLogo, lblVersion, lblUsuarioLogado, lblTipoUsuario, btnDashboard, btnChamados, 
                btnNovoChamado, btnRelatorios, btnUsuarios, btnConfiguracoes, btnSair 
            });
            
            this.Controls.Add(_sidebarPanel);
        }
        
        private Button CreateMenuButton(string text, int y, EventHandler clickHandler)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(20, y),
                Size = new Size(210, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(52, 73, 94);
            btn.Click += clickHandler;
            
            return btn;
        }
        
        private void CreateContentArea()
        {
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 244, 247),
                Padding = new Padding(30)
            };
            
            // Header
            var headerPanel = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Padding = new Padding(30, 20, 30, 20)
            };
            
            var lblHeader = new Label
            {
                Text = "Bem-vindo ao MVP DESK",
                Font = new Font("Segoe UI", 16, FontStyle.Regular),
                ForeColor = Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            
            headerPanel.Controls.Add(lblHeader);
            
            // Área principal de conteúdo
            var mainContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(30),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            var lblBoasVindas = new Label
            {
                Text = $"Bem-vindo(a) ao MVP DESK, {_usuarioLogado.Nome}!",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.TopLeft
            };
            
            mainContentPanel.Controls.Add(lblBoasVindas);
            
            
            
            _contentPanel.Controls.AddRange(new Control[] { headerPanel, mainContentPanel });
            this.Controls.Add(_contentPanel);
        }
        
private void OnDashboard(object sender, EventArgs e)
        {
            try
            {
                var dashboardForm = new DashboardForm(_usuarioLogado);
                dashboardForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao abrir Dashboard: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
	        private void OnGerenciarChamados(object sender, EventArgs e)
	        {
	            try
	            {
	                var chamadosForm = new ChamadosForm();
	                chamadosForm.ShowDialog();
	            }
	            catch (Exception ex)
	            {
	                MessageBox.Show($"Erro ao abrir gerenciamento de chamados: {ex.Message}", "Erro", 
	                    MessageBoxButtons.OK, MessageBoxIcon.Error);
	            }
	        }
	        
	        private void OnNovoChamado(object sender, EventArgs e)
	        {
	            try
	            {
	                var novoChamadoForm = new NovoChamadoForm();
	                var result = novoChamadoForm.ShowDialog();
	            }
	            catch (Exception ex)
	            {
	                MessageBox.Show($"Erro ao criar novo chamado: {ex.Message}", "Erro", 
	                    MessageBoxButtons.OK, MessageBoxIcon.Error);
	            }
	        }
	        
	        private void OnRelatorios(object sender, EventArgs e)
	        {
	            try
	            {
	                var relatoriosForm = new RelatoriosForm();
	                relatoriosForm.ShowDialog();
	            }
	            catch (Exception ex)
	            {
	                MessageBox.Show($"Erro ao abrir relatórios: {ex.Message}", "Erro", 
	                    MessageBoxButtons.OK, MessageBoxIcon.Error);
	            }
	        }
	        
	        private void OnGerenciarUsuarios(object sender, EventArgs e)
	        {
	            try
	            {
	                var usuariosForm = new UsuariosFormCompleto();
	                usuariosForm.ShowDialog();
	            }
	            catch (Exception ex)
	            {
	                MessageBox.Show($"Erro ao abrir gerenciamento de usuários: {ex.Message}", "Erro", 
	                    MessageBoxButtons.OK, MessageBoxIcon.Error);
	            }
	        }
	        
	        private void OnConfiguracoes(object sender, EventArgs e)
	        {
	            var configForm = new ConfiguracoesForm();
	            configForm.ShowDialog();
	        }
	    }
	}
