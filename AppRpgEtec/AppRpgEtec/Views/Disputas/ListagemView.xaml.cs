using AppRpgEtec.ViewModels.Disputas;

namespace AppRpgEtec.Views.Disputas;

public partial class ListagemView : ContentPage
{
	DisputaViewModel ViewModel;
	public ListagemView()
	{
		InitializeComponent();

		ViewModel = new DisputaViewModel();
		BindingContext = ViewModel;
	}
}