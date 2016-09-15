using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace premierTest
{
    partial class Utilisateurs
    {
        HillthDataClassDataContext HDB = new HillthDataClassDataContext();
        private string idUtilisateur;
        private string mdpUtilisateur;

        public string IDUser
        {
            get { return idUtilisateur; }
            set { idUtilisateur = value; }
        }

        public string MdpUtilisateur
        {
            get { return mdpUtilisateur; }
            set { mdpUtilisateur = value; }
        }

        /* Constructeur */
        public Utilisateurs(string iduser, string mdpuser)
        {
            idUtilisateur   = iduser;
            mdpUtilisateur  = mdpuser;
        }

        /*  Fonctions */
        // Fonction permettant de créer un utilisateur
        public void CreerUtilisateur()
        {
            try
            {
                //Appel de la dataclass (base de données)
                using (HillthDataClassDataContext HDB = new HillthDataClassDataContext())
                {
                    // Insertion dans la DC via sproc
                    HDB.InsertUtilisateur(idUtilisateur, mdpUtilisateur);
                    // Insere dans la DB
                    HDB.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur : " + e.Message);
            }
        }

        // Fonction veirifant si les données entrée pour se connecter sont correctes
        public static bool LoginUser(string username, string pass)
        {
            bool isOk = false;
            HillthDataClassDataContext HDB = new HillthDataClassDataContext();
            // Verifie si les logs sont corrects.
            if (HDB.AuthentificationUser(username, pass).Any())
            {
                isOk = true;
            }
            return isOk;
        }
    }
}
