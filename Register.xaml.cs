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
    /// <summary>
    /// Logique d'interaction pour Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        /* Verifictation entier, object car on ne connait pas le type */
        private void verifChiffre_PreviewKeyDown(object sender, TextCompositionEventArgs e)
        {
            short val;
            if (!Int16.TryParse(e.Text, out val))
            {
                // Disallow non-numeric key presses.
                e.Handled = true;
            }
        }

        /* Verification concordance MDP */
        private bool verifMDP(PasswordBox mdp)
        {
            bool isOk = false;
            if (mdp.Password.Length > 6)
            {
                isOk = true;
            }
            else
            {
                lab_mdpUser.Background = Brushes.Red;
                lab_mdpUser.Foreground = Brushes.White;
            }
            return isOk;
        }

        /* Verification des données utilisateur */
        private bool VerifUser()
        {
            bool isOk = true;
            // INT
            if ((txt_IDUser.Text.Length != 12))
            {
                isOk = false;
            }

            // MDP
            if (!verifMDP(mdpBoxUser))
            {
                isOk = false;
            }
            return isOk;
        }

        /* FONCTIONS PROPRE A LA PAGE */
        public Register()
        {
            InitializeComponent();
        }

        private void btn_prec_Click_1(object sender, RoutedEventArgs e)
        {
            Login L1 = new Login();
            L1.Show();
            Close();
        }

        private void btn_submitUser_Click(object sender, RoutedEventArgs e)
        {
            // Si l'utilisateur est bon on le crée
            if (VerifUser())
            {
                Login               L = new Login();
                Utilisateurs        U = new Utilisateurs(txt_IDUser.Text, mdpBoxUser.Password);
                ListUtilisateurs    LU = new ListUtilisateurs();
                U.CreerUtilisateur();
                LU.AjouterUtilisateur(U);
                L.Show();
                Close();
            }
            else
            {
                /* le bouton devient rouge */
                btn_submitUser.Background = Brushes.Red;
                MessageBox.Show("Certaines informations sont incorrectes.");
            }
        }
    }
}
