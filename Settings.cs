/*toda vez que geramos um token na aplicação precisamos de uma chave privada 
usada para desencriptar o token*/

namespace webapi{
    public static class Settings{
        public static string Secret = "inserindoqualquerchave123";
    }
}