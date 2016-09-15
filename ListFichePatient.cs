using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace premierTest
{
    class ListFichePatient
    {
        // Liste contenant tout les patients. 
        List<FichePatient> listFichePat = new List<FichePatient>();

        // Ajoute un patient à la liste
        public void AjouterPatient(FichePatient newFP)
        {
            listFichePat.Add(newFP);
        }

        // Supprimer un patient de la liste quand celui-ci est supprimé de la base de données
        public void SupprimerPatient(FichePatient oldFP)
        {
            listFichePat.Remove(oldFP);
        }
    }
}
