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
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Window
    {
        // Utilisé pour garder  les informations de la personne connectée
        private string USRID;

        public string _USRID
        {
            get { return USRID; }
            set { USRID = value; }
        }

        public Accueil()
        {
            InitializeComponent();
            /* on cache le menu latéral */
            rect_contenu.Visibility = Visibility.Hidden;
        }

        /* RETOUR AU LOGIN */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login L1 = new Login();
            L1.Show();
            Close();
        }

        /* SE DECONNECTER */
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /* AFFICHE OU CACHE LE BOUTON LATERAL */
        private void toggle_Menu_Checked(object sender, RoutedEventArgs e)
        {
           Can_MenuLat.Visibility = Visibility.Visible;
        }

        private void toggle_Menu_Unchecked(object sender, RoutedEventArgs e)
        {
            Can_MenuLat.Visibility = Visibility.Hidden;
        }

        /* PROFIL */
        // Affiche les informations de l'utilisateur
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            img_Logo.Visibility         = Visibility.Hidden;
            Can_Profil.Visibility       = Visibility.Visible;
            Can_Recherche.Visibility    = Visibility.Hidden;
            Can_Stat.Visibility         = Visibility.Hidden;
            rect_contenu.Visibility     = Visibility.Visible;
            string nom, spec, tel, inami, adresse;
            Medecin M = new Medecin();
            M.AfficherProfil(_USRID, out nom, out spec, out tel, out inami, out adresse);
            txt_Nom.Text        = nom;
            txt_Spec.Text       = spec;
            txt_NumTel.Text     = tel;
            txt_NumINAMI.Text   = inami;
            txt_Adr.Text        = adresse;
        }

        /* RECHERCHER  */
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FichePatient FP = new FichePatient();
            if (FP.VerifExistencePatient())
            {
                List<string> listElem = new List<string>();
                rect_contenu.Visibility     = Visibility.Visible;
                Can_Recherche.Visibility    = Visibility.Visible;
                img_Logo.Visibility         = Visibility.Hidden;
                Can_Profil.Visibility       = Visibility.Hidden;
                Can_Stat.Visibility         = Visibility.Hidden;

                // Chargement des comboBox
                //FP.InitComboRecherche(this);
                Mutuelle M = new Mutuelle();
                Hopital H = new Hopital();
                listElem = M.DisplayMutuelle();
                foreach (string el in listElem)
                {
                    ComboB_Mut.Items.Add(el);
                }
                listElem = H.DisplayHopital();
                foreach (string el in listElem)
                {
                    ComboB_Hop.Items.Add(el);
                }
            }
            else
            {
                MessageBox.Show("Il n'y a actuellement aucuns patients enregistrés");
            }

        }

        // BOUTON ANNULER  
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            txt_NomRep.Clear();
            txt_PreRep.Clear();
            txt_IDRep.Clear();
            // Clean les valeurs sélectionnées
        }

        // BOUTON DE RECHERCHE REQUETES SQL
        private void btn_rech_Click(object sender, RoutedEventArgs e)
        {
            FichePatient FP     = new FichePatient();
            PatientRecherche PR = new PatientRecherche();
            string rechercheMut = "";
            string rechercheHop = "";

            // Obligé de faire 2 try catch sinon le dernier if n'est jamais pris en compte...
            try
            {
                if (ComboB_Mut.SelectedValue.ToString() != "")
                {
                    rechercheMut = ComboB_Mut.SelectedValue.ToString();
                }
            }
            catch {}
            try
            {
                if (ComboB_Hop.SelectedValue.ToString() != "")
                {
                    rechercheHop = ComboB_Hop.SelectedValue.ToString();
                }
            }
            catch {}
            List<Object> listSource = new List<object>();
            listSource = FP.RecherchePatient(txt_NomRep.Text, txt_PreRep.Text, txt_IDRep.Text, rechercheMut, rechercheHop);
            if (listSource.Count != 0)
            {
                // Cache et affiche la fenetre actuelle et sort les informations issues de la recherche s'il y en a
                PR.LV_Patient.ItemsSource = listSource;
                PR.Show();
                Close();
            }
            else
            {
                PR.Close();
            }
        }

        // AFFICHE TOUT LE MONDE
        private void btn_AffTout_Click(object sender, RoutedEventArgs e)
        {
            FichePatient FP     = new FichePatient();
            PatientRecherche PR = new PatientRecherche();
            List<Object> listElem = new List<Object>();
            listElem = FP.DisplayPatient();
            PR.LV_Patient.ItemsSource = listElem;
            PR.Show();
            Close();
        }

        // STATISTIQUES 
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            rect_contenu.Visibility     = Visibility.Visible;
            Can_Stat.Visibility         = Visibility.Visible;
            Can_Profil.Visibility       = Visibility.Hidden;
            Can_Recherche.Visibility    = Visibility.Hidden;
            img_Logo.Visibility         = Visibility.Hidden;

            FichePatient FP = new FichePatient();
            Medecin M       = new Medecin();
            Mutuelle Mu     = new Mutuelle();

            // Affiche le nombre de patients
            try
            {
                txtB_nbrPat.Text = "Il y a actuellement " + FP.CptPatient() + " patient(s) enregistré(s).";
            }
            catch
            {
                txtB_nbrPat.Text = "Il y a actuellement 0 patient enregistré.";
            }
            

            // Pourcentage homme et femme
            try
            {
                txtB_PourcHoFe.Text = "Patients masculins : "
                + Convert.ToInt32(FP.CptHommeFemme( "H")) * 100 / Convert.ToInt32(FP.CptPatient())
                + "% [" + FP.CptHommeFemme("H") + "] \n"
                + "Patients féminins : " +
                +Convert.ToInt32(FP.CptHommeFemme("F")) * 100 / Convert.ToInt32(FP.CptPatient())
                + "% [" + FP.CptHommeFemme("F") + "]";
            }
            catch
            {
                txtB_PourcHoFe.Text = "Aucun patient enregistré.";
            }


            // Les mutuelles et leurs nombre d'affiliés
            try
            {
                txtB_PourcMut.Text = "";
                for (int i = 0; i < Mu.CptMutuelle().Count; i++)
                {
                    txtB_PourcMut.Text += Mu.CptMutuelle()[i];
                }
            }
            catch
            {
                txtB_PourcMut.Text = "Aucun patient enregistré";
            }


            // Affiche le nombre de médecins
            try
            {
                txtB_nbrDoc.Text = "Il y a actuellement " + M.CptMedecin() + " médecin(s) enregistré(s).";
            }
            catch
            {
                txtB_nbrDoc.Text = "Aucun médecin enregistré.";
            }
            
        }


        // AJOUTER UN NOUVEAU PATIENT
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Medecin M = new Medecin();
            if (M.MedGedDejaCree())
            {
                RegisterPatient R1 = new RegisterPatient();
                R1.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Veuillez ajouter un médecin généraliste avant d'ajouter des patients");
            }

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            RegisterMedecin RM = new RegisterMedecin();
            RM.Show();
            Close();
        }

        // Lance la vidéo du médecin généraliste
        private void toggle_Video_Checked(object sender, RoutedEventArgs e)
        {
            
            video_medecin.Visibility = Visibility.Visible;
            video_medecin.Play();
        }

        // Vidéo désactivée
        private void toggle_Video_Unchecked(object sender, RoutedEventArgs e)
        {
            video_medecin.Pause();
            video_medecin.Visibility = Visibility.Hidden;
        }
    }
}