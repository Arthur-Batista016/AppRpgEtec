using AppRpgEtec.ViewModels.Usuarios;

namespace AppRpgEtec.Views;

public partial class LoginView : ContentPage
{
	UsuarioViewModel usuarioViewModel;
	public LoginView()
	{
		InitializeComponent();
	
		usuarioViewModel = new UsuarioViewModel();
		BindingContext = usuarioViewModel;
	}
}