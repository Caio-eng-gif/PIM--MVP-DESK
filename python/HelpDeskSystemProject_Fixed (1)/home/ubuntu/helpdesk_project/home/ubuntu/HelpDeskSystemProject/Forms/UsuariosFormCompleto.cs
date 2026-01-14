using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDeskSystemFixed.Models;
using HelpDeskSystemFixed.Repositories;

namespace HelpDeskSystemFixed.Forms
{
    public partial class UsuariosFormCompleto : Form
    {
        private readonly UsuarioRepository _usuarioRepository;
        private DataGridView _dataGridView;
        private TextBox _txtNome;
        private TextBox _txtEmail;
        private ComboBox _cmbTipo;
        private Button _btnSalvar;
        private Button _btnNovo;
        private Button _btnEditar;
        private Button _btnExcluir;
        private Button _btnResetSenha;
        private int _usuarioEditandoId = 0;
        
        public UsuariosFormCompleto()
        {
            _usuarioRepository = new UsuarioRepository();
            InitializeComponent();
            LoadData();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Gerenciamento de Usuários";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(240, 244, 247);
            
            // Título
            var lblTitulo = new Label
            {
                Text = "GERENCIAMENTO DE USUÁRIOS",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 20),
                AutoSize = true
            };
            
            // Panel para formulário
            var formPanel = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(850, 120),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };
            
