using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDeskSystemFixed.Models;
using HelpDeskSystemFixed.Repositories;

namespace HelpDeskSystemFixed.Forms
{
    public partial class ChamadosForm : Form
    {
        private readonly ChamadoRepository _chamadoRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private DataGridView _dataGridView;
        private ComboBox _comboTecnicos;
        private Button _btnAlterarTecnico;
        private Button _btnExcluir;
        private Button _btnAtualizar;
        
	        public ChamadosForm()
	        {
	            _chamadoRepository = new ChamadoRepository();
	            _usuarioRepository = new UsuarioRepository();
	            
	            // Inicialização manual dos componentes para evitar dependência do designer
	            this.Text = "Gerenciar Chamados";
	            this.Size = new Size(1000, 600);
	            this.StartPosition = FormStartPosition.CenterParent;
	            
	            // Título
	            var lblTitulo = new Label
	            {
	                Text = "Gerenciamento de Chamados",
	                Font = new Font("Arial", 16, FontStyle.Bold),
	                Location = new Point(20, 20),
	                AutoSize = true,
	                ForeColor = Color.DarkBlue
	            };
	            
	            // DataGridView para exibir chamados
	            _dataGridView = new DataGridView
	            {
	                Location = new Point(20, 60),
	                Size = new Size(950, 350),
	                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
	                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
	                MultiSelect = false,
	                ReadOnly = true,
	                AllowUserToAddRows = false
	            };
	            
	            // ComboBox para seleção de técnicos
	            var lblTecnico = new Label
	            {
	                Text = "Alterar Técnico:",
	                Location = new Point(20, 430),
	                AutoSize = true,
	                Font = new Font("Arial", 10, FontStyle.Bold)
	            };
	            
	            _comboTecnicos = new ComboBox
	            {
	                Location = new Point(130, 427),
	                Size = new Size(200, 23),
	                DropDownStyle = ComboBoxStyle.DropDownList
	            };
	            
	            // Botões
	            _btnAlterarTecnico = new Button
	            {
	                Text = "Alterar Técnico",
	                Location = new Point(350, 427),
	                Size = new Size(120, 30),
	                BackColor = Color.LightBlue
	            };
	            _btnAlterarTecnico.Click += OnAlterarTecnico;
	            
	            _btnExcluir = new Button
	            {
	                Text = "Excluir Chamado",
	                Location = new Point(490, 427),
	                Size = new Size(120, 30),
	                BackColor = Color.LightCoral
	            };
	            _btnExcluir.Click += OnExcluirChamado;
	            
	            var _btnFinalizar = new Button
	            {
	                Text = "Finalizar Chamado",
	                Location = new Point(630, 427),
	                Size = new Size(120, 30),
	                BackColor = Color.LightGreen
	            };
	            _btnFinalizar.Click += OnFinalizarChamado;
	            
	            _btnAtualizar = new Button
	            {
	                Text = "Atualizar Lista",
	                Location = new Point(770, 427),
	                Size = new Size(120, 30),
	                BackColor = Color.LightGray
	            };
	            _btnAtualizar.Click += OnAtualizar;
	            
	            var btnFechar = new Button
	            {
	                Text = "Fechar",
	                Location = new Point(900, 427), // Ajustei a posição para caber
	                Size = new Size(80, 30)
	            };
	            btnFechar.Click += (s, e) => this.Close();
	            
	            // Adicionar controles ao formulário
	            this.Controls.AddRange(new Control[] 
	            { 
	                lblTitulo,
	                _dataGridView, 
	                lblTecnico, 
	                _comboTecnicos, 
	                _btnAlterarTecnico, 
	                _btnExcluir, 
	                _btnFinalizar, // Adicionado
	                _btnAtualizar,
	                btnFechar
	            });
	            
	            LoadData();
	        }
        
