 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;

namespace premierTest
{
    partial class FichePatient
    {
        HillthDataClassDataContext HDB = new HillthDataClassDataContext();
        private string nomPatient;
        private string prePatient;
        private string numTelPatient;
        private DateTime dateNaissancePatient;
        private string sexePatient;
        private string adressePatient;
        private string nisPatient;
        private string allergiesPatient;
        private string tiersPayantPatient;
        private int idMutuellePatient;
        private int idHopitalPatient;

        public string NomPatient
        {
            get { return nomPatient; }
            set { nomPatient = value; }
        }

        public string PrePatient
        {
            get { return prePatient; }
            set { prePatient = value; }
        }

        public string NumTelPatient
        {
            get { return numTelPatient; }
            set { numTelPatient = value; }
        }

        public DateTime DateNaissancePatient
        {
            get { return dateNaissancePatient; }
            set { dateNaissancePatient = value; }
        }

        public string SexePatient
        {
            get { return sexePatient; }
            set { sexePatient = value; }
        }

        public string NisPatient
        {
            get { return nisPatient; }
            set { nisPatient = value; }
        }

        public string AllergiesPatient
        {
            get { return allergiesPatient; }
            set { allergiesPatient = value; }
        }

        public string TiersPayantPatient
        {
            get { return tiersPayantPatient; }
            set { tiersPayantPatient = value; }
        }

        public string AdressePatient
        {
            get { return adressePatient; }
            set { adressePatient = value; }
        }

        public int IDMutuellePatient
        {
            get { return idMutuellePatient; }
            set { idMutuellePatient = value; }
        }

        public int IDHopitalPatient
        {
            get { return idHopitalPatient; }
            set { idHopitalPatient = value; }
        }

        // CONSTRUCTEUR 
        public FichePatient(string nomPatient, string prePatient, string numTelPatient, string sexePatient,
            string adresse, DateTime dateNaissancePatient, string nisPatient, string allergiesPatient,
            string tiersPayantPatient, string libelMut, string libelHopital)
        {
            this.nomPatient             = nomPatient;
            this.prePatient             = prePatient;
            this.numTelPatient          = numTelPatient;
            this.dateNaissancePatient   = dateNaissancePatient;
            this.sexePatient            = sexePatient;
            this.adressePatient         = adresse;
            this.nisPatient             = nisPatient;
            this.allergiesPatient       = allergiesPatient;
            this.tiersPayantPatient     = tiersPayantPatient;
            // Pour les combo box il faut retourner l'ID et non le string on doit donc le récupérer
            this.idMutuellePatient      = Mutuelle.AfficherLibelMut(libelMut);
            this.idHopitalPatient       = Hopital.AfficherLibelHop(libelHopital);
        }

