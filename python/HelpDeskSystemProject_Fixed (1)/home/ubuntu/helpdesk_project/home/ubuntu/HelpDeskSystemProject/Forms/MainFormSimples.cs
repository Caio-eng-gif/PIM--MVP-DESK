using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDeskSystemFixed.Models;
using HelpDeskSystemFixed.Repositories;

namespace HelpDeskSystemFixed.Forms
{
    public partial class MainFormSimples : Form
    {
        private Label _lblStats;
        
        public MainFormSimples()
        {
            InitializeComponent();
            LoadDashboard();
        }
        
        private void InitializeComponent()
        {
            this.Text = "MVP DESK - Teste Dashboard";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            
            // Label para mostrar as estatísticas
            _lblStats = new Label
            {
                Text = "Carregando...",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(20, 20),
                Size = new Size(750, 500),
                BackColor = Color.White
            };
            
            this.Controls.Add(_lblStats);
            
            // Botão para recarregar
            var btnRecarregar = new Button
            {
                Text = "Recarregar Dashboard",
                Location = new Point(20, 530),
                Size = new Size(150, 30),
                BackColor = Color.LightBlue
            };
            btnRecarregar.Click += (s, e) => LoadDashboard();
            
            this.Controls.Add(btnRecarregar);
        }
        
        private void LoadDashboard()
        {
            try
            {
                _lblStats.Text = "🔄 Carregando estatísticas...";
                _lblStats.ForeColor = Color.Blue;
                Application.DoEvents();
                
                var chamadoRepo = new ChamadoRepository();
                var usuarioRepo = new UsuarioRepository();
                
                // Carregar dados
                var chamados = chamadoRepo.GetAll();
                var totalChamados = chamados?.Count ?? 0;
                var chamadosPendentes = chamados?.Count(c => c.Status == StatusChamado.Pendente) ?? 0;
                var chamadosConcluidos = chamados?.Count(c => c.Status == StatusChamado.Concluido) ?? 0;
                var chamadosAndamento = chamados?.Count(c => c.Status == StatusChamado.EmAndamento) ?? 0;
                
                var tecnicos = usuarioRepo.GetByTipo(TipoUsuario.Tecnico);
                var colaboradores = usuarioRepo.GetByTipo(TipoUsuario.Colaborador);
                var totalTecnicos = tecnicos?.Count ?? 0;
                var totalColaboradores = colaboradores?.Count ?? 0;
                
                // Calcular taxa de resolução
                var taxaResolucao = totalChamados > 0 ? 
                    Math.Round((double)chamadosConcluidos / totalChamados * 100, 1) : 0;
                
                // Atualizar interface
                _lblStats.Text = $@"📊 DASHBOARD - MVP DESK

🎫 CHAMADOS
   • Total: {totalChamados}
   • Pendentes: {chamadosPendentes}
   • Em Andamento: {chamadosAndamento}
   • Concluídos: {chamadosConcluidos}

👥 USUÁRIOS
   • Técnicos: {totalTecnicos}
   • Colaboradores: {totalColaboradores}
   • Total: {totalTecnicos + totalColaboradores}

📈 PERFORMANCE
   • Taxa de Resolução: {taxaResolucao}%
   • Média por Técnico: {(totalTecnicos > 0 ? Math.Round((double)totalChamados / totalTecnicos, 1) : 0)} chamados

🕒 Última Atualização: {DateTime.Now:dd/MM/yyyy HH:mm:ss}

✅ Sistema Operacional

DETALHES DOS CHAMADOS:";

                if (chamados != null && chamados.Count > 0)
                {
                    _lblStats.Text += "\n\n";
                    foreach (var chamado in chamados)
                    {
                        _lblStats.Text += $"\n• Chamado {chamado.Id}: {chamado.Descricao} (Status: {chamado.Status})";
                    }
                }
                else
                {
                    _lblStats.Text += "\n\nNenhum chamado encontrado.";
                }
                
                _lblStats.Text += "\n\nDETALHES DOS USUÁRIOS:";
                if (tecnicos != null && tecnicos.Count > 0)
                {
                    _lblStats.Text += "\n\nTécnicos:";
                    foreach (var tecnico in tecnicos)
                    {
                        _lblStats.Text += $"\n• {tecnico.Nome} ({tecnico.Email ?? "sem email"})";
                    }
                }
                
                if (colaboradores != null && colaboradores.Count > 0)
                {
                    _lblStats.Text += "\n\nColaboradores:";
                    foreach (var colaborador in colaboradores)
                    {
                        _lblStats.Text += $"\n• {colaborador.Nome} ({colaborador.Email ?? "sem email"})";
                    }
                }
                
                _lblStats.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                _lblStats.Text = $@"❌ ERRO NO DASHBOARD

Falha ao carregar estatísticas:
{ex.Message}

Tipo do erro: {ex.GetType().Name}

Stack Trace:
{ex.StackTrace}

🕒 {DateTime.Now:HH:mm:ss}";
                
                _lblStats.ForeColor = Color.Red;
            }
        }
    }
}
