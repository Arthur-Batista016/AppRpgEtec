using AppRpgEtec.Models;
using AppRpgEtec.Services.Disputas;
using AppRpgEtec.Services.PersonagemHabilidades;
using AppRpgEtec.Services.Personagens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Disputas
{
    public class DisputaViewModel : BaseViewModel
    {
        private PersonagemService pServivce;
        public ObservableCollection<Personagem> PersonagensEncontrados { get; set; }

        public Personagem Atacante { get; set; }
        public Personagem Oponente { get; set; }
        private DisputaService dService;
        public Disputa DisputaPersonagens { get; set; }

        private PersonagemHabilidadeService phService;


        private Disputa DisputaPersonagem { get; set; }
        public ObservableCollection<PersonagemHabilidade> Habilidades { get; set; }

        public DisputaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            pServivce = new PersonagemService(token);
            phService = new PersonagemHabilidadeService(token);
            dService = new DisputaService(token);

            Atacante = new Personagem();
            Oponente = new Personagem();
            DisputaPersonagens = new Disputa();

            PersonagensEncontrados = new ObservableCollection<Personagem>();

            PesquisarPersonagensCommand = new Command<string>(async (string pesquisa) => { await PesquisarPersonagens(pesquisa); });

            DisputaComArmaCommand = new Command(async () => { await ExecutarDisputaArmada(); });

            DisputaComHabilidadeCommand = new Command(async () => { await ExecutarDisputaHabilidades(); });
        }

        public ICommand PesquisarPersonagensCommand { get; set; }

        public ICommand DisputaComHabilidadeCommand { get; set; }
        public ICommand DisputaComArmaCommand { get; set; }


        public string DescricaoPersonagemAtacante
        {
            get => Atacante.Nome;
        }

        public string DescricaoPersonagemOponente
        {
            get => Oponente.Nome;
        }



        private PersonagemHabilidade habilidadeSelecionada;

        public PersonagemHabilidade HabilidadeSelecionada
        {
            get { return habilidadeSelecionada; }
            set
            {
                if (value != null)
                {
                    try
                    {
                        habilidadeSelecionada = value;
                        OnPropertyChanged();
                    }
                    catch (Exception ex) {
                        Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "Ok");
                    }
                }
            }
        }


        public async Task PesquisarPersonagens(string textoPesquisaPersonagem)
        {
            try
            {
                PersonagensEncontrados = await pServivce.GetByNomeAproximado(textoPesquisaPersonagem);
                OnPropertyChanged(nameof(PersonagensEncontrados));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task SelecionarPersonagem(Personagem p)
        {
            try
            {
                string tipoCombatente = await Application.Current.MainPage.DisplayActionSheet("Atacante ou Oponente", "Cancelar", "", "Atacante", "Oponente");


                if (tipoCombatente == "Atacante")
                {
                    await this.ObterHabilidadesAsync(p.Id);
                    Atacante = p;
                    OnPropertyChanged(nameof(DescricaoPersonagemAtacante));
                }
                else if (tipoCombatente == "Oponente")
                {
                    Oponente = p;
                    OnPropertyChanged(nameof(DescricaoPersonagemOponente));
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }



        private Personagem personagemSelecionado;

        public Personagem PersonagemSelecionado
        {
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;
                    SelecionarPersonagem(personagemSelecionado);
                    OnPropertyChanged();
                    PersonagensEncontrados.Clear();
                }
            }
        }


        private string textoBuscaDigitado = string.Empty;
        public string TextoBuscaDigitado
        {
            get { return textoBuscaDigitado; }
            set
            {
                if (value != null && !string.IsNullOrEmpty(value) && value.Length > 0)
                {
                    textoBuscaDigitado = value;
                    _ = PesquisarPersonagens(textoBuscaDigitado);
                }
                else
                {
                    PersonagensEncontrados.Clear();
                }
            }
        }




        public async Task ObterHabilidadesAsync(int personagemId)
        {
            try
            {
                Habilidades = await phService.GetPersonagemHabilidadesAsync(personagemId);
                OnPropertyChanged(nameof(Habilidades));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExecutarDisputaHabilidades()
        {
            try
            {

                DisputaPersonagem.AtacanteId = Atacante.Id;
                DisputaPersonagem.OponenteId = Oponente.Id;
                DisputaPersonagem.HabilidadeId = HabilidadeSelecionada.HabilidadeId;
                DisputaPersonagem = await dService.PostDisputaComHabilidadesAsync(DisputaPersonagem);


                await Application.Current.MainPage.DisplayAlert("Resultado", DisputaPersonagem.Narracao, "Ok");
            }
            catch (Exception ex) {

                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }


        }

        private async Task ExecutarDisputaArmada()
        {
            try
            {
                DisputaPersonagens.AtacanteId = Atacante.Id;
                DisputaPersonagens.OponenteId = Oponente.Id;
                DisputaPersonagens = await dService.PostDisputaComArmaAsync(DisputaPersonagens);

                await Application.Current.MainPage
                    .DisplayAlert("Resultado", DisputaPersonagens.Narracao, "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }




        private async Task ExecutarDisputaArmada()
        {
            try{
                ObservableCollection<Personagem> lista = await pServivce.GetPersonagensAsync();
                DisputaPersonagens.ListaIdPersonagens = lista.Select(x => x.Id).ToList();


                DisputaPersonagens = await dService.PostDisputaGeralAsync(DisputaPersonagens);

                string resultados = string.Join("|", DisputaPersonagens.Resultados);

                await Application.Current.MainPage.DisplayAlert("Resultado", resultados, "Ok");


            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");

            }

        }
}
