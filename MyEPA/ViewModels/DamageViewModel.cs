using MyEPA.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class DamageViewModel : DamageJoinModel
    {
        public FileDataModel ImageFile { get; set; }
        public FileDataModel File { get; set; }

        public List<int> InputIncineratorIds { get; set; }
        public List<int> InputLandfillIds { get; set; }
        [DisplayName("災害主題")]
        public string DiasterName { get; set; }
    }
}