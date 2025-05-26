using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Personagens;

namespace AppRpgEtec.ViewModels.Personagens
{
    public class ListagemPersonagemViewModel: BaseViewModel
    {
        private PersonagemService pService;

        public ObservableCollection<Personagem> personagens {  get; set; }

        public ListagemPersonagemViewModel()
        {
            string token = Preferences.Get("UsuarioToken",string.Empty);
            pService =new PersonagemService(token);
            Personagens = new ObservableCollection<Personagem>();
        }
    }
}
