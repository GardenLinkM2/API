namespace Union.Backend.Model.Models
{
    public class Contact : UniqueEntity
        //TODO: vérifier si c'est bon (je suis pas sur des liens et de l'utilité de ContactAsk)
    {
        public bool Accept { get; set; }
        public User UserOne { get; set; }
        public User UserTwo { get; set; }
        public ContactAsk Ask { get; set; }
    }
}
