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
        /* envoie les erreurs */

        /* Verification 1ère page */
        private bool VerifInfoPatient()
        {
            bool isOk = true;
            // STRING 
            if (!VerifString(txt_nomPat, lab_nom))
            {
                isOk = false;
            }

            if (!VerifString(txt_prenomPat, lab_pre))
            {
                isOk = false;
            }

            // INT
            if (!VerifChiffre(txt_numINAMI, lab_inami) || (txt_numINAMI.Text.Length != 12))
            {
                isOk = false;
            }

            /*if (!VerifChiffre(txt_honoraire, lab_honoraire))
            {
                isOk = false;
            }*/

            if (!VerifChiffre(txt_numTelPat, lab_numTelPat) || (txt_numTelPat.Text.Length != 10))
            {
                isOk = false;
            }

            // DATE NAISSANCE
            if(date_naissancePat.SelectedDate == null)
            {
                lab_naissPat.Background = Brushes.Red;
                lab_naissPat.Foreground = Brushes.White;
                isOk = false;
            }

            // SPECIALITE
            if(comboB_Spec.SelectedValue == null)
            {
                lab_spec.Background = Brushes.Red;
                lab_spec.Foreground = Brushes.White;
                isOk = false;
            }
            return isOk;
        }

        /* INITIALISATION */
        void InitComboBoxPat()
        {
            // Combo sexe
            comboB_sexePat.Items.Add("Homme");
            comboB_sexePat.Items.Add("Femme");

            // Combo médecin
            comboB_medGeneraliste.Items.Add("Calicis Vincent");
        }

        /* FONCTIONS PROPRE A LA PAGE */
        public RegisterPatient()
        {
            InitializeComponent();
            InitComboBoxPat();
            Can_RegPat2.Visibility = Visibility.Hidden;
        }

        private void btn_prec_Click_1(object sender, RoutedEventArgs e)
        {
            Accueil A2 = new Accueil();
            A2.Show();
            Close();
        }

        private void btn_suiv_Click(object sender, RoutedEventArgs e)
        {
            Can_RegPat1.Visibility = Visibility.Hidden;
            Can_RegPat2.Visibility = Visibility.Visible;
        }

        private void btn_prec2_Click(object sender, RoutedEventArgs e)
        {
            Can_RegPat2.Visibility = Visibility.Hidden;
            Can_RegPat1.Visibility = Visibility.Visible;
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            if (VerifPremPage())
            {
                Accueil A1 = new Accueil();
                A1.Show();
                Close();
            }
            else
            {
                /* le bouton devient rouge */
                btn_submit.Background = Brushes.Red;
                Notification N2 = new Notification();
                N2.Show();
            }
        }
    }
}
