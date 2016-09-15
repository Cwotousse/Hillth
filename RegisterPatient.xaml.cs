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
    public partial class RegisterPatient : Window
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
        private bool VerifChiffre(TextBox intATester, Label motCouleur)
        {
            int elemInt;
            double elemeDouble;
            bool isOk = false;
            string isInt = intATester.Text.Replace(" ", string.Empty);
            if ((int.TryParse(intATester.Text, out elemInt) || double.TryParse(intATester.Text, out elemeDouble)) && intATester.Text.Length != 0)
            {
                isOk = true;
            }
            else
            {
                motCouleur.Background = Brushes.Red;
                motCouleur.Foreground = Brushes.White;
            }
            return isOk;
        }

        /* Verification 1ère page */
        private bool VerifInfoPatient()
        {
            bool isOk = true;

            // VERIFICATION NIS
            if (txt_NIS.Text.Length != 11 || !VerifChiffre(txt_NIS, lab_NIS))
            {
                isOk = false;
            }
            // STRING 
            if (!VerifString(txt_nomPat, lab_nomPat))
            {
                isOk = false;
            }

            if (!VerifString(txt_prenomPat, lab_prePat))
            {
                isOk = false;
            }

            if (!VerifChiffre(txt_numTelPat, lab_numTelPat) || (txt_numTelPat.Text.Length != 10))
            {
                isOk = false;
            }

            // DATE NAISSANCE
           if (date_naissancePat.SelectedDate == null)
            {
                lab_naissPat.Background = Brushes.Red;
                lab_naissPat.Foreground = Brushes.White;
                isOk = false;
            }

            // Sexe
            if (comboB_sexePat.SelectedValue == null)
            {
                lab_sexePat.Background = Brushes.Red;
                lab_sexePat.Foreground = Brushes.White;
                isOk = false;
            }

            // Medecin
            if (comboB_medGeneraliste.SelectedValue == null)
            {
                lab_medGeneraliste.Background = Brushes.Red;
                lab_medGeneraliste.Foreground = Brushes.White;
                isOk = false;
            }
            return isOk;
        }

        /* INITIALISATION */
        void InitComboBoxPat()
        {
            // Combo sexe
            comboB_sexePat.Items.Add("H");
            comboB_sexePat.Items.Add("F");
            comboB_sexePat.SelectedIndex = 1;

            // Recherche les hopitaux  la mutuelle dans la base de données 
            try
            {
                List<string> listElem = new List<string>();
                Mutuelle M = new Mutuelle();
                Hopital H = new Hopital();
                Medecin Me = new Medecin();
                listElem = M.DisplayMutuelle();
                foreach (string el in listElem)
                {
                    comboB_mut.Items.Add(el);
                }
                listElem = H.DisplayHopital();
                foreach (string el in listElem)
                {
                    comboB_hop.Items.Add(el);
                }
                List<String> listMedGen = new List<string>();
                listMedGen = Me.DisplayMedecinGeneraliste();
                foreach (string medGen in listMedGen)
                {
                    comboB_medGeneraliste.Items.Add(medGen);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /* FONCTIONS PROPRE A LA PAGE */
        public RegisterPatient()
        {
            InitializeComponent();
            InitComboBoxPat();
            Can_RegPat2.Visibility = Visibility.Hidden;
        }

        private void btn_precPat_Click_1(object sender, RoutedEventArgs e)
        {
            Accueil A2 = new Accueil();
            A2.Show();
            Close();
        }

        private void btn_suivPat_Click(object sender, RoutedEventArgs e)
        {
            Can_RegPat1.Visibility  = Visibility.Hidden;
            Can_RegPat2.Visibility  = Visibility.Visible;
            description.Content     = "Information médicale"; 
        }

        private void btn_precPat2_Click(object sender, RoutedEventArgs e)
        {
            Can_RegPat2.Visibility = Visibility.Hidden;
            Can_RegPat1.Visibility = Visibility.Visible;
        }

        private void btn_submitPat_Click(object sender, RoutedEventArgs e)
        {
            if (VerifInfoPatient())
            {
                // Ajoute du patient à la base de données
                string tiersP="";
                if (radBtn_tiersPayOui.IsChecked == true)
                    tiersP = "Oui";
                else if (radBtn_tiersPayNon.IsChecked == true)
                    tiersP = "Non";
                ListFichePatient LFP    = new ListFichePatient();
                FichePatient FP         = new FichePatient(txt_nomPat.Text, txt_prenomPat.Text, txt_numTelPat.Text,
                            comboB_sexePat.SelectionBoxItem.ToString(), txt_adrPat.Text, date_naissancePat.SelectedDate.Value,
                            txt_NIS.Text, txt_allergies.Text, tiersP, comboB_mut.SelectionBoxItem.ToString(),
                            comboB_hop.SelectionBoxItem.ToString());
                LFP.AjouterPatient(FP);
                FP.CreerPatient(comboB_medGeneraliste.SelectionBoxItem.ToString());
                Accueil A1 = new Accueil();
                A1.Show();
                Close();
            }
            else
            {
                /* le bouton devient rouge */
                btn_submitPat.Background = Brushes.Red;
                MessageBox.Show("Certaines informations sont incorrectes.");
            }
        }

        /* Verifictation entier, object car on ne connait pas le type */
        private void txt_NIS_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {    
            short val;
            if (!Int16.TryParse(e.Text, out val))
            {
                // Disallow non-numeric key presses.
                e.Handled = true;
            }
        }
    }
}