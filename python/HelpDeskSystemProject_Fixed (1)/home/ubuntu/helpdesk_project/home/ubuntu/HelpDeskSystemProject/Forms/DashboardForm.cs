using System;
using System.Drawing;
using System.Windows.Forms;
using HelpDeskSystemFixed.Models;
using HelpDeskSystemFixed.Repositories;

namespace HelpDeskSystemFixed.Forms
{
    public partial class DashboardForm : Form
    {
        private readonly Usuario _usuarioLogado;
        private Label _lblStats;

        public DashboardForm(Usuario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
            
            // Inicialização manual dos componentes para evitar dependência do designer
            this.Text = "Dashboard MVP DESK";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            _lblStats = new Label
            {
                Text = "Carregando estatísticas...",
                Font = new Font("Consolas", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                Padding = new Padding(20)
            };

            this.Controls.Add(_lblStats);
            
            this.Load += (s, e) => LoadDashboard();
        }

        private void LoadDashboard()
        {
            try
            {
                _lblStats.Text = "🔄 Carregando Dashboard MVP DESK...";
                
                var chamadoRepo = new ChamadoRepository();
                var usuarioRepo = new UsuarioRepository();
                
                var chamados = chamadoRepo.GetAll();
                var usuarios = usuarioRepo.GetAll();
                
                var totalChamados = chamados?.Count ?? 0;
                var totalUsuarios = usuarios?.Count ?? 0;
                var chamadosPendentes = 0;
                var chamadosConcluidos = 0;
                
                if (chamados != null)
                {
                    foreach (var chamado in chamados)
                    {
                        if (chamado.Status == StatusChamado.Pendente)
                            chamadosPendentes++;
                        else if (chamado.Status == StatusChamado.Concluido)
                            chamadosConcluidos++;
                    }
                }
                
                var tecnicos = 0;
                var colaboradores = 0;
                if (usuarios != null)
                {
                    foreach (var usuario in usuarios)
                    {
                        if (usuario.Tipo == TipoUsuario.Tecnico)
                            tecnicos++;
                        else if (usuario.Tipo == TipoUsuario.Colaborador)
                            colaboradores++;
                    }
                }
                
                var taxaResolucao = totalChamados > 0 ? 
                    Math.Round((double)chamadosConcluidos / totalChamados * 100, 1) : 0;
                
                _lblStats.Text = $@"📊 DASHBOARD MVP DESK

👤 USUÁRIO LOGADO: {_usuarioLogado.Nome} ({_usuarioLogado.Tipo})
📧 Email: {_usuarioLogado.Email ?? "Não informado"}

🎫 CHAMADOS
   • Total: {totalChamados}
   • Pendentes: {chamadosPendentes}
   • Concluídos: {chamadosConcluidos}
   • Em Andamento: {totalChamados - chamadosPendentes - chamadosConcluidos}

👥 USUÁRIOS
   • Técnicos: {tecnicos}
   • Colaboradores: {colaboradores}
   • Total: {totalUsuarios}

📈 PERFORMANCE
   • Taxa de Resolução: {taxaResolucao}%
   • Média por Técnico: {(tecnicos > 0 ? Math.Round((double)totalChamados / tecnicos, 1) : 0)} chamados

🕒 Última Atualização: {DateTime.Now:dd/MM/yyyy HH:mm:ss}

✅ Sistema Operacional - Todos os módulos funcionando";
                
                _lblStats.ForeColor = Color.FromArgb(0, 128, 0); // Verde
            }
            catch (Exception ex)
            {
                _lblStats.Text = $@"❌ ERRO NO DASHBOARD

Falha ao carregar estatísticas:
{ex.Message}

🔧 Possíveis soluções:
• Verificar string de conexão no DatabaseContext.cs
• Verificar se o banco de dados HelpDeskDB existe
• Verificar se as tabelas Usuarios e Chamados foram criadas

🕒 {DateTime.Now:HH:mm:ss}";
                
                _lblStats.ForeColor = Color.FromArgb(255, 0, 0); // Vermelho
            }
        }
    }
}

