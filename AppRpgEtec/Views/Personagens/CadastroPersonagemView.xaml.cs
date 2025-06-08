namespace AppRpgEtec.Views.Personagens;

public partial class CadastroPersonagemView : ContentPage
{

	private CadastroPersonagemView cadViewModel;
	public CadastroPersonagemView()
	{
		InitializeComponent();

		cadViewModel = new CadastroPersonagemView();
		BindingContext = cadViewModel;
		Title = "Novo Personagem";
	}
}