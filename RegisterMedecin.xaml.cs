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
    public partial class RegisterMedecin : Window
    {
        /* envoie les erreurs */
        /* Fonction vérifie ce qui est transmis en paramètre est bon*/
        private bool VerifString(TextBox motATester, Label motCouleur)
        {
            bool isString = true;
            if (motATester.Text.Length == 0)
            {
                isString = false;
                motCouleur.Background = Brushes.Red;
                motCouleur.Foreground = Brushes.White;
            }
            else
            {
                foreach (char c in motATester.Text)
                {
                    if (!char.IsLetter(c))
                    {
                        isString = false;
                        /* Les mauvaises informations sont en rouge */
                        motCouleur.Background = Brushes.Red;
                        motCouleur.Foreground = Brushes.White;
                    }
                }
            }
            return isString;
        }

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

        /* Verification 1ère page */
        private bool VerifPremPage()
        {
            bool isOk = true;
            // STRING 
            if (!VerifString(txt_nom, lab_nom))
            {
                isOk = false;
            }

            if (!VerifString(txt_prenom, lab_pre))
            {
                isOk = false;
            }

            if (txt_numINAMI.Text.Length != 12)
            {
                isOk = false;
            }

            // INT
            if ((txt_numTel.Text.Length != 10))
            {
                isOk = false;
                lab_numTel.Background = Brushes.Red;
                lab_numTel.Foreground = Brushes.White;
            }

            // DATE NAISSANCE
            if (date_naissance.SelectedDate == null)
            {
                lab_naiss.Background = Brushes.Red;
                lab_naiss.Foreground = Brushes.White;
                isOk = false;
            }

            // SPECIALITE
            if (comboB_Spec.SelectedValue == null)
            {
                lab_spec.Background = Brushes.Red;
                lab_spec.Foreground = Brushes.White;
                isOk = false;
            }
            return isOk;
        }

        /* INITIALISATION */
        void InitComboBox()
        {
            HillthDataClassDataContext HDB = new HillthDataClassDataContext();
            var query = from s in HDB.Specialisation select s.LibelSpec;
            foreach (string s in query)
            {
                comboB_Spec.Items.Add(s);
            }

            // Combo sexe
            comboB_sexeMed.Items.Add("H");
            comboB_sexeMed.Items.Add("F");
            comboB_sexeMed.SelectedIndex = 1;

            // Combo convention
            comboB_convention.Items.Add("Oui");
            comboB_convention.Items.Add("Non");
            comboB_convention.SelectedIndex = 1;
        }

        /* FONCTIONS PROPRE A LA PAGE */
        public RegisterMedecin()
        {
            InitializeComponent();
            InitComboBox();
        }

        private void btn_prec_Click_1(object sender, RoutedEventArgs e)
        {
            Login L1 = new Login();
            L1.Show();
            Close();
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            if (VerifPremPage())
            {
                Accueil A1      = new Accueil();
                ListMedecin LM  = new ListMedecin();
                Medecin M       = new Medecin(txt_nom.Text, txt_prenom.Text, date_naissance.SelectedDate.Value,
                    comboB_sexeMed.SelectionBoxItem.ToString(), txt_numINAMI.Text, txt_adr.Text, txt_numTel.Text,
                    comboB_Spec.SelectedIndex, comboB_convention.SelectionBoxItem.ToString());
                LM.AjouterMedecin(M);
                // this car c'est dans cette window
                M.CreerMedecin();
                A1.Show();
                Close();
            }
            else
            {
                /* le bouton devient rouge */
                btn_submit.Background = Brushes.Red;
                MessageBox.Show("Certaines informations sont incorrectes.");
            }
        }
    }
}
