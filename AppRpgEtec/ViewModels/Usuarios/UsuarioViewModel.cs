using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class UsuarioViewModel:BaseViewModel
    {
        private UsuarioService uService;

        public Icommand AutenticarCommand { get; set; }

        public UsuarioViewModel()
        {
            uService = new UsuarioService();
            InicializarCommands();
        }

        public void InicializarCommands()
        {
            AutenticarCommand = new Command(async () => await AutenticarUsuario());

        }


        private string login = string.Empty;


        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                onPropertyChanged();
            }
        }

        private string senha = string.Empty;

        public string Senha
        {
            get { return senha; }
            set
            {
                senha = value;
                onPropertyChanged();
            }
        }

        public async Task AutenticarUsuario()
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = Login;

                Usuario uAutenticado = await uService.PostAutenticarUsuarioAsync(u);

                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem-Vindo(a) {uAutenticado.Username}.";

                    //Guardando dados do usuario para uso futuro
                    Preferences.Set("UsuarioId",uAutenticado.Id);
                    Preferences.Set("UsuarioUsername", uAutenticado.Username);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);
                    Preferences.Set("UsuarioToken", uAutenticado.Token);

                    await MainApplication.Current.MainPage.DisplayAlert("Informação", mansagem, "OK");

                    Application.Current.MainPage = new MainPage();


                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Informação", "Dados Incorretos :(", "OK");

                }

            }
            catch(Exception ex)
            {
                await Application.Current.MainPage
                .DisplayAlert("Informação",ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

    }
}
