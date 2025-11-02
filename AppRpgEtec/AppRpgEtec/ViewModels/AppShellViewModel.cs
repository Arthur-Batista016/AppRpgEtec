
using System.ComponentModel;
using AppRpgEtec.Services.Usuarios;
using Azure.Storage.Blobs;

namespace AppRpgEtec.ViewModels;

public class AppShellViewModel : BaseViewModel
{
    private UsuarioService uService;
    private static string conexaoAzureStorage = "DefaultEndpointsProtocol=https;AccountName=rpgapistorage;AccountKey=2oSyHIfSq2Pw3zcjOsGE0oI7DQV4PIdzT2wBIQ3Lncvj4E2FRvSd3t3pPVwMxlz0pfk1lUZVc6CZ+AStOFENCw==;EndpointSuffix=core.windows.net";
    private static string container = "arquivos";//nome do container criado
    public AppShellViewModel()
    {
        string token = Preferences.Get("UsuarioToken", string.Empty);
        uService = new UsuarioService(token);
        CarregarUsuarioAzure();
    }

    private byte[] foto;
    public byte[] Foto
    {
        get => foto;
        set
        {
            foto = value;
            OnPropertyChanged();
        }
    }

    public async void CarregarUsuarioAzure()
    {
        try
        {
            int usuarioId = Preferences.Get("UsuarioId", 0);
            string filename = $"{usuarioId}.jpg";
            var blobClient = new BlobClient(conexaoAzureStorage, container, filename);

            if (blobClient.Exists())
            {
                Byte[] fileBytes;

                using (MemoryStream ms = new MemoryStream())
                {
                    blobClient.OpenRead().CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }



        }
        catch (Exception ex)
        {
            await Application.Current.MainPage
                .DisplayAlert("Ops", ex.Message + "Detelhes: " + ex.InnerException, "Ok");
        }
    }
}
