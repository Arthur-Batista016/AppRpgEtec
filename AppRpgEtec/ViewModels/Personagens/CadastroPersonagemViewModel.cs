using AppRpgEtec.Models;
using AppRpgEtec.Models.Enuns;
using AppRpgEtec.Services.Personagens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Personagens
{
    [QueryProperty("PersonagemSelecionadoId", "pId")]
    public class CadastroPersonagemViewModel:BaseViewModel
    {

        private PersonagemService pService;

        public ICommand SalvarCommand { get; }
        public ICommand CancelarCommand { get; }

        public CadastroPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            _ = ObterClasses();

            SalvarCommand = new Command(async () => { await SalvarPersonagem(); });
            CancelarCommand = new Command(async () => CancelarCadastro());
        }

        private async void CancelarCadastro()
        {
            await Shell.Current.GoToAsync("..");
        }


        private int id;
        private string nome;
        private int pontosVida;
        private int forca;
        private int defesa;
        private int inteligencia;
        private int disputas;
        private int vitorias;
        private int derrotas;


        public int Id { get => id; set => id = value; }
        public string Nome { get => this.nome; set => this.nome = value; }
        public int PontosVida { get => this.pontosVida; set => this.pontosVida = value; }
        public int Forca { get => this.forca; set => this.forca = value; }
        public int Defesa { get => this.defesa; set => this.defesa = value; }
        public int Inteligencia { get => this.inteligencia; set => this.inteligencia = value; }
        public int Disputas { get => this.disputas; set => this.disputas = value; }
        public int Vitorias { get => this.vitorias; set => this.vitorias = value; }
        public int Derrotas { get => this.derrotas; set => this.derrotas = value; }
       
        
        
        
        
        private ObservableCollection<TipoClasse> listaTiposClasse;

        public ObservableCollection<TipoClasse> ListaTipoClasse
        {
            get { return listaTiposClasse; }
            set
            {
                if(value != null)
                {
                    listaTiposClasse = value;
                    onPropertyChanged();
                }
            }
        }


        public async Task ObterClasses()
        {
            try { 
            listaTiposClasse = new ObservableCollection<TipoClasse>();
            listaTiposClasse.Add(new TipoClasse() { Id = 1, Descricao = "CAVALEIRO" });
            listaTiposClasse.Add(new TipoClasse() { Id = 2, Descricao = "MAGO" });
            listaTiposClasse.Add(new TipoClasse() { Id = 3, Descricao = "CLERIGO" });
            onPropertyChanged(nameof(listaTiposClasse));
            }catch( Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok" );
            }
        }



        private TipoClasse tipoClasseSelecionado;

        public TipoClasse TipoClasseSelecionado
        {
            get { return tipoClasseSelecionado; }
            set
            {
                if (value != null)
                {
                   tipoClasseSelecionado = value;
                    onPropertyChanged();
                }
            }
        }

       

        public async Task SalvarPersonagem()
        {
            try
            {
                Personagem model = new Personagem()
                {
                    Nome = this.nome,
                    PontosVida = this.pontosVida,
                    Defesa = this.defesa,
                    Derrotas = this.derrotas,
                    Disputas = this.disputas,
                    Forca = this.forca,
                    Inteligencia = this.inteligencia,
                    Vitorias = this.vitorias,
                    Id = this.Id,
                    Classe = (ClasseEnum)tipoClasseSelecionado.Id

                };
                if (model.Id == 0)
                    await pService.PostPersonagemAsync(model);
                else
                    await pService.PutPersonagemAsync(model);

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados Salvos com sucesso!", "OK");

                await AppShell.Current.GoToAsync(".."); // REMOVE A PAGUNA ATUAL DA PILHA DE PAGINAS
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }


        public async void CarregarPersonagem()
        {
            try
            {
                Personagem p = await pService.GetPersonagemAsync(int.Parse(personagemSelecionadoId));

                this.nome = p.Nome;
                this.PontosVida = p.PontosVida;
                this.Defesa = p.Defesa;
                this.Derrotas = p.Derrotas;
                this.Disputas = p.Disputas;
                this.Forca = p.Forca;
                this.Inteligencia = p.Inteligencia;
                this.Vitorias = p.Vitorias;
                this.Id = p.Id;

                TipoClasseSelecionado = this.ListaTipoClasse.FirstOrDefault(tClasse => tClasse.Id == (int)p.Classe);
            } catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "OK");
            }
        }

        private string personagemSelecionadoId;

        public string PersonagemSelecionadoId 
        {
            set
            {
                if (value != null)
                {
                    personagemSelecionadoId = Uri.UnescapeDataString(value);
                    CarregarPersonagem();
                }
            }
        }

    }
}
