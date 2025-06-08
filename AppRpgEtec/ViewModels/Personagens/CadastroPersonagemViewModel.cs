using AppRpgEtec.Models;
using AppRpgEtec.Models.Enuns;
using AppRpgEtec.Services.Personagens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.ViewModels.Personagens
{
    public class CadastroPersonagemViewModel:BaseViewModel
    {

        private PersonagemService pService;

        public Icommand SalvarCommand { get; }
        public Icommand CancelarCommand { get; }

        public CadastroPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pService = new PersonagemService(token);
            _ = ObterClasses();

            SalvarCommand = new Command(async () => { await SalvarPersonagem(); });
            CancelarCommand = new Command(async () => { await CancelarCadastro(); });
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

        public int Id
        {
            get => id;
            set
            {
                id = value;
                onPropertyChanged();
            }
        }


        public string nome
        {
            get => id;
            set
            {
                id = value;
                onPropertyChanged();
            }
        }

        public int pontosVida
        {
            get => id;
            set
            {
                id = value;
                onPropertyChanged();
            }
        }

        public int forca
        {
            get => id;
            set
            {
                id = value;
                onPropertyChanged();
            }
        }

        public int defesa
        {
            get => id;
            set
            {
                id = value;
                onPropertyChanged();
            }
        }

        public int inteligencia
        {
            get => id;
            set
            {
                id = value;
                onPropertyChanged();
            }
        }


        public int disputas
        {
            get => id;
            set
            {
                id = value;
                onPropertyChanged();
            }
        }

        public int vitorias
        {
            get => id;
            set
            {
                id = value;
                onPropertyChanged();
            }
        }

        public int derrotas
        {
            get => id;
            set
            {
                id = value;
                onPropertyChanged();
            }
        }


        private ObservableCollection<TipoClasse> listaTipoClasse;

        public ObservableCollection<TipoClasse> ListaTiposClasse
        {
            get { return listaTiposClasse; }
            set
            {
                if(value != null)
                {
                    listaTipoClasse = value;
                    onPropertyChanged();
                }
            }
        }


        public async Task ObterClasses()
        {
            try { 
            ListaTiposClasse = new ObservableCollection<TipoClasse>();
            ListaTiposClasse.Add(new TipoClasse() { Id = 1, Descricao = "CAVALEIRO" });
            ListaTiposClasse.Add(new TipoClasse() { Id = 2, Descricao = "MAGO" });
            ListaTiposClasse.Add(new TipoClasse() { Id = 3, Descricao = "CLERIGO" });
            onPropertyChanged(nameof(ListaTiposClasse));
            }catch( Exception ex)
            {
                await MainApplication.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok" );
            }
        }



        private TipoClasse tipoClasseSelecionado;

        public TipoClasse tipoClasseSelecionad
        {
            get { return listaTiposClasse; }
            set
            {
                if (value != null)
                {
                    listaTipoClasse = value;
                    onPropertyChanged();
                }
            }
        }


        public async Task ObterClasses()
        {
            try
            {
                Personagem model = new Personagem()
                {
                    Nome = this.nome,
                    pontosVida = this.pontosVida,
                    Defesa = this.defesa,
                    Derrotas = this.derrotas,
                    Disputas = this.disputas,
                    Forca = this.forca,
                    Inteligencia = this.inteligencia,
                    Vitorias = this.vitorias,
                    Id = this.id,
                    Classe = (ClasseEnum)tipoClasseSelecionad.Id

                };
                if (model.Id == 0)
                    await pService.PostPersonagemAsync(model);

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados Salvos com sucesso!", "OK");

                await AppShell.Current.GoToAsync(".."); // REMOVE A PAGUNA ATUAL DA PILHA DE PAGINAS
            }
            catch (Exception ex)
            {
                await MainApplication.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }






    }
}
