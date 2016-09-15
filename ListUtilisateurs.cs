using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace premierTest
{
    class ListUtilisateurs
    {
        // Liste contenant les utilisateurs
        List<Utilisateurs> listUtilisateurs = new List<Utilisateurs>();

        // Fonction ajoutant un utilisateur à la liste
        public void AjouterUtilisateur(Utilisateurs newU)
        {
            listUtilisateurs.Add(newU);
        }
    }
}
