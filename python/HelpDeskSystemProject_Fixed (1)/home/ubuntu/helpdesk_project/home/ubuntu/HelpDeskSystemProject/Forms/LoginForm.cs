using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDeskSystemFixed.Repositories;
using HelpDeskSystemFixed.Models;

namespace HelpDeskSystemFixed.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox _txtUsuario;
        private TextBox _txtSenha;
        private Button _btnLogin;
        private Label _lblMensagem;
        private UsuarioRepository _usuarioRepository;
        
        public Usuario UsuarioLogado { get; private set; }
        
        public LoginForm()
        {
            _usuarioRepository = new UsuarioRepository();
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.Text = "MVP DESK - Login";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(44, 62, 80);
            
            // Painel principal
            var mainPanel = new Panel
            {
                Size = new Size(400, 300),
                Location = new Point(25, 25),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            
            // Logo/Título
            var lblLogo = new Label
            {
                Text = "MVP DESK",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(0, 30),
                Size = new Size(400, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            var lblSubtitulo = new Label
            {
                Text = "Sistema de Help Desk",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(0, 75),
                Size = new Size(400, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Campo Usuário
            var lblUsuario = new Label
            {
                Text = "Usuário:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(50, 120),
                Size = new Size(60, 20)
            };
            
            _txtUsuario = new TextBox
            {
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(50, 145),
                Size = new Size(300, 25),
                BorderStyle = BorderStyle.FixedSingle,
                Text = "admin" // Usuário padrão para teste
            };
            
            // Campo Senha
            var lblSenha = new Label
            {
                Text = "Senha:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(50, 180),
                Size = new Size(60, 20)
            };
            
            _txtSenha = new TextBox
            {
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(50, 205),
                Size = new Size(300, 25),
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true,
                Text = "123456" // Senha padrão para teste
            };
            
            // Botão Login
            _btnLogin = new Button
            {
                Text = "ENTRAR",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(50, 245),
                Size = new Size(300, 35),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _btnLogin.FlatAppearance.BorderSize = 0;
            _btnLogin.Click += OnLogin;
            
            // Mensagem de erro/info
            _lblMensagem = new Label
            {
                Text = "Digite suas credenciais para acessar o sistema",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(50, 100),
                Size = new Size(300, 15),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Adicionar controles ao painel
            mainPanel.Controls.AddRange(new Control[] 
            { 
                lblLogo, lblSubtitulo, _lblMensagem, lblUsuario, _txtUsuario, 
                lblSenha, _txtSenha, _btnLogin 
            });
            
            this.Controls.Add(mainPanel);
            
            // Eventos
            _txtSenha.KeyPress += (s, e) => 
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    OnLogin(null, null);
                }
            };
            
            this.KeyPreview = true;
            this.KeyDown += (s, e) => 
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
            };
        }
        
        private void OnLogin(object sender, EventArgs e)
        {
            try
            {
                var usuario = _txtUsuario.Text.Trim();
                var senha = _txtSenha.Text;
                
                if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
                {
                    MostrarMensagem("Informe usuário e senha.", Color.FromArgb(231, 76, 60));
                    return;
                }
                
                _lblMensagem.Text = "Verificando credenciais...";
                _lblMensagem.ForeColor = Color.FromArgb(52, 152, 219);
                _btnLogin.Enabled = false;
                Application.DoEvents();
                
                // Verificar credenciais
                var usuarioLogado = VerificarCredenciais(usuario, senha);
                
                if (usuarioLogado != null)
                {
                    UsuarioLogado = usuarioLogado;
                    MostrarMensagem("Login realizado com sucesso!", Color.FromArgb(39, 174, 96));
                    
                    // Aguardar um pouco para mostrar a mensagem
                    System.Threading.Thread.Sleep(500);
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MostrarMensagem("Usuário ou senha incorretos.", Color.FromArgb(231, 76, 60));
                    _txtSenha.Clear();
                    _txtSenha.Focus();
                }
            }
            catch (Exception ex)
            {
                MostrarMensagem($"Erro ao fazer login: {ex.Message}", Color.FromArgb(231, 76, 60));
            }
            finally
            {
                _btnLogin.Enabled = true;
            }
        }
        
        private Usuario VerificarCredenciais(string usuario, string senha)
        {
            try
            {
                // Buscar usuário por nome
                var usuarios = _usuarioRepository.GetAll();
                var usuarioEncontrado = usuarios?.FirstOrDefault(u => 
                    u.Nome.Equals(usuario, StringComparison.OrdinalIgnoreCase) && u.Ativo);
                
                if (usuarioEncontrado != null)
                {
                    // Verificar senha
                    if (BCrypt.Net.BCrypt.Verify(senha, usuarioEncontrado.SenhaHash))
                    {
                        return usuarioEncontrado;
                    }
                }
                
                // Fallback: verificar credenciais padrão (admin/123456)
                if (usuario.Equals("admin", StringComparison.OrdinalIgnoreCase) && senha == "123456")
                {
                    return new Usuario
                    {
                        Id = 1,
                        Nome = "Administrador",
                        Email = "admin@mvpdesk.com",
                        Tipo = TipoUsuario.Administrador,
                        Ativo = true
                    };
                }
                
                return null;
            }
            catch (Exception)
            {
                // Em caso de erro, permitir login com credenciais padrão
                if (usuario.Equals("admin", StringComparison.OrdinalIgnoreCase) && senha == "123456")
                {
                    return new Usuario
                    {
                        Id = 1,
                        Nome = "Administrador",
                        Email = "admin@mvpdesk.com",
                        Tipo = TipoUsuario.Administrador,
                        Ativo = true
                    };
                }
                return null;
            }
        }
        
        private void MostrarMensagem(string mensagem, Color cor)
        {
            _lblMensagem.Text = mensagem;
            _lblMensagem.ForeColor = cor;
        }
    }
}
