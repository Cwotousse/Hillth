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
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private bool isAlreadyClicked = false;
        public Login()
        {
            InitializeComponent();
        }

        private void btn_quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_enreg_Click(object sender, RoutedEventArgs e)
        {
            Register R1 = new Register();
            R1.Show();
            Close();
        }

        // Si on appuie sur enter ou sur le bouton il tente la connexion 
        private void btn_co_Click(object sender, RoutedEventArgs e)
        {
            Connexion();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Connexion();
            }
        }

        private void Connexion()
        {
            Accueil A1 = new Accueil();
            // statique ainsi on peut directement l'appeler avec la classe sans constructeur
            if (Utilisateurs.LoginUser(txt_login.Text, txt_mdp.Text))
            {
                A1.Show();
                // Garde l'id de l'utilisateur connecté
                A1._USRID = txt_login.Text;
                // Affiche le nom du médecin dans l'encadré
                Medecin M = new Medecin();
                M.AfficherNomMed(A1);
                this.Close();
            }
            else
            {
                MessageBox.Show("Mot de passe et/ou n° fourni incorrect.");
                A1.Close();
            }
        }

        // Efface ce qui est écrit
        private void txt_login_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!isAlreadyClicked)
            {
                isAlreadyClicked = true;
                txt_login.Text  = "";
                txt_mdp.Text    = "";
            }
        }
    }
}