        // fonction permettant de créer un patient 
        public void CreerPatient(string medecinGen)
        {
            try
            {
                // Ajout de la personne dans fichePatient
                Medecin             M  = new Medecin();
                HistoriquePatient   HP = new HistoriquePatient(M.retourneNumINAMI(medecinGen), nisPatient, DateTime.Today.Date);
                // Medecin -  idpatient - date ajout 
                // Le numero inami 00...0 est celui par défaut, permet de voir si le médecin existe vraiment
                if (M.retourneNumINAMI(medecinGen) != "000000000000")
                {
                    //Appel de la dataclass (base de données)
                    using (HillthDataClassDataContext HDB = new HillthDataClassDataContext())
                    {
                        // Insertion dans la DC
                        HDB.InsertPatient(nomPatient, prePatient, numTelPatient, sexePatient, adressePatient, dateNaissancePatient,
                            nisPatient, allergiesPatient, tiersPayantPatient, idMutuellePatient, idHopitalPatient);
                        HP.AjoutHistorique();
                        // Insere dans la DB
                        HDB.SubmitChanges();
                    }
                    MessageBox.Show("Ajout effectué.");
                }
                else
                {
                    MessageBox.Show("Vous devez ajouter un médecin généraliste.\n Enregistrement annulé.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Une erreur est apparue, enregistrement annulé. \n " + e.Message);
            }
        }   

        public List<Object> DisplayPatient()
        {
            // On insère les résultats dans la liste
            List<Object> listSource = new List<Object>();
            // On verifie que la DB existe bien
            if (HDB.DatabaseExists())
            {
                // fin page 574 livre
                IQueryable<object> query =
                from fiche in HDB.FichePatient
                join hop in HDB.Hopital on fiche.IDHopital equals hop.IDHopital
                join mut in HDB.Mutuelle on fiche.IDMutuelle equals mut.IDMutuelle
                join hp  in HDB.HistoriquePatient on fiche.NIS equals hp.NIS
                join med in HDB.Medecin on hp.IDMedecin equals med.IDMedecin
                select new
                {
                    fiche.NIS,
                    fiche.Nom,
                    fiche.Prenom,
                    fiche.Sexe,
                    fiche.Allergies,
                    fiche.numTel,
                    fiche.TiersPayant,
                    hop.LibelHopital,
                    mut.LibelMutuelle,
                    nomMed = med.Nom
                };
                // -> nomMed = med.nom est utilisé car deux attributs possèdent le nom "Nom".
                // Le resultat de la requête est inséré dans la liste
                listSource = query.ToList();
                //https://msdn.microsoft.com/fr-fr/library/ee340709%28v=vs.110%29.aspx
            }
            return listSource;
        }

        // Même fonctionnement que pour DisplayPatient
        public List<Object> RecherchePatient (string nom, string prenom, string NIS, string mutRech, string hopRech)
        {
            List<Object> listSource = new List<object>();
            //https://msdn.microsoft.com/fr-be/library/bb882535.aspx
            if (nom != "" || prenom != "" || NIS != "" || mutRech != "" || hopRech != "")
            {
                IQueryable<object> queryRech =
                from fiche in HDB.FichePatient
                where fiche != null
                //join hist in HDB.HistoriquePatient on fiche.NIS equals hist.NIS
                //join med in HDB.Medecin on hist.NumINAMI equals med.NumINAMI
                join hop in HDB.Hopital on fiche.IDHopital equals hop.IDHopital
                join mut in HDB.Mutuelle on fiche.IDMutuelle equals mut.IDMutuelle
                join hp in HDB.HistoriquePatient on fiche.NIS equals hp.NIS
                join med in HDB.Medecin on hp.IDMedecin equals med.IDMedecin
                where (fiche.Nom == nom && nom != "")  || (fiche.Prenom == prenom && prenom != "")
                    || (fiche.NIS == NIS && NIS != "") || (mut.LibelMutuelle == mutRech && mut != null)
                    || (hop.LibelHopital== hopRech && hop != null)
                //orderby fiche.Nom ascending
                select new
                {
                    fiche.NIS,
                    fiche.Nom,
                    fiche.Prenom,
                    fiche.Sexe,
                    fiche.Allergies,
                    fiche.numTel,
                    fiche.TiersPayant,
                    hop.LibelHopital,
                    mut.LibelMutuelle,
                    nomMed = med.Nom
                };
                listSource = queryRech.ToList();
                // s'il  y a au moins un élément choisi
                if (queryRech.Count() == 0)
                {
                    MessageBox.Show("Aucune correspondance", "Erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            else
            {
                MessageBox.Show("Veuillez entrer des valeurs", "Erreur", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            // Permet de savoir l'état de la requête 
            return listSource;
        }

        // fonction permettant de modifier certaines valeurs (nom, prenom, sexe, tiers payant) d'un patient
        public void ModifierPatient(string nisModify, string nom, string pren, string sexe, string tp)
        {
            try
            {
                HDB.ModifierPatient(nom, pren, sexe, tp, nisModify);
                HDB.SubmitChanges();
                MessageBox.Show("Modification effectuée !");
            }
            catch(Exception e)
            {
                MessageBox.Show("Erreur\n" + e.Message);
            }
        }

        // Initialise les valeurs pouvant être modifiée de la personne sélectionnée dans les textbox prévus à l'occasion.
        public List<string> InitValeursModifPatient(string nisPat, out string nom, out string pre)
        {
            // nom et prenom sont transmis en référence car ils vont ressortir modifiés et n'ont pas besoin d'etre init au préalable
            nom = pre = "";
            try
            {
                // Initialiser les textbox avec les valeurs de la personne sélectionnée pour pouvoir la modifier.
                var initValues = (from d in HDB.FichePatient where d.NIS == nisPat select d).First();
                nom = initValues.Nom.ToString();
                pre = initValues.Prenom.ToString();

                // Initialiser la combobox avec le nom des hopitaux
                List<string> listHop = new List<string>();
                Hopital H = new Hopital();
                return H.DisplayHopital();
            }
            catch (Exception e)
            {
                MessageBox.Show("Veuillez sélectionner une personne.\n" + e.Message);
                return null;
            }
        }

        // Suppression d'un patient
        public void Delete(string nisDelete)
        {      
            try
            {
                // Procédure stockée de suppression
                HDB.DeletePatient(nisDelete);
                HDB.SubmitChanges();
                MessageBox.Show("Le patient a bien été effacé.");
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur\n" + e.Message);
            }
        }

        // Compte le nombre de patients dans la base de données
        public string CptPatient()
        {
            return (from a in HDB.FichePatient select a.NIS).Count().ToString();
        }

        // Compte le nombre de femmes/homme (selon paramètre) dans la base de données
        public string CptHommeFemme(string type)
        {
            return (from a in HDB.FichePatient where a.Sexe == type select a.Sexe).Count().ToString();
        }

        // Verifie qu'il y a au moins un patient enregistré
        public bool VerifExistencePatient()
        {
            try
            {
                bool existe = false;
                var query = from p in HDB.FichePatient select p;
                // Si la requete retourne vrai il existe au moins une personne correspondant aux critères
                if (query.Any())
                {
                    existe = true;
                }
                return existe;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}