        private void LoadData()
        {
            try
            {
                // Carregar chamados
                var chamados = _chamadoRepository.GetAll();
                var dataSource = chamados.Select(c => new
                {
                    Id = c.Id,
                    Colaborador = c.Colaborador?.Nome ?? "N/A",
                    Tecnico = c.Tecnico?.Nome ?? "Não atribuído",
                    Descricao = c.Descricao,
                    Status = GetStatusText(c.Status),
                    DataAbertura = c.DataAbertura.ToString("dd/MM/yyyy HH:mm"),
                    DataConclusao = c.DataConclusao?.ToString("dd/MM/yyyy HH:mm") ?? "N/A"
                }).ToList();
                
                _dataGridView.DataSource = dataSource;
                
                // Configurar colunas
                if (_dataGridView.Columns.Count > 0)
                {
                    _dataGridView.Columns["Id"].HeaderText = "ID";
                    _dataGridView.Columns["Id"].Width = 50;
                    _dataGridView.Columns["Colaborador"].HeaderText = "Colaborador";
                    _dataGridView.Columns["Tecnico"].HeaderText = "Técnico";
                    _dataGridView.Columns["Descricao"].HeaderText = "Descrição";
                    _dataGridView.Columns["Status"].HeaderText = "Status";
                    _dataGridView.Columns["DataAbertura"].HeaderText = "Data Abertura";
                    _dataGridView.Columns["DataConclusao"].HeaderText = "Data Conclusão";
                }
                
                // Carregar técnicos no ComboBox
                var tecnicos = _usuarioRepository.GetByTipo(TipoUsuario.Tecnico);
                _comboTecnicos.DataSource = tecnicos;
                _comboTecnicos.DisplayMember = "Nome";
                _comboTecnicos.ValueMember = "Id";
                
                if (tecnicos.Count > 0)
                    _comboTecnicos.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private string GetStatusText(StatusChamado status)
        {
            return status switch
            {
                StatusChamado.Pendente => "Pendente",
                StatusChamado.EmAndamento => "Em Andamento",
                StatusChamado.Concluido => "Concluído",
                _ => "Desconhecido"
            };
        }
        
        private void OnAlterarTecnico(object sender, EventArgs e)
        {
            if (_dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um chamado para alterar o técnico.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (_comboTecnicos.SelectedValue == null)
            {
                MessageBox.Show("Selecione um técnico.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                var selectedRow = _dataGridView.SelectedRows[0];
                var chamadoId = (int)selectedRow.Cells["Id"].Value;
                var novoTecnicoId = (int)_comboTecnicos.SelectedValue;
                
                var chamado = _chamadoRepository.GetById(chamadoId);
                if (chamado != null)
                {
                    chamado.IdTecnico = novoTecnicoId;
                    if (chamado.Status == StatusChamado.Pendente)
                        chamado.Status = StatusChamado.EmAndamento;
                    
                    if (_chamadoRepository.Update(chamado))
                    {
                        MessageBox.Show("Técnico alterado com sucesso!", "Sucesso", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Erro ao alterar técnico.", "Erro", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao alterar técnico: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
	        private void OnFinalizarChamado(object sender, EventArgs e)
	        {
	            if (_dataGridView.SelectedRows.Count == 0)
	            {
	                MessageBox.Show("Selecione um chamado para finalizar.", "Aviso", 
	                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
	                return;
	            }
	            
	            var selectedRow = _dataGridView.SelectedRows[0];
	            var chamadoId = (int)selectedRow.Cells["Id"].Value;
	            var tecnicoId = (int)_comboTecnicos.SelectedValue; // Assume que o técnico logado é o selecionado
	            
	            var result = MessageBox.Show("Tem certeza que deseja finalizar este chamado? O status será alterado para 'Concluído'.", "Confirmação", 
	                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
	            
	            if (result == DialogResult.Yes)
	            {
	                try
	                {
	                    if (_chamadoRepository.FinalizarChamado(chamadoId, tecnicoId))
	                    {
	                        MessageBox.Show("Chamado finalizado com sucesso!", "Sucesso", 
	                            MessageBoxButtons.OK, MessageBoxIcon.Information);
	                        LoadData();
	                    }
	                    else
	                    {
	                        MessageBox.Show("Erro ao finalizar chamado. Verifique se ele já não está concluído.", "Erro", 
	                            MessageBoxButtons.OK, MessageBoxIcon.Error);
	                    }
	                }
	                catch (Exception ex)
	                {
	                    MessageBox.Show($"Erro ao finalizar chamado: {ex.Message}", "Erro", 
	                        MessageBoxButtons.OK, MessageBoxIcon.Error);
	                }
	            }
	        }
	        
	        private void OnExcluirChamado(object sender, EventArgs e)
	        {
	            if (_dataGridView.SelectedRows.Count == 0)
	            {
	                MessageBox.Show("Selecione um chamado para excluir.", "Aviso", 
	                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
	                return;
	            }
	            
	            var result = MessageBox.Show("Tem certeza que deseja excluir este chamado?", "Confirmação", 
	                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
	            
	            if (result == DialogResult.Yes)
	            {
	                try
	                {
	                    var selectedRow = _dataGridView.SelectedRows[0];
	                    var chamadoId = (int)selectedRow.Cells["Id"].Value;
	                    
	                    if (_chamadoRepository.Delete(chamadoId))
	                    {
	                        MessageBox.Show("Chamado excluído com sucesso!", "Sucesso", 
	                            MessageBoxButtons.OK, MessageBoxIcon.Information);
	                        LoadData();
	                    }
	                    else
	                    {
	                        MessageBox.Show("Erro ao excluir chamado.", "Erro", 
	                            MessageBoxButtons.OK, MessageBoxIcon.Error);
	                    }
	                }
	                catch (Exception ex)
	                {
	                    MessageBox.Show($"Erro ao excluir chamado: {ex.Message}", "Erro", 
	                        MessageBoxButtons.OK, MessageBoxIcon.Error);
	                }
	            }
	        }
        
        private void OnAtualizar(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
