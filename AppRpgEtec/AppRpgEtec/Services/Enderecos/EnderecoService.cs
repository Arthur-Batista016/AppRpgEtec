using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppRpgEtec.Services.Enderecos
{
    internal class EnderecoService:Request
    {
        private readonly Request _request;
        
   

      
        public EnderecoService()
        {
            _request = new Request();

            
        }


        public async Task<Models.Endereco> GetEnderecoByCep(string cep)
        {
            const string urlBase = "https://nominatim.openstreetmap.org/search?format=json&q=02110-010";
            var endereco = await _request.GetAsync<Models.Endereco>(urlBase,"");
            return endereco;

        }

        
    }
}
