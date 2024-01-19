using Shopwise.Entities;

namespace Shopwise.ViewModels
{
    public class FooterVM
    {
        public Dictionary<string,string> Settings{ get; set; }
        public List<Service> Services{ get; set; }
    }
}
