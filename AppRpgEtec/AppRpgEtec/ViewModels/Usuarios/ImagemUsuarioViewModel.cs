using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Usuarios;
using Azure.Storage.Blobs;

namespace AppRpgEtec.ViewModels.Usuarios
{
    public class ImagemUsuarioViewModel : BaseViewModel
    {
        private UsuarioService uService;
        private static string conexaoAzureStorage = "DefaultEndpointsProtocol=https;AccountName=rpgapistorage;AccountKey=2oSyHIfSq2Pw3zcjOsGE0oI7DQV4PIdzT2wBIQ3Lncvj4E2FRvSd3t3pPVwMxlz0pfk1lUZVc6CZ+AStOFENCw==;EndpointSuffix=core.windows.net";
        private static string container = "arquivos";//nome do container criado

        public ImagemUsuarioViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);

            SalvarImagemCommand = new Command(SalvarImagemAzure);
            FotografarCommand = new Command(async () => await Fotografar());
            AbrirGaleriaCommand = new Command(async () => await AbrirGaleria());

            CarregarUsuarioAzure();
        }

        public ICommand FotografarCommand { get; }
        public ICommand SalvarImagemCommand { get; }
        public ICommand AbrirGaleriaCommand { get; }

        private ImageSource fonteImagem;
        public ImageSource FontImagem
        {
            get { return fonteImagem; }
            set
            {
                fonteImagem = value;
                OnPropertyChanged();
            }
        }

        private byte[] foto;

        public ImageSource FonteImagem
        {
            get { return fonteImagem; }
            set
            {
                fonteImagem = value;
                OnPropertyChanged();
            }
        }

        public byte[] Foto
        {
            get { return foto; }
            set
            {
                foto = value;
                OnPropertyChanged();
            }
        }


        // preferível: async Task ao invés de async void
        public async Task AbrirGaleria()
        {
            try
            {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo == null)
                {
                    // usuário cancelou — nada a fazer
                    return;
                }

                // leia os bytes dentro de um using (stream original será fechado)
                byte[] fileBytes;
                using (var sourceStream = await photo.OpenReadAsync())
                using (var ms = new MemoryStream())
                {
                    await sourceStream.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                // guarde na propriedade (você já tem a propriedade Foto como byte[])
                Foto = fileBytes;

                // crie a ImageSource com uma factory que gera um MemoryStream novo a partir dos bytes
                FonteImagem = ImageSource.FromStream(() => new MemoryStream(fileBytes));

                // notifique propriedades (supondo que FonteImagem tem setter com OnPropertyChanged)
            }
            catch (Exception ex)
            {
                // ex.InnerException pode ser null
                string details = ex.InnerException?.Message ?? "";
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + details, "Ok");
            }
        }



        public async void SalvarImagemAzure()
        {
            try
            {
                Usuario u = new Usuario();
                u.Foto = foto;
                u.Id = Preferences.Get("UsuarioId", 0);

                string filename = $"{u.Id}.jpg";

                var blobClient = new BlobClient(conexaoAzureStorage, container, filename);


                if (blobClient.Exists())
                    blobClient.Delete();

                using (var stream = new MemoryStream(foto))
                {
                    blobClient.Upload(stream, overwrite: true);
                }


            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");

            }


        }



        public async Task Fotografar()
        {
            try
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo == null)
                    return;

                byte[] fileBytes;
                using (var sourceStream = await photo.OpenReadAsync())
                using (var ms = new MemoryStream())
                {
                    await sourceStream.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                Foto = fileBytes;

                // ✅ Cria uma nova instância de stream a cada acesso
                FonteImagem = ImageSource.FromStream(() => new MemoryStream(fileBytes));
            }
            catch (Exception ex)
            {
                string details = ex.InnerException?.Message ?? "";
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + " Detalhes: " + details, "Ok");
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
                    byte[] fileBytes;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        blobClient.OpenRead().CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    Foto = fileBytes;
                }


            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");

            }



        }
    }
}
