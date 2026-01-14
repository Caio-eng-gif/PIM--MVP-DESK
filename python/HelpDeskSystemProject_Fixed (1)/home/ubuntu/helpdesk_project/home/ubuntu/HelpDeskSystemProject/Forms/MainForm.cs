using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HelpDeskSystemFixed.Forms
{
    public partial class MainForm : Form
    {
        private Panel _sidebarPanel;
        private Panel _contentPanel;
        private Label _lblStats;
        
        public MainForm()
        {
            InitializeComponent();
            
            // Carregar dashboard após a interface estar pronta
            this.Load += (s, e) => {
                try
                {
                    LoadDashboard();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao inicializar dashboard: {ex.Message}", "Aviso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
        }
        
        private void InitializeComponent()
        {
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
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(0, 20, 0, 0)
            };
            
            // Logo/Título
            var lblLogo = new Label
            {
                Text = "MVP DESK",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(210, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            var lblVersion = new Label
            {
                Text = "Sistema de Help Desk v2.0",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(189, 195, 199),
                Location = new Point(20, 50),
                Size = new Size(210, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Botões do menu
            var btnDashboard = CreateMenuButton("📊 Dashboard", 100, OnDashboard);
            var btnChamados = CreateMenuButton("🎫 Gerenciar Chamados", 150, OnGerenciarChamados);
            var btnNovoChamado = CreateMenuButton("➕ Novo Chamado", 200, OnNovoChamado);
            var btnRelatorios = CreateMenuButton("📈 Relatórios", 250, OnRelatorios);
            var btnUsuarios = CreateMenuButton("👥 Usuários", 300, OnGerenciarUsuarios);
            var btnConfiguracoes = CreateMenuButton("⚙️ Configurações", 350, OnConfiguracoes);
            
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
            btnSair.Click += (s, e) => Application.Exit();
            
            _sidebarPanel.Controls.AddRange(new Control[] 
            { 
                lblLogo, lblVersion, btnDashboard, btnChamados, btnNovoChamado, 
                btnRelatorios, btnUsuarios, btnConfiguracoes, btnSair 
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
                Padding = new Padding(20)
            };
            
            var lblWelcome = new Label
            {
                Text = "Bem-vindo ao MVP DESK",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 20),
                AutoSize = true
            };
            
            var lblDate = new Label
            {
                Text = DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy"),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(20, 50),
                AutoSize = true
            };
            
            headerPanel.Controls.AddRange(new Control[] { lblWelcome, lblDate });
            
            // Área de conteúdo principal
            var mainContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(20)
            };
            
            _lblStats = new Label
            {
                Text = "Carregando estatísticas...",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(0, 0),
                Size = new Size(800, 400),
                BackColor = Color.White,
                Padding = new Padding(20),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            mainContentPanel.Controls.Add(_lblStats);
            
            _contentPanel.Controls.AddRange(new Control[] { headerPanel, mainContentPanel });
            this.Controls.Add(_contentPanel);
        }
        
        private void LoadDashboard()
        {
            try
            {
                _lblStats.Text = "🔄 Testando Dashboard...";
                
                // Teste básico primeiro
                _lblStats.Text = $@"📊 DASHBOARD MVP DESK - TESTE BÁSICO

🕒 Horário atual: {DateTime.Now:dd/MM/yyyy HH:mm:ss}

🔧 Testando componentes...";
                
                // Testar repositórios um por vez
                var chamadoRepo = new Repositories.ChamadoRepository();
                _lblStats.Text += "\n✅ ChamadoRepository criado";
                
                var usuarioRepo = new Repositories.UsuarioRepository();
                _lblStats.Text += "\n✅ UsuarioRepository criado";
                
                // Testar GetAll
                var chamados = chamadoRepo.GetAll();
                _lblStats.Text += $"\n✅ Chamados carregados: {chamados?.Count ?? 0}";
                
                var usuarios = usuarioRepo.GetAll();
                _lblStats.Text += $"\n✅ Usuários carregados: {usuarios?.Count ?? 0}";
                
                // Estatísticas básicas
                var totalChamados = chamados?.Count ?? 0;
                var totalUsuarios = usuarios?.Count ?? 0;
                
                _lblStats.Text += $@"

📊 ESTATÍSTICAS BÁSICAS:
• Total de Chamados: {totalChamados}
• Total de Usuários: {totalUsuarios}

✅ Dashboard funcionando!
🕒 Última atualização: {DateTime.Now:HH:mm:ss}";
                
                _lblStats.ForeColor = Color.FromArgb(0, 128, 0); // Verde
            }
            catch (Exception ex)
            {
                _lblStats.Text = $@"❌ ERRO DETALHADO NO DASHBOARD

Tipo do erro: {ex.GetType().Name}
Mensagem: {ex.Message}

Stack Trace:
{ex.StackTrace}

🕒 {DateTime.Now:HH:mm:ss}";
                
                _lblStats.ForeColor = Color.FromArgb(255, 0, 0); // Vermelho
            }
        }
        
        private void OnDashboard(object sender, EventArgs e)
        {
            try
            {
                // Forçar atualização do dashboard
                LoadDashboard();
                
                // Garantir que o painel de estatísticas está visível
                _lblStats.Visible = true;
                _lblStats.BringToFront();
                
                // Atualizar a tela
                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar dashboard: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void OnGerenciarChamados(object sender, EventArgs e)
        {
            try
            {
                var chamadosForm = new ChamadosForm();
                chamadosForm.ShowDialog();
                LoadDashboard(); // Atualizar após fechar
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
                if (novoChamadoForm.ShowDialog() == DialogResult.OK)
                {
                    LoadDashboard();
                    MessageBox.Show("Chamado criado com sucesso!", "Sucesso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
                LoadDashboard(); // Atualizar após fechar
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