            // Campos do formulário
            var lblNome = new Label { Text = "Nome:", Location = new Point(15, 15), Size = new Size(50, 20), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            _txtNome = new TextBox { Location = new Point(15, 35), Size = new Size(200, 23), Font = new Font("Segoe UI", 9) };
            
            var lblEmail = new Label { Text = "Email:", Location = new Point(230, 15), Size = new Size(50, 20), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            _txtEmail = new TextBox { Location = new Point(230, 35), Size = new Size(200, 23), Font = new Font("Segoe UI", 9) };
            
            var lblTipo = new Label { Text = "Tipo:", Location = new Point(445, 15), Size = new Size(40, 20), Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            _cmbTipo = new ComboBox { Location = new Point(445, 35), Size = new Size(120, 23), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9) };
            
            // Botões
            _btnNovo = CreateButton("NOVO", new Point(15, 75), Color.FromArgb(52, 152, 219), OnNovo);
            _btnSalvar = CreateButton("SALVAR", new Point(105, 75), Color.FromArgb(46, 204, 113), OnSalvar);
            _btnEditar = CreateButton("EDITAR", new Point(195, 75), Color.FromArgb(241, 196, 15), OnEditar);
            _btnExcluir = CreateButton("EXCLUIR", new Point(285, 75), Color.FromArgb(231, 76, 60), OnExcluir);
            _btnResetSenha = CreateButton("RESET SENHA", new Point(375, 75), Color.FromArgb(155, 89, 182), OnResetSenha);
            
            formPanel.Controls.AddRange(new Control[] 
            { 
                lblNome, _txtNome, lblEmail, _txtEmail, lblTipo, _cmbTipo,
                _btnNovo, _btnSalvar, _btnEditar, _btnExcluir, _btnResetSenha
            });
            
            // DataGridView
            _dataGridView = new DataGridView
            {
                Location = new Point(20, 200),
                Size = new Size(850, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White
            };
            _dataGridView.SelectionChanged += OnSelectionChanged;
            
            // Botão Fechar
            var btnFechar = new Button
            {
                Text = "FECHAR",
                Location = new Point(790, 520),
                Size = new Size(80, 30),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.Click += (s, e) => this.Close();
            
            // Carregar tipos de usuário
            var tipos = new[]
            {
                new { Value = TipoUsuario.Colaborador, Text = "Colaborador" },
                new { Value = TipoUsuario.Tecnico, Text = "Técnico" },
                new { Value = TipoUsuario.Administrador, Text = "Administrador" }
            };
            _cmbTipo.DataSource = tipos;
            _cmbTipo.DisplayMember = "Text";
            _cmbTipo.ValueMember = "Value";
            
            this.Controls.AddRange(new Control[] { lblTitulo, formPanel, _dataGridView, btnFechar });
            
            LimparFormulario();
        }
        
        private Button CreateButton(string text, Point location, Color backColor, EventHandler clickHandler)
        {
            var btn = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(80, 30),
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += clickHandler;
            return btn;
        }
        
        private void LoadData()
        {
            try
            {
                var usuarios = _usuarioRepository.GetAll();
                var dataSource = usuarios.Select(u => new
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Tipo = GetTipoText(u.Tipo),
                    DataCriacao = u.DataCriacao.ToString("dd/MM/yyyy"),
                    Status = u.Ativo ? "Ativo" : "Inativo"
                }).ToList();
                
                _dataGridView.DataSource = dataSource;
                
                if (_dataGridView.Columns.Count > 0)
                {
                    _dataGridView.Columns["Id"].Width = 50;
                    _dataGridView.Columns["Nome"].HeaderText = "Nome";
                    _dataGridView.Columns["Email"].HeaderText = "Email";
                    _dataGridView.Columns["Tipo"].HeaderText = "Tipo";
                    _dataGridView.Columns["DataCriacao"].HeaderText = "Data Criação";
                    _dataGridView.Columns["Status"].HeaderText = "Status";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar usuários: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (_dataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = _dataGridView.SelectedRows[0];
                var userId = (int)selectedRow.Cells["Id"].Value;
                var usuario = _usuarioRepository.GetById(userId);
                
                if (usuario != null)
                {
                    _usuarioEditandoId = usuario.Id;
                    _txtNome.Text = usuario.Nome;
                    _txtEmail.Text = usuario.Email;
                    _cmbTipo.SelectedValue = usuario.Tipo;
                    
                    _btnSalvar.Text = "ATUALIZAR";
                    _btnEditar.Enabled = true;
                    _btnExcluir.Enabled = true;
                    _btnResetSenha.Enabled = true;
                }
            }
        }
        
        private void OnNovo(object sender, EventArgs e)
        {
            LimparFormulario();
        }
        
        private void OnSalvar(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_txtNome.Text))
                {
                    MessageBox.Show("Informe o nome do usuário.", "Validação", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(_txtEmail.Text))
                {
                    MessageBox.Show("Informe o email do usuário.", "Validação", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Corrigir o problema do SelectedValue
                TipoUsuario tipoSelecionado = TipoUsuario.Colaborador; // Valor padrão
                if (_cmbTipo.SelectedValue != null)
                {
                    tipoSelecionado = (TipoUsuario)_cmbTipo.SelectedValue;
                }
                
                var usuario = new Usuario
                {
                    Id = _usuarioEditandoId,
                    Nome = _txtNome.Text.Trim(),
                    Email = _txtEmail.Text.Trim(),
                    Tipo = tipoSelecionado
                };
                
                bool sucesso;
                if (_usuarioEditandoId == 0)
                {
                    // Novo usuário - gerar senha padrão
                    usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456");
                    sucesso = _usuarioRepository.Add(usuario);
                    
                    if (sucesso)
                    {
                        MessageBox.Show($"Usuário criado com sucesso!\n\nSenha padrão: 123456\n\nOriente o usuário a alterar a senha no primeiro acesso.", 
                            "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // Atualizar usuário existente
                    sucesso = _usuarioRepository.Update(usuario);
                    
                    if (sucesso)
                    {
                        MessageBox.Show("Usuário atualizado com sucesso!", "Sucesso", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
                if (sucesso)
                {
                    LoadData();
                    LimparFormulario();
                }
                else
                {
                    MessageBox.Show("Erro ao salvar usuário.", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar usuário: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void OnEditar(object sender, EventArgs e)
        {
            if (_usuarioEditandoId == 0)
            {
                MessageBox.Show("Selecione um usuário para editar.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void OnExcluir(object sender, EventArgs e)
        {
            if (_usuarioEditandoId == 0)
            {
                MessageBox.Show("Selecione um usuário para excluir.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var result = MessageBox.Show("Tem certeza que deseja excluir este usuário?", "Confirmação", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                if (_usuarioRepository.Delete(_usuarioEditandoId))
                {
                    MessageBox.Show("Usuário excluído com sucesso!", "Sucesso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    LimparFormulario();
                }
                else
                {
                    MessageBox.Show("Erro ao excluir usuário.", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void OnResetSenha(object sender, EventArgs e)
        {
            if (_usuarioEditandoId == 0)
            {
                MessageBox.Show("Selecione um usuário para resetar a senha.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var result = MessageBox.Show("Tem certeza que deseja resetar a senha deste usuário para '123456'?", "Confirmação", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                var novaSenhaHash = BCrypt.Net.BCrypt.HashPassword("123456");
                
                if (_usuarioRepository.UpdatePassword(_usuarioEditandoId, novaSenhaHash))
                {
                    MessageBox.Show($"Senha resetada com sucesso!\n\nNova senha: 123456\n\nOriente o usuário a alterar a senha.", 
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Erro ao resetar senha.", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void LimparFormulario()
        {
            _usuarioEditandoId = 0;
            _txtNome.Clear();
            _txtEmail.Clear();
            
            // Corrigir o problema do SelectedIndex
            if (_cmbTipo.Items.Count > 0)
            {
                _cmbTipo.SelectedIndex = 0;
            }
            else
            {
                _cmbTipo.SelectedIndex = -1;
            }
            
            _btnSalvar.Text = "SALVAR";
            _btnEditar.Enabled = false;
            _btnExcluir.Enabled = false;
            _btnResetSenha.Enabled = false;
        }
        
        private string GetTipoText(TipoUsuario tipo)
        {
            return tipo switch
            {
                TipoUsuario.Colaborador => "Colaborador",
                TipoUsuario.Tecnico => "Técnico",
                TipoUsuario.Administrador => "Administrador",
                _ => "Indefinido"
            };
        }
    }
}
