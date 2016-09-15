using System;
using System.Collections.Generic;
using System.Data;
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
using WindowsInput;
using WindowsInput.Native;

namespace premierTest
{
    /// <summary>
    /// Logique d'interaction pour PatientRecherche.xaml
    /// </summary>
    public partial class PatientRecherche : Window
    {
        public PatientRecherche()
        {
            InitializeComponent();
        }

        private void btn_Ret_Click(object sender, RoutedEventArgs e)
        {
            Accueil A = new Accueil();
            A.Show();
            Close();
        }

        private void btn_AddPat_Click(object sender, RoutedEventArgs e)
        {
            RegisterPatient P = new RegisterPatient();
            Medecin M = new Medecin();
            if (M.MedGedDejaCree())
            {
                P.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Veuillez ajouter un médecin généraliste avant d'ajouter des patients");
            }
        }

        // Suppression
        private void btn_SuppPat_Click(object sender, RoutedEventArgs e)
        {
            FichePatient FP = new FichePatient();
            ListFichePatient LFP = new ListFichePatient();
            FP.Delete(txtB_NISDelete.Text);
            LFP.SupprimerPatient(FP);
        }

        // Permet de modifier la personne choisie
        private void btn_ModifierPat_Click(object sender, RoutedEventArgs e)
        {
            FichePatient FP = new FichePatient();
            FP.ModifierPatient(txtB_NISDelete.Text, txtB_NomModif.Text, txtB_PreModif.Text,
                    comboB_SexeModif.SelectionBoxItem.ToString(), comboB_TPModif.SelectionBoxItem.ToString());
        }

        // Quitter le programme
        private void btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LV_Patient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FichePatient FP = new FichePatient();

            txtB_NISDelete.Text = "";
            int cpt = 0;
            List<char> test = LV_Patient.SelectedItems[0].ToString().ToList() ;
            foreach (char s in test)
            {
                cpt++;
                // Stocke le numero NIS de la ligne sélectionnée pour pouvoir le modifier / supprimer.
                if (cpt < 20 && cpt > 8)
                {
                    txtB_NISDelete.Text += s;
                }
            }

            // Affiche les valeurs si la personne veut le modifier + le canva + change la couleur du bouton
            string nom, pre;

            //FP.InitValeursModifPatient(txtB_NISDelete.Text, out nom, out pre);
            InitValeursModifPat(FP.InitValeursModifPatient(txtB_NISDelete.Text, out nom, out pre), nom, pre);
            btn_ModifierPat.Visibility  = Visibility.Visible;
            btn_SuppPat.Visibility      = Visibility.Visible;
            Can_Modif.Visibility        = Visibility.Visible;
        }

        private void InitValeursModifPat(List<string> listElem, string nom, string pre )
        {
            // On nettoie les combobox avant tout
            comboB_SexeModif.Items.Clear();
            comboB_TPModif.Items.Clear();
            comboB_DHModif.Items.Clear();

            comboB_SexeModif.Items.Add("H");
            comboB_SexeModif.Items.Add("F");
            comboB_TPModif.Items.Add("Oui");
            comboB_TPModif.Items.Add("Non");

            // hopital
            foreach(string el in listElem)
            {
                comboB_DHModif.Items.Add(el);
            }

            txtB_NomModif.Text = nom;
            txtB_PreModif.Text = pre;
        }
    }
}
