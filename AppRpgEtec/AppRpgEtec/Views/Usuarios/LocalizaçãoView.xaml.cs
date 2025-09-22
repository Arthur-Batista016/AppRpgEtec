using AppRpgEtec.ViewModels.Usuarios;

namespace AppRpgEtec.Views.Usuarios;

public partial class LocalizaçãoView : ContentPage
{
	LocalizaçãoViewModel viewModel;
	public LocalizaçãoView()
	{
		InitializeComponent();

		viewModel = new LocalizaçãoViewModel();
		//viewModel.InicializarMapa();

		BindingContext = viewModel;
		viewModel.ExibirUsuariosNoMapa();
	}
}