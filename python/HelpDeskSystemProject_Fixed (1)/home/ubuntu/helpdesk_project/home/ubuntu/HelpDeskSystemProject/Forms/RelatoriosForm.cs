using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDeskSystemFixed.Models;
using HelpDeskSystemFixed.Repositories;

namespace HelpDeskSystemFixed.Forms
{
    public partial class RelatoriosForm : Form
    {
        private readonly ChamadoRepository _chamadoRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private DataGridView _dataGridView;
        
        public RelatoriosForm()
        {
            _chamadoRepository = new ChamadoRepository();
            _usuarioRepository = new UsuarioRepository();
            InitializeComponent();
            LoadData();
        }
        
        private void InitializeComponent()
        {
            this.Text = "Relatório de Chamados";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Título
            var lblTitulo = new Label
            {
                Text = "Relatório de Chamados por Técnico",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true,
                ForeColor = Color.DarkBlue
            };
            
            // DataGridView para exibir relatório
            _dataGridView = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(750, 350),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            
            // Botões
            var btnAtualizar = new Button
            {
                Text = "Atualizar Relatório",
                Location = new Point(20, 430),
                Size = new Size(150, 30),
                BackColor = Color.LightGreen
            };
            btnAtualizar.Click += (s, e) => LoadData();
            
            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new Point(190, 430),
                Size = new Size(80, 30)
            };
            btnFechar.Click += (s, e) => this.Close();
            
            // Adicionar controles ao formulário
            this.Controls.AddRange(new Control[] 
            { 
                lblTitulo,
                _dataGridView, 
                btnAtualizar,
                btnFechar
            });
        }
        
        private void LoadData()
        {
            try
            {
                // Obter todos os técnicos
                var tecnicos = _usuarioRepository.GetByTipo(TipoUsuario.Tecnico);
                
                // Obter todos os chamados
                var chamados = _chamadoRepository.GetAll();
                
                // Criar relatório
                var relatorio = tecnicos.Select(tecnico =>
                {
                    var chamadosTecnico = chamados.Where(c => c.IdTecnico == tecnico.Id).ToList();
                    var pendentes = chamadosTecnico.Count(c => c.Status == StatusChamado.Pendente || c.Status == StatusChamado.EmAndamento);
                    var concluidos = chamadosTecnico.Count(c => c.Status == StatusChamado.Concluido);
                    var total = pendentes + concluidos;
                    
                    return new
                    {
                        Tecnico = tecnico.Nome,
                        Pendentes = pendentes,
                        Concluidos = concluidos,
                        Total = total,
                        Percentual = total > 0 ? Math.Round((double)concluidos / total * 100, 1) : 0.0
                    };
                }).OrderByDescending(x => x.Total).ToList();
                
                _dataGridView.DataSource = relatorio;
                
                // Configurar colunas
                if (_dataGridView.Columns.Count > 0)
                {
                    _dataGridView.Columns["Tecnico"].HeaderText = "Técnico";
                    _dataGridView.Columns["Pendentes"].HeaderText = "Pendentes";
                    _dataGridView.Columns["Concluidos"].HeaderText = "Concluídos";
                    _dataGridView.Columns["Total"].HeaderText = "Total";
                    _dataGridView.Columns["Percentual"].HeaderText = "% Conclusão";
                }
                
                // Adicionar cores condicionais
                foreach (DataGridViewRow row in _dataGridView.Rows)
                {
                    if (row.Cells["Percentual"].Value != null)
                    {
                        var percentual = Convert.ToDouble(row.Cells["Percentual"].Value);
                        
                        if (percentual >= 80)
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                        else if (percentual >= 60)
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                        else if (percentual > 0)
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar relatório: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
