using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace premierTest
{
    class ListMedecin
    {
        // Liste contenant les médecins
        List<Medecin> listMedecin = new List<Medecin>();

        // Fonction ajoutant un médecin à la liste
        public void AjouterMedecin(Medecin newM)
        {
            listMedecin.Add(newM);
        }
    }
}
