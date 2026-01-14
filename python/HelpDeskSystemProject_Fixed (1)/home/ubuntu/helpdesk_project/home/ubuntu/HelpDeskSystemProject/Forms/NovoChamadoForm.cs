using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDeskSystemFixed.Models;
using HelpDeskSystemFixed.Repositories;

namespace HelpDeskSystemFixed.Forms
{
    public partial class NovoChamadoForm : Form
    {
        private readonly ChamadoRepository _chamadoRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private ComboBox _cmbColaborador;
        private ComboBox _cmbTecnico;
        private TextBox _txtDescricao;
        
        public NovoChamadoForm()
        {
            _chamadoRepository = new ChamadoRepository();
            _usuarioRepository = new UsuarioRepository();
            InitializeComponent();
            LoadData();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Novo Chamado";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 244, 247);
            
            // Panel principal
            var mainPanel = new Panel
            {
                Size = new Size(450, 320),
                Location = new Point(25, 25),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20)
            };
            
            // Título
            var lblTitulo = new Label
            {
                Text = "CRIAR NOVO CHAMADO",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 20),
                Size = new Size(410, 25)
            };
            
            // Colaborador
            var lblColaborador = new Label
            {
                Text = "Colaborador:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 60),
                Size = new Size(100, 20)
            };
            
            _cmbColaborador = new ComboBox
            {
                Location = new Point(20, 80),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            
            // Técnico
            var lblTecnico = new Label
            {
                Text = "Técnico (Opcional):",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(230, 60),
                Size = new Size(130, 20)
            };
            
            _cmbTecnico = new ComboBox
            {
                Location = new Point(230, 80),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            
            // Descrição
            var lblDescricao = new Label
            {
                Text = "Descrição do Problema:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 120),
                Size = new Size(200, 20)
            };
            
            _txtDescricao = new TextBox
            {
                Location = new Point(20, 140),
                Size = new Size(410, 100),
                Font = new Font("Segoe UI", 10),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            
            // Botões
            var btnSalvar = new Button
            {
                Text = "CRIAR CHAMADO",
                Location = new Point(20, 260),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += OnSalvar;
            
            var btnCancelar = new Button
            {
                Text = "CANCELAR",
                Location = new Point(180, 260),
                Size = new Size(100, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            
            // Adicionar controles
            mainPanel.Controls.AddRange(new Control[] 
            { 
                lblTitulo, lblColaborador, _cmbColaborador, lblTecnico, _cmbTecnico,
                lblDescricao, _txtDescricao, btnSalvar, btnCancelar
            });
            
            this.Controls.Add(mainPanel);
        }
        
        private void LoadData()
        {
            try
            {
                // Carregar colaboradores
                var colaboradores = _usuarioRepository.GetByTipo(TipoUsuario.Colaborador);
                _cmbColaborador.DataSource = colaboradores;
                _cmbColaborador.DisplayMember = "Nome";
                _cmbColaborador.ValueMember = "Id";
                
                // Carregar técnicos
                var tecnicos = _usuarioRepository.GetByTipo(TipoUsuario.Tecnico);
                var tecnicosComVazio = new List<Usuario> { new Usuario { Id = 0, Nome = "-- Não atribuído --" } };
                tecnicosComVazio.AddRange(tecnicos);
                _cmbTecnico.DataSource = tecnicosComVazio;
                _cmbTecnico.DisplayMember = "Nome";
                _cmbTecnico.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void OnSalvar(object sender, EventArgs e)
        {
            try
            {
                // Validações
                if (_cmbColaborador.SelectedValue == null)
                {
                    MessageBox.Show("Selecione um colaborador.", "Validação", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(_txtDescricao.Text))
                {
                    MessageBox.Show("Informe a descrição do chamado.", "Validação", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Criar chamado
                var chamado = new Chamado
                {
                    IdColaborador = (int)_cmbColaborador.SelectedValue,
                    IdTecnico = (int)_cmbTecnico.SelectedValue == 0 ? null : (int)_cmbTecnico.SelectedValue,
                    Descricao = _txtDescricao.Text.Trim(),
                    Status = StatusChamado.Pendente
                };
                
                if (_chamadoRepository.Add(chamado))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Erro ao criar chamado.", "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar chamado: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
