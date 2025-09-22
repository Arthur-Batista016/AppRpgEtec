using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AppRpgEtec.Services.Usuarios;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Enderecos;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Usuarios
{
    partial class LocalizaçãoViewModel:BaseViewModel
    {
        private UsuarioService uService;

        private EnderecoService enderecoService;

        public LocalizaçãoViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);
            enderecoService = new EnderecoService();
            GetEnderecoCommand = new Command(async () => await getEnderecoByCep());
        }

        public ICommand GetEnderecoCommand { get; set; }

        private string cep;
        public string Cep
        {
            get => cep;
            set
            {
                if (value != null)
                {
                    cep = value;
                    OnPropertyChanged();
                }

            }
        }

        private Map meuMapa;
        public Map MeuMapa
        {
            get => meuMapa;
            set
            {
                if (value != null)
                {
                    meuMapa = value;
                    OnPropertyChanged();
                }

            }
        }

        public async void InicializarMapa()
        {
            try
            {
                Location location = new Location(-23.5200241d, -46.596498d);
                Pin pinEtec = new Pin()
                {
                    Type = PinType.Place,
                    Label = "Etec Horácio",
                    Address = "Rua Alcântaria, 113, Vila Guilherme",
                    Location = location
                };

                Map map = new Map();
                MapSpan mapSpan = MapSpan
                    .FromCenterAndRadius(location, Distance.FromKilometers(5));
                map.Pins.Add(pinEtec);
                map.MoveToRegion(mapSpan);

                MeuMapa = map;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        public async void ExibirUsuariosNoMapa()
        {
            try
            {
                ObservableCollection<Usuario> ocUsuarios = await uService.GetUsuariosAsync();
                List<Usuario> listaUsuarios = new List<Usuario>(ocUsuarios);
                Map map = new Map();
                foreach (Usuario u in listaUsuarios)
                {
                    if (u.Latitude != null && u.Longitude != null)
                    {
                        double latitude = (double)u.Latitude;
                        double longitude = (double)u.Longitude;
                        Location location = new Location(latitude, longitude);

                        Pin pinAtual = new Pin()
                        {
                            Type = PinType.Place,
                            Label = u.Username,
                            Address = $"E-mail: {u.Email}",
                            Location = location
                        };
                        map.Pins.Add(pinAtual);
                    }
                }
                MeuMapa = map;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        public async Task getEnderecoByCep()
        {
            Endereco endereco = new Endereco();
            try 
            {
                endereco = await enderecoService.GetEnderecoByCep(cep);
                OnPropertyChanged(nameof(endereco));                       
            }
            catch (Exception ex)
            {
                
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}
